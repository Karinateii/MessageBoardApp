namespace MessageBoard.Maui.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } // Store a hashed version for security
        public string ProfilePicture { get; set; } // Path or URL to the image
    }
}
