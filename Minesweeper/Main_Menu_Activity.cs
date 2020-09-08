using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Minesweeper
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class Main_Menu_Activity : Activity, Android.Views.View.IOnClickListener {


        protected override void OnCreate(Bundle savedInstanceState) {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main_menu);


            Button Play_Button = FindViewById<Button>(Resource.Id.Play_Button);
            Button Top10_Button = FindViewById<Button>(Resource.Id.Top10_Button);
            Button Instructions_Button = FindViewById<Button>(Resource.Id.Instructions_Button);


            Play_Button.SetOnClickListener(this);
            Top10_Button.SetOnClickListener(this);
            Instructions_Button.SetOnClickListener(this);
        }

        public void OnClick(View View) {

            Intent Intent = null;

            if (View.Id == Resource.Id.Play_Button) // Changing To Play Menu Activity.
                Intent = new Intent(this, typeof(Play_Menu));
                
            else if (View.Id == Resource.Id.Top10_Button) // Changing To Top 10 Activity.
                Intent = new Intent(this, typeof(Top10_Menu_Activity));

            else if (View.Id == Resource.Id.Instructions_Button) // Changing To Instruction Activity.
                Intent = new Intent(this, typeof(Game_Instructions_Activity));
            
            StartActivity(Intent);
        }

        public override void OnBackPressed() {

            // Removing The Back Click.
            // base.OnBackPressed();
        }

    }
}