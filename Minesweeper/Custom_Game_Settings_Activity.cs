using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Minesweeper
{
    [Activity(Label = "Custom_Game_Settings_Activity")]
    public class Custom_Game_Settings_Activity : Activity, Android.Views.View.IOnClickListener {

        protected override void OnCreate(Bundle savedInstanceState) {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.custom_game_settings);


            Button Play_Button = FindViewById<Button>(Resource.Id.Play_Button);

            Play_Button.SetOnClickListener(this);
        }

        public void OnClick(View View) {

            if (View.Id == Resource.Id.Play_Button) {

                EditText Grid_Width_EditText = FindViewById<EditText>(Resource.Id.Grid_Width_EditText);
                EditText Grid_Height_EditText = FindViewById<EditText>(Resource.Id.Grid_Height_EditText);
                EditText Number_Of_Mines_EditText = FindViewById<EditText>(Resource.Id.Number_Of_Mines_EditText);

                int a; // For TryParse function.

                if(int.TryParse(Grid_Width_EditText.Text,out a) && int.TryParse(Grid_Height_EditText.Text, out a) && int.TryParse(Number_Of_Mines_EditText.Text, out a)) {

                    Intent Intent = new Intent(this, typeof(MainActivity));

                    Intent.PutExtra("Grid_Width", int.Parse(Grid_Width_EditText.Text));
                    Intent.PutExtra("Grid_Height", int.Parse(Grid_Height_EditText.Text));
                    Intent.PutExtra("Number_Of_Mines", int.Parse(Number_Of_Mines_EditText.Text));

                    StartActivity(Intent);

                } else
                    Toast.MakeText(this, "Make Sure You Have Filled Every Thing Currently", ToastLength.Long).Show();
            }
        }

    }
}