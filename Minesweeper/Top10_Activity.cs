
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;


namespace Minesweeper
{
    [Activity(Label = "Top10_Activity")]
    public class Top10_Activity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.top10);

            ListView Top10_List = FindViewById<ListView>(Resource.Id.Top10_ListView);
            ArrayAdapter<string> ArrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, Load_Data());
            Top10_List.SetAdapter(ArrayAdapter);
        }

        List<string> Load_Data() {

            Sqlite_Helper Sqlite_Helper = new Sqlite_Helper(Intent.GetStringExtra("DB_Path"));

            return Sqlite_Helper.Load_Users();
        }
    }
}