
using SQLite;

namespace BlockBlast.Models
{
    public class GameRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string PlayerName { get; set; } = "Player";

        public int Score { get; set; }

        public DateTime PlayedAt { get; set; } = DateTime.Now;
    }
}
