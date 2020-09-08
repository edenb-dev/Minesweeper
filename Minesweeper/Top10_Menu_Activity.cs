
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Minesweeper
{
    [Activity(Label = "Top10_Menu_Activity")]
    public class Top10_Menu_Activity : Activity, Android.Views.View.IOnClickListener {

        protected override void OnCreate(Bundle savedInstanceState) {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.top10_menu);


            Button Easy_Button = FindViewById<Button>(Resource.Id.Easy_Button);
            Button Medium_Button = FindViewById<Button>(Resource.Id.Medium_Button);
            Button Hard_Button = FindViewById<Button>(Resource.Id.Hard_Button);

            Easy_Button.SetOnClickListener(this);
            Medium_Button.SetOnClickListener(this);
            Hard_Button.SetOnClickListener(this);
        }

        public void OnClick(View View) {

            Intent Intent = new Intent(this, typeof(Top10_Activity));

            if (View.Id == Resource.Id.Easy_Button) {

                Intent.PutExtra("DB_Path", "Top10 Easy Mode");

            } else if (View.Id == Resource.Id.Medium_Button) {

                Intent.PutExtra("DB_Path", "Top10 Medium Mode");

            } else if (View.Id == Resource.Id.Hard_Button) {

                Intent.PutExtra("DB_Path", "Top10 Hard Mode");
            }

            StartActivity(Intent);
        }
    }
}

