using Android.App;
using Android.OS;
using Android.Support.V7.App;

using Android.Widget;
using Android.Views;

using System;
using System.Timers;
using Android.Content;

namespace Minesweeper
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity, Android.Views.View.IOnClickListener {

        //         Basic         \\

        int Screen_Width;
        int Screen_Height;


        //          Game         \\

        Game_Manger Minesweeper; // Hold's The Game Object.

        int Block_Size; // Hold's The Size Of The Block That Are Going To Be Created.

        bool Player_Lost = false;
        bool Player_Won = false;

        int Grid_Width;
        int Grid_Height;
        int Number_Of_Mines;

        //  Long Press Detection \\

        Timer Timer_Detect_LongPress;  // Timer.
        int Pressed_Block_Location;
        MotionEvent Press_Location;
        bool Flag_Created;


        //  Hand Mode Variables  \\

        bool Hand_Mode = false;

        float Previous_X_Location = 0;
        float Previous_Y_Location = 0;

        //     In Game Timer     \\

        Timer In_Game_Clock_Timer;  // Timer.
        TextView In_Game_Clock_Counter;

        bool Timer_Running = false;

        //                       \\

        protected override void OnCreate(Bundle savedInstanceState) {

            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            SupportActionBar.Hide(); // Hiding The ActionBar. ( The Banner At The Top Of The App That Displays The App Name )

            // Getting The Screen Dimensions.

            Screen_Width = Resources.DisplayMetrics.WidthPixels;   // Getting The Screen Width.
            Screen_Height = Resources.DisplayMetrics.HeightPixels; // Getting The Screen Hight.

            // Getting The Request.

            Grid_Width = Intent.GetIntExtra("Grid_Width", 5);
            Grid_Height = Intent.GetIntExtra("Grid_Height",10);
            Number_Of_Mines = Intent.GetIntExtra("Number_Of_Mines", 10);

            //          Game          \\
            
            Block_Size = (int)(Screen_Width * 0.10); // Setting The Size Of The Blocks

            Minesweeper = new Game_Manger(Grid_Width, Grid_Height, Number_Of_Mines, Block_Size, FindViewById<RelativeLayout>(Resource.Id.Grid)); // Running The Game.

            Fix_Grid_Size_And_Location(); // Fixing The Size And Location Of The Grid.

            //  Long Press Detection  \\

            Timer_Detect_LongPress = new Timer(300);
            Timer_Detect_LongPress.Elapsed += Detect_LongPress;

            Create_Banner();

            //      In Game Timer     \\

            In_Game_Clock_Timer = new Timer(1000);
            In_Game_Clock_Timer.Elapsed += Update_Clock;
        }

        public void Update_Clock(object sender, ElapsedEventArgs Event) {

            // Incriminating The Timer.
            RunOnUiThread(() => { In_Game_Clock_Counter.Text = (int.Parse(In_Game_Clock_Counter.Text) + 1).ToString(); });
        }

        // Detecting The User Presses.

        private void Detect_LongPress(object sender, ElapsedEventArgs Event) {

            // Checking If The User 'Longged Pressed' A ImageView. ( To Change It To A Flag )

            ImageView[,] Grid_Views = Minesweeper.Get_Grid_Views();

            RelativeLayout Grid_Layout = FindViewById<RelativeLayout>(Resource.Id.Grid);

            // Checking If This View Still Being Pressed.
            if (Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetY() + (int)(Block_Size / 2) < Press_Location.GetY() - Grid_Layout.GetY() && Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetY() + (int)(Block_Size / 2) + Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].Height > Press_Location.GetY() - Grid_Layout.GetY() && Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetX() < (Press_Location.GetX() + (Grid_Layout.Width - Screen_Width)) + (Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width)) && Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetX() + Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].Width > (Press_Location.GetX() + (Grid_Layout.Width - Screen_Width)) + (Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width))) {

                Flag_Created = true;

                // Creating A Flag On That Location.
                if(Minesweeper.Change_Flag_State((Pressed_Block_Location / Grid_Views.GetLength(1)) + 1, (Pressed_Block_Location % Grid_Views.GetLength(1)) + 1)) {

                    // Vibrating The Phone When Creating A Flag.
                    Vibrator Vibrator = (Vibrator)Application.Context.GetSystemService(VibratorService);
                    Vibrator.Vibrate(100);
                }
            }

            Timer_Detect_LongPress.Stop();

            throw new NotImplementedException();
        }

        // User Pressed The Screen Events.

        public override bool OnTouchEvent(MotionEvent Event) {

            Press_Location = Event; // Updating The Location Of The Press.

            if (!Hand_Mode) {

                if (!Timer_Running) { // Starting The Game Clock.

                    In_Game_Clock_Timer.Start();
                    Timer_Running = true;
                }

                if (!Player_Lost && !Player_Won) { // If The Player Didn't Lose/Win.

                    if (Press_Location.Action == MotionEventActions.Down) { // Detect When The User First Pressed The Screen.

                        ImageView[,] Grid_Views = Minesweeper.Get_Grid_Views();

                        RelativeLayout Grid_Layout = FindViewById<RelativeLayout>(Resource.Id.Grid);

                        // Going Thru Each Element In The Grid Views Array.
                        for (int Row_Index = 0; Row_Index < Grid_Views.GetLength(0); Row_Index++) {

                            for (int Column_Index = 0; Column_Index < Grid_Views.GetLength(1); Column_Index++) {

                                // Checking If The User Pressed Inside The View.
                                if (Grid_Views[Row_Index, Column_Index].GetY() + (int)(Block_Size / 2) < Press_Location.GetY() - Grid_Layout.GetY() && Grid_Views[Row_Index, Column_Index].GetY() + (int)(Block_Size / 2) + Grid_Views[Row_Index, Column_Index].Height > Press_Location.GetY() - Grid_Layout.GetY() && Grid_Views[Row_Index, Column_Index].GetX() < (Press_Location.GetX() + (Grid_Layout.Width - Screen_Width)) + (Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width)) && Grid_Views[Row_Index, Column_Index].GetX() + Grid_Views[Row_Index, Column_Index].Width > (Press_Location.GetX() + (Grid_Layout.Width - Screen_Width)) + (Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width))) {

                                    Flag_Created = false;

                                    // Saving The Location Of The Block That Have Been Pressed.
                                    Pressed_Block_Location = (Row_Index * Grid_Views.GetLength(1)) + Column_Index;
                                }
                            }
                        }

                        // Detecting Long Press. ( Used For Flag )

                        Timer_Detect_LongPress.Start(); // Starting The Timer.

                    } else if (Press_Location.Action == MotionEventActions.Up) { // Detect When The User Released He's Press.

                        ImageView[,] Grid_Views = Minesweeper.Get_Grid_Views();

                        RelativeLayout Grid_Layout = FindViewById<RelativeLayout>(Resource.Id.Grid);

                        // Checking If This View Still Being Pressed.
                        if (Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetY() + (int)(Block_Size / 2) < Press_Location.GetY() - Grid_Layout.GetY() && Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetY() + (int)(Block_Size / 2) + Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].Height > Press_Location.GetY() - Grid_Layout.GetY() && Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetX() < (Press_Location.GetX() + (Grid_Layout.Width - Screen_Width)) + (Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width)) && Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].GetX() + Grid_Views[Pressed_Block_Location / Grid_Views.GetLength(1), Pressed_Block_Location % Grid_Views.GetLength(1)].Width > (Press_Location.GetX() + (Grid_Layout.Width - Screen_Width)) + (Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width))) {

                            // Making Sure Not To Take 2 Actions.
                            if (!Flag_Created) {

                                // Game State = Player Won/Lost The Game.
                                int Game_State = Minesweeper.Check_Surrounding_Area((Pressed_Block_Location / Grid_Views.GetLength(1)) + 1, (Pressed_Block_Location % Grid_Views.GetLength(1)) + 1); // Running The Algorithm On The Pressed View.

                                if (Game_State == 0)
                                    Player_Pressed_On_Mine(); // Player Lost The Game. ( Pressed On A Mine )
                                else if (Game_State == 1)
                                    Player_Won_Minesweeper();
                            }
                        }

                        Timer_Detect_LongPress.Stop(); // Stopping The Timer.
                    }
                }
            } else { // Hand Mode Activated.

                /* Hand Mode Allows The User To Move The Grid. */

                if (Press_Location.Action == MotionEventActions.Down) { // Detect When The User First Pressed The Screen.
                    
                    // Saving Where The User First Start Pressing.

                    Previous_X_Location = Press_Location.GetX();
                    Previous_Y_Location = Press_Location.GetY();
                }


                // Moving The Grid Based On To The User Press.

                RelativeLayout Grid_Layout = FindViewById<RelativeLayout>(Resource.Id.Grid);

                // Updating The Location Of The Grid.

                RelativeLayout Grid = FindViewById<RelativeLayout>(Resource.Id.Grid);

                if (Screen_Width < Grid.LayoutParameters.Width) { // Moving On The X Access If The Grid Is Bigger Then The Screen.
                
                    if ((Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width)) - (Press_Location.GetX() - Previous_X_Location) > 0)
                        Grid_Layout.SetX(Grid_Layout.Right - Grid_Layout.Width);
                    else if ((Grid_Layout.Right - (Grid_Layout.GetX() + Grid_Layout.Width)) + Grid_Layout.Width - (Press_Location.GetX() - Previous_X_Location) < Screen_Width)
                        Grid_Layout.SetX((Grid_Layout.Right - Grid_Layout.Width) + (Grid_Layout.Width - Screen_Width));
                    else
                        Grid_Layout.SetX(Grid_Layout.GetX() + (Press_Location.GetX() - Previous_X_Location));
                }

                if (Screen_Height < Grid.LayoutParameters.Height) {// Moving On The Y Access If The Grid Is Bigger Then The Screen.

                    if (Grid_Layout.GetY() + (Press_Location.GetY() - Previous_Y_Location) > (int)(Screen_Height * 0.12))
                        Grid_Layout.SetY(0 + (int)(Screen_Height * 0.12));
                    else if (Grid_Layout.GetY() + Grid_Layout.Height + (Press_Location.GetY() - Previous_Y_Location) < Screen_Height - (int)(Screen_Height * 0.88 % Block_Size))
                        Grid_Layout.SetY(Screen_Height - (int)(Screen_Height * 0.88 % Block_Size) - Grid_Layout.Height);
                    else
                        Grid_Layout.SetY(Grid_Layout.GetY() + (Press_Location.GetY() - Previous_Y_Location));
                }

                // Updating The Location Of The Press.

                Previous_X_Location = Press_Location.GetX();
                Previous_Y_Location = Press_Location.GetY();
            }

            return base.OnTouchEvent(Press_Location);
        }

        void Create_Banner() {

            RelativeLayout Main_Layout = FindViewById<RelativeLayout>(Resource.Id.Main_Layout);

            // Adding The Borders.
            Main_Layout.AddView(Create_ImageView(0, 0,Screen_Width,(int)(Screen_Height*0.12), Resource.Drawable.unpressed_block)); // Top.
            Main_Layout.AddView(Create_ImageView(0,Screen_Height - (int)(Screen_Height*0.88 % Block_Size), Screen_Width, (int)(Screen_Height*0.88 % Block_Size), Resource.Drawable.unpressed_block)); // Bottom.

            // Adding The Buttons.
            Main_Layout.AddView(Create_Button(-(int)(Screen_Width * 0.5) + (int)(Screen_Height * 0.05), (int)(Screen_Height * 0.0225), (int)(Screen_Height * 0.075), (int)(Screen_Height * 0.075), Resource.Drawable.happy_smiley, 0));
            Main_Layout.AddView(Create_Button(-(int)(Screen_Width*0.88) + (int)(Screen_Height * 0.1), (int)(Screen_Height * 0.035), (int)(Screen_Height*0.05), (int)(Screen_Height * 0.05), Resource.Drawable.hand_mode_off, 1));

            Button Menu_Button = Create_Button(-(int)(Screen_Width * 0.9) + (int)(Screen_Height * 0.035), (int)(Screen_Height * 0.035), (int)(Screen_Height * 0.05), (int)(Screen_Height * 0.05), Resource.Drawable.menu_button, 2);

            Main_Layout.AddView(Menu_Button);

            RegisterForContextMenu(Menu_Button); // Controling The Menu.

            // Adding The Timer Of The Game.
            In_Game_Clock_Counter = Create_TextView(-(int)(Screen_Width * 0.12), (int)(Screen_Height * 0.035), 0, (int)(Screen_Height * 0.05), "0", (int)(Screen_Height * 0.01));
            Main_Layout.AddView(In_Game_Clock_Counter);
            Main_Layout.AddView(Create_TextView(-(int)(Screen_Width * 0.18), (int)(Screen_Height * 0.035), 0, (int)(Screen_Height * 0.05), "Timer :", (int)(Screen_Height * 0.01)));
        }

        void Fix_Grid_Size_And_Location() {


            // This Function Fixes The Size Of The Gird, And Center The Grid If Needed.

            RelativeLayout Grid = FindViewById<RelativeLayout>(Resource.Id.Grid);
            Grid.LayoutParameters.Width = Block_Size * Grid_Width;
            Grid.LayoutParameters.Height = Block_Size * Grid_Height;
            Grid.SetY((int)(Screen_Height * 0.12));


            if (Screen_Width > Grid.LayoutParameters.Width) { // Centering The Grid. ( X )

                Grid.SetX((int)(-Screen_Width*0.5) + Grid.LayoutParameters.Width / 2);
            }

            if (Screen_Height > Grid.LayoutParameters.Height) { // Centering The Grid. ( Y )

                Grid.SetY((int)(Screen_Height * 0.12) + (int)(Screen_Height * 0.44) - Grid.LayoutParameters.Height / 2);
            }
        }

        // Player Won / Lost The Game.

        void Player_Won_Minesweeper() {

            // Preventing The Player From Playing.
            Player_Won = true;

            // Updating The Image Of The Smiley Button.
            Button Smiley_Button = FindViewById<Button>(0);
            Smiley_Button.SetBackgroundResource(Resource.Drawable.cool_smiley);

            // Stoping The Game Timer.
            In_Game_Clock_Timer.Stop();
            Timer_Running = false;
        }

        void Player_Pressed_On_Mine() {

            /*
                This Function Changes Several Element, To Indicate That The Player Have Lost. 

                1. The Image Of The Smiley, To The 'sad_smiley' Image.
                2. Showing All The Mines.
                3. Showing All The Missed Place Flags.
            */

            // Preventing The User From Playing Till He Will Restart The Game.
            Player_Lost = true;

            // Updating The Image Of The Smiley Button.
            Button Smiley_Button = FindViewById<Button>(0);
            Smiley_Button.SetBackgroundResource(Resource.Drawable.sad_smiley);

            // Showing All The Mines And The Miss Places Flags.
            Minesweeper.Show_All_Mines();

            // Stoping The Game Timer.
            In_Game_Clock_Timer.Stop();
            Timer_Running = false;
        }

        ImageView Create_ImageView(float X, float Y, int Width, int Height, int Image_Resource) {

            ImageView ImageView = new ImageView(this); // Creating A New ImageView.

            // Setting The Location Of The ImageView.
            ImageView.SetX(X); // Setting The X Coordinates.
            ImageView.SetY(Y); // Setting The Y Coordinates.

            if (Height == 0)
                Height = WindowManagerLayoutParams.WrapContent;
            if (Width == 0)
                Width = WindowManagerLayoutParams.WrapContent;

            ImageView.LayoutParameters = new RelativeLayout.LayoutParams(Width, Height); // Setting The Width And The Height.
            ImageView.SetImageResource(Image_Resource); // Setting The Image Of The ImageView.
            ImageView.SetScaleType(ImageView.ScaleType.FitXy); // Resizing The Image To The Size Of The ImageView.

            return ImageView;
        }

        Button Create_Button(float X, float Y, int Width, int Height, int Image_Resource,int Tag) {

            Button Button = new Button(this); // Creating A New Button.

            // Setting The Location Of The Button.
            Button.SetX(X); // Setting The X Coordinates.
            Button.SetY(Y); // Setting The Y Coordinates.

            if (Height == 0)
                Height = WindowManagerLayoutParams.WrapContent;
            if (Width == 0)
                Width = WindowManagerLayoutParams.WrapContent;

            Button.LayoutParameters = new RelativeLayout.LayoutParams(Width, Height); // Setting The Width And The Height.
            Button.SetBackgroundResource(Image_Resource); // Setting The Image Of The Button.

            Button.Id = Tag; // Setting The Tag.

            Button.SetOnClickListener(this); // Adding A Click Listener.

            return Button;
        }

        TextView Create_TextView(float X, float Y, int Width, int Height, string Text, int Font_Size) {

            TextView TextView = new TextView(this); // Creating A New TextView.

            // Setting The Location Of The TextView.
            TextView.SetX(X); // Setting The X Coordinates.
            TextView.SetY(Y); // Setting The Y Coordinates.

            if (Height == 0)
                Height = WindowManagerLayoutParams.WrapContent;
            if (Width == 0)
                Width = WindowManagerLayoutParams.WrapContent;

            // Width And Height Settings.
            TextView.LayoutParameters = new RelativeLayout.LayoutParams(Width, Height); // Setting The Width And The Height.

            // Text Settings.
            TextView.Text = Text; // Setting The Text.
            TextView.SetTextColor(Android.Graphics.Color.White); // Setting The Text Color.
            TextView.SetTextSize(Android.Util.ComplexUnitType.Dip, Font_Size); // Setting The Font. ( By Dip )

            // Text Gravity Settings.
            TextView.Gravity = GravityFlags.Center; // Setting The Gravity. 

            return TextView;
        }

        public void OnClick(View View) {

            // The User Clicked On The Smiley Button.
            if (View.Id == 0 ) {

                // Updating The Image Of The Smiley.
                Button Hand_Mode_Button = View as Button;
                Hand_Mode_Button.SetBackgroundResource(Resource.Drawable.happy_smiley);

                // Restarting The Game.
                Minesweeper.Restart_Game();
                Player_Lost = false;

                // Stopping The Game Clock.
                In_Game_Clock_Timer.Stop();
                Timer_Running = false;

                // Checking If The Player Won The Game.
                if (Player_Won) {

                    Player_Won = false; // Reseting The Winning bool.

                    // Moving The User To The Winning Activity.
                    Intent Intent = new Intent(this, typeof(Winning_Activity));

                    // Adding The Level Difficulty To The Intent.
                    if (Grid_Width == 9 && Grid_Height == 9 && Number_Of_Mines == 15)
                        Intent.PutExtra("Level_Difficulty", "Easy Mode");
                    else if (Grid_Width == 16 && Grid_Height == 16 && Number_Of_Mines == 40)
                        Intent.PutExtra("Level_Difficulty", "Medium Mode");
                    else if (Grid_Width == 20 && Grid_Height == 25 && Number_Of_Mines == 80)
                        Intent.PutExtra("Level_Difficulty", "Hard Mode");
                    else
                        Intent.PutExtra("Level_Difficulty", "Costume Mode");

                    // Adding The Level Difficulty To The Intent.
                    Intent.PutExtra("Time", In_Game_Clock_Counter.Text);

                    // Changing To The Next Activity.
                    StartActivity(Intent);
                }

                In_Game_Clock_Counter.Text = "0"; // Reseting The Game Clock Counter.


            } else if (View.Id == 1) { // The User Clicked On The Hand-Mode Button

                RelativeLayout Grid = FindViewById<RelativeLayout>(Resource.Id.Grid);

                // Hand Mode Can Only Be Activated When The Grid Can Move On One Of The Accesses.
                if (Screen_Width < Grid.LayoutParameters.Width || Screen_Height < Grid.LayoutParameters.Height) {

                    Hand_Mode = !Hand_Mode;

                    Button Hand_Mode_Button = View as Button;

                    if (Hand_Mode)
                        Hand_Mode_Button.SetBackgroundResource(Resource.Drawable.hand_mode_on);
                    else
                        Hand_Mode_Button.SetBackgroundResource(Resource.Drawable.hand_mode_off);
                }
            }
        }


        // Menu.

        public override void OnCreateContextMenu(Android.Views.IContextMenu menu, Android.Views.View v, Android.Views.IContextMenuContextMenuInfo menuInfo)  {

            base.OnCreateContextMenu(menu, v, menuInfo);

            MenuInflater.Inflate(Resource.Menu.game_menu, menu);
        }

        public override bool OnContextItemSelected(Android.Views.IMenuItem item) {

            if (item.ItemId == Resource.Id.Play_Menu) {

                FinishAffinity(); // Removing The Activity History. ( All The Activitys That Were Open, No Longer Exist )

                // Moving The User To The Play Manu Activity.
                Intent[] Intent = new Intent[] { new Intent(this, typeof(Main_Menu_Activity)), new Intent(this, typeof(Play_Menu)) };

                // Changing To The Next Activity.
                StartActivities(Intent);

                return true;

            } else if (item.ItemId == Resource.Id.Top10_Menu) {

                FinishAffinity(); // Removing The Activity History. ( All The Activitys That Were Open, No Longer Exist )

                // Moving The User To The Top10 Manu Activity.
                Intent[] Intent = new Intent[] { new Intent(this, typeof(Main_Menu_Activity)) , new Intent(this, typeof(Top10_Menu_Activity)) };

                // Changing To The Next Activity.
                StartActivities(Intent);

                return true;

            } else if (item.ItemId == Resource.Id.Game_Instructions) {

                FinishAffinity(); // Removing The Activity History. ( All The Activitys That Were Open, No Longer Exist )

                // Moving The User To The Game Instructions Activity.
                Intent[] Intent = new Intent[] { new Intent(this, typeof(Main_Menu_Activity)), new Intent(this, typeof(Game_Instructions_Activity)) };

                // Changing To The Next Activity.
                StartActivities(Intent);

                return true;

            } else if (item.ItemId == Resource.Id.Dialog) {

                Dialog Dialog = new Dialog(this);

                Dialog.SetContentView(Resource.Layout.custom_dialog);
                Dialog.SetCancelable(true);

                Dialog.Show();

                return true;
            }

            return false;
        }

    }
}