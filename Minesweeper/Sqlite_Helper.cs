using System.Collections.Generic;

using SQLite;

namespace Minesweeper
{
    class Sqlite_Helper {

        SQLiteConnection DB;

        public Sqlite_Helper(string Path) {

            // Making The Connection With The DataBase, Base On The Given Path.

            string Full_Path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), Path);
            DB = new SQLiteConnection(Full_Path);
            DB.CreateTable<User_Score>();
        }

        public void Add_User(User_Score User) {

            DB.Insert(User);
        }

        public List<string> Load_Users() {

            List<string> Users = new List<string>();

            string SQL_Query = string.Format("SELECT * FROM Score ORDER BY Time LIMIT 10"); // Loading Only The Top 10 Players.
            var DB_Results = DB.Query<User_Score>(SQL_Query);


            if (DB_Results.Count > 0) {

                foreach (var Temp in DB_Results) {

                    User_Score User = (User_Score)Temp;
                    Users.Add(User.Name + " : " + User.Time);
                }
            }

            return Users;
        }

    }
}