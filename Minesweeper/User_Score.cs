

using SQLite;

namespace Minesweeper
{
    [Table("Score")]
    class User_Score
    {
        public string Name { get; set; }
        public int Time { get; set; }

        public User_Score() { }

        public User_Score(string Name, int Time) {

            this.Name = Name;
            this.Time = Time;
        }
    }
}
