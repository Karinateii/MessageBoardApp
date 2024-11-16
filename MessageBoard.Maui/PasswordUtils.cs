using System.Security.Cryptography;
using System.Text;

namespace MessageBoard.Maui
{
    public static class PasswordUtils
    {
        public static string ComputeHash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        // Method to validate if the entered password matches the stored hash
        public static bool ValidatePassword(string enteredPassword, string storedPasswordHash)
        {
            // Hash the entered password and compare it to the stored hash
            var enteredPasswordHash = ComputeHash(enteredPassword);
            return enteredPasswordHash == storedPasswordHash;
        }
    }
}
