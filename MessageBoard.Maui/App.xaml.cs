namespace MessageBoard.Maui
{
    public partial class App : Application
    {
        private static DatabaseHelper _database;

        public static DatabaseHelper Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MessageBoard.db3");
                    _database = new DatabaseHelper(dbPath);
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            // Check if the user is logged in
            if (IsUserLoggedIn())
            {
                // If logged in, show the message board page (or whichever page is your main page)
                MainPage = new NavigationPage(new MainPage()); // Replace with your main message board page
            }
            else
            {
                // If not logged in, show the login page
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        // This method checks if the user is logged in
        private bool IsUserLoggedIn()
        {
            // Check if there's a flag in Preferences indicating that the user is logged in
            return Preferences.Get("IsUserLoggedIn", false); // Default is false, meaning not logged in
        }
    }
}
