using Microsoft.Maui.Controls;
using System.Text;

namespace MessageBoard.Maui
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            // Here, retrieve the user details from your database
            var user = await App.Database.GetUserByUserNameAsync(UsernameEntry.Text);

            if (user != null && PasswordUtils.ValidatePassword(PasswordEntry.Text, user.PasswordHash))
            {
                // Save the username to Preferences for future use
                Preferences.Set("UserName", user.UserName);

                // Set the login status to true
                Preferences.Set("IsUserLoggedIn", true);

                // Navigate to the main page
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }


        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the RegistrationPage for new users
            await Navigation.PushAsync(new RegistrationPage());
        }

        private async void OnNavigateToRegisterClicked(object sender, EventArgs e)
        {
            // Navigate to the RegistrationPage
            await Navigation.PushAsync(new RegistrationPage());
        }

    }
}
