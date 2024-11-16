using MessageBoard.Maui.Models;
using Microsoft.Maui.Controls;
using System;

namespace MessageBoard.Maui
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            // Hash the password using the PasswordUtils utility
            var passwordHash = PasswordUtils.ComputeHash(PasswordEntry.Text);

            var user = new User
            {
                UserName = UserNameEntry.Text,
                PasswordHash = passwordHash
            };

            // Save the user to the database (implement this logic in your database helper class)
            await App.Database.SaveUserAsync(user);

            await DisplayAlert("Success", "Registration complete. Please log in.", "OK");
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnGoToLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
