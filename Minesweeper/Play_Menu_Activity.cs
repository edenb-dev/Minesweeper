
using Android.App;
using Android.Content;
using Android.OS;

using Android.Views;
using Android.Widget;

namespace Minesweeper
{
    [Activity(Label = "Play_Menu_Activity")]
    public class Play_Menu : Activity, Android.Views.View.IOnClickListener {
        

        protected override void OnCreate(Bundle savedInstanceState) {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.play_menu);


            Button Easy_Button = FindViewById<Button>(Resource.Id.Easy_Button);
            Button Medium_Button = FindViewById<Button>(Resource.Id.Medium_Button);
            Button Hard_Button = FindViewById<Button>(Resource.Id.Hard_Button);
            Button Custom_Button = FindViewById<Button>(Resource.Id.Custom_Button);


            Easy_Button.SetOnClickListener(this);
            Medium_Button.SetOnClickListener(this);
            Hard_Button.SetOnClickListener(this);
            Custom_Button.SetOnClickListener(this);
        }


        public void OnClick(View View) {

            Intent Intent = new Intent(this, typeof(MainActivity));

            if (View.Id == Resource.Id.Easy_Button) { // Adding The Data To Start An Easy Game.

                Intent.PutExtra("Grid_Width", 9);
                Intent.PutExtra("Grid_Height", 9);
                Intent.PutExtra("Number_Of_Mines", 15);

            } else if (View.Id == Resource.Id.Medium_Button) { // Adding The Data To Start An Medium Game.

                Intent.PutExtra("Grid_Width", 16);
                Intent.PutExtra("Grid_Height", 16);
                Intent.PutExtra("Number_Of_Mines", 35);

            } else if (View.Id == Resource.Id.Hard_Button) { // Adding The Data To Start An Hard Game.

                Intent.PutExtra("Grid_Width", 20);
                Intent.PutExtra("Grid_Height", 25);
                Intent.PutExtra("Number_Of_Mines", 80);

            } else if (View.Id == Resource.Id.Custom_Button) // Changing The Intent To Custom Game Settings Activity.
                Intent = new Intent(this, typeof(Custom_Game_Settings_Activity));

            StartActivity(Intent);
        }

    }
}