using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Minesweeper
{
    [Activity(Label = "Winning_Activity")]
    public class Winning_Activity : Activity, Android.Views.View.IOnClickListener {

        string Difficulty;
        string Time;

        protected override void OnCreate(Bundle savedInstanceState) {

            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.winning_layout);

            Difficulty = Intent.GetStringExtra("Level_Difficulty");
            Time = Intent.GetStringExtra("Time");

            TextView Winner_TextView = FindViewById<TextView>(Resource.Id.Winner_TextView);
            Winner_TextView.Text = "Congratulations !!!" + "\n" + "You Have Won On " + Difficulty + "\n" + "Within : " + Time + " Seconds.";

            Button Save_Button = FindViewById<Button>(Resource.Id.Save_Button);
            Save_Button.SetOnClickListener(this);
        }

        public void OnClick(View View) {

            if (View.Id == Resource.Id.Save_Button) { // User Clicked On The Save Button.

                Save_User(); // Saving The User In The DB.

                Intent Intent = new Intent(this, typeof(Main_Menu_Activity)); // Changing The Intent To The Main Menu.
                StartActivity(Intent);
            }
        }

        public void Save_User() {

            // Creating The User_Score Object.

            EditText Player_Name_EditText = FindViewById<EditText>(Resource.Id.Player_Name_EditText);
            User_Score User = new User_Score(Player_Name_EditText.Text, int.Parse(Time));

            // SQLite

            Sqlite_Helper Sqlite_Helper = new Sqlite_Helper("Top10 " + Difficulty);
            Sqlite_Helper.Add_User(User); // Adding The User To The DB.
        }

        public override void OnBackPressed() {

            // Removing The Back Click.
            //base.OnBackPressed();
        }

    }
}