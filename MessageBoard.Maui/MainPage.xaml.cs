using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.IO;
using System.Threading.Tasks;

namespace MessageBoard.Maui
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Message> messages;
        private DatabaseHelper databaseHelper;

        public MainPage()
        {
            InitializeComponent();

            // Set the binding context to this page so that 'messages' is accessible in XAML
            this.BindingContext = this;

            // Get the database path
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "messages.db");
            databaseHelper = new DatabaseHelper(dbPath);

            // Load messages from the database
            LoadMessagesFromDatabaseAsync().ConfigureAwait(false);
        }


        private async Task LoadMessagesFromDatabaseAsync()
        {
            var messageList = await databaseHelper.GetMessagesAsync();
            messages = new ObservableCollection<Message>(messageList);
            MessagesListView.ItemsSource = messages;
        }

        private async void OnPostButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a message", "OK");
                return;
            }

            // Retrieve the logged-in user's username from Preferences
            var userName = Preferences.Get("UserName", string.Empty);

            if (string.IsNullOrEmpty(userName))
            {
                await DisplayAlert("Error", "You must be logged in to post a message.", "OK");
                return;
            }

            // Create a new message with the username and message text
            var message = new Message
            {
                Text = MessageEntry.Text,
                UserName = userName,  // Add the username to the message
                Timestamp = DateTime.Now
            };

            // Save the message to the database
            await databaseHelper.SaveMessageAsync(message);

            // Clear the message input field
            MessageEntry.Text = string.Empty;

            // Directly update the ObservableCollection without reloading from the database
            messages.Insert(0, message); // Adds the new message to the beginning of the list
        }




        private async void OnClearAllButtonClicked(object sender, EventArgs e)
        {
            await databaseHelper.DeleteAllMessagesAsync();
            messages.Clear();
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var messageToEdit = button?.CommandParameter as Message;

            if (messageToEdit != null)
            {
                // Prompt the user to edit the message text
                string updatedText = await DisplayPromptAsync("Edit Message", "Update the message text:", initialValue: messageToEdit.Text);

                if (!string.IsNullOrWhiteSpace(updatedText))
                {
                    messageToEdit.Text = updatedText;
                    await databaseHelper.UpdateMessageAsync(messageToEdit);

                    // Refresh the ObservableCollection to reflect the change
                    var index = messages.IndexOf(messageToEdit);
                    if (index >= 0)
                    {
                        messages[index] = messageToEdit;
                    }
                }
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var messageToDelete = button?.CommandParameter as Message;

            if (messageToDelete != null)
            {
                bool confirmDelete = await DisplayAlert("Delete Message", "Are you sure you want to delete this message?", "Yes", "No");
                if (confirmDelete)
                {
                    await databaseHelper.DeleteMessageAsync(messageToDelete);
                    messages.Remove(messageToDelete);
                }
            }
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            // Set the IsUserLoggedIn flag to false to log the user out
            Preferences.Set("IsUserLoggedIn", false);

            // Navigate back to the login page
            await Navigation.PushAsync(new LoginPage());
        }


    }
}
