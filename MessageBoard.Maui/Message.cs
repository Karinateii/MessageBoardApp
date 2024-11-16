using SQLite;
using System;

namespace MessageBoard.Maui
{
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
    }
}
