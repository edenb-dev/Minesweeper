
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Minesweeper
{
    [Activity(Label = "Game_Instructions_Activity")]
    public class Game_Instructions_Activity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game_instructions);

            TextView Battery_Level_TextView = FindViewById<TextView>(Resource.Id.Battery_Level_TextView);

            // BroadcastReceiver.

            IntentFilter IntentFilter = new IntentFilter(Intent.ActionBatteryChanged);

            MyBroadcastReceiver BroadcastReceiver = new MyBroadcastReceiver(Battery_Level_TextView);

            RegisterReceiver(BroadcastReceiver, IntentFilter);
        }

        internal class MyBroadcastReceiver : BroadcastReceiver
        {

            TextView Battery_Level_TextView;

            public MyBroadcastReceiver(TextView TextView) {

                Battery_Level_TextView = TextView;
            }

            public override void OnReceive(Context context, Intent intent)
            {

                int Level = intent.GetIntExtra(BatteryManager.ExtraLevel,-1);
                int Scale = intent.GetIntExtra(BatteryManager.ExtraScale, -1);

                Battery_Level_TextView.Text = (Level / (float)Scale) * 100 + "%";
            }
        }

    }
}