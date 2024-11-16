using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageBoard.Maui.Models;

namespace MessageBoard.Maui
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Message>().Wait();
            _database.CreateTableAsync<User>().Wait(); // Ensure User table is created
        }

        public Task<int> SaveMessageAsync(Message message)
        {
            return _database.InsertAsync(message);
        }

        public Task<List<Message>> GetMessagesAsync()
        {
            return _database.Table<Message>().ToListAsync();
        }

        public Task<int> DeleteAllMessagesAsync()
        {
            return _database.DeleteAllAsync<Message>();
        }

        public Task<int> UpdateMessageAsync(Message message)
        {
            return _database.UpdateAsync(message);
        }

        public Task<int> DeleteMessageAsync(Message message)
        {
            return _database.DeleteAsync(message);
        }

        // New methods for user management
        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserAsync(string username)
        {
            return _database.Table<User>().FirstOrDefaultAsync(u => u.UserName == username);
        }

        public Task<User> GetUserByUserNameAsync(string username)
        {
            return _database.Table<User>().Where(u => u.UserName == username).FirstOrDefaultAsync();
        }

    }
}
