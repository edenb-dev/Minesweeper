using System;

using Android.Views;
using Android.Widget;

namespace Minesweeper {

    class Game_Manger : Position {

        RelativeLayout Grid_Layout; // Holds The Grid Layout.
        ImageView[,] Grid_Views; // Holds The ImageViews Of The Grid Layout.
        int[,] Grid; // Holds The Data Types Of All The Locations In The Grid.

        int Number_Of_Mines; // Holds The Number Of Mines Requested When The Grid Was Created.
        int Counter_Openned_Blocks = 0; // Holds the Number Of Blocks That The User Have Openned.

        int Block_Size;

        bool First_Press = true;


        // Grid Codes.
        // They Are Used To Identify The Type Of Data In A Given Location.

        int Grid_Code_Pressed_Mine = 5;         // This Location Contains A Mine, And The User Have Pressed On It.
        int Grid_Code_Flagged_Mine_Spot = 4;    // This Location Contains A Mine, And It Is Flagged. 
        int Grid_Code_Flagged_Spot = 3;         // This Location Is Flagged. 
        int Grid_Code_Mine = 2;                 // This Location Contains A Mine. 
        int Grid_Code_Checked_Location = 1;     // This Location Has Been Check, And Does Not Contain A Mine.
        int Grid_Code_Empty_Spot = 0;           // This Location Is Empty, And Haven't Been Checked Before.
        int Grid_Code_Buffer = -1;              // This Location Is A Part Of The Border.


        // Contractor.

        public Game_Manger(int Grid_Width, int Grid_Heigth, int Number_Of_Mines, int Block_Size, RelativeLayout Grid_Layout) {

            this.Grid_Layout = Grid_Layout; // Creating A Reference.

            // Setting The Size Of The Grid.
            Grid = new int[Grid_Heigth + 2, Grid_Width + 2]; // Allocating Memory For The Grid.
            Grid_Views = new ImageView[Grid_Heigth, Grid_Width]; // Allocating Memory For The Grid Views.

            // Setting The Size Of The Blocks.
            this.Block_Size = Block_Size;

            // Setting The Number Of Requested Mines In The Current Game.
            this.Number_Of_Mines = Number_Of_Mines;

            // Setting Up The Grid.
            Setup_Grid();

            // Creating The Grid On The Gird Layout.
            Create_Visual_Grid(Block_Size);
        }


        // Game Logic

        public int Check_Surrounding_Area(int Location_Row, int Location_Column) {

            /*
                This Function Checks The Surrounding Area Of A Given Location In The Grid.

                * If The Function Will Detect That The Given Location :

                    - Was Already Checked, The Function Will Not Do Anything.
                    - Is A Mine, It Will Return true ( Stoping The Game ), And Change The Image Of The Given Location To A "Pressed_Mine" Image.
                    - Is A Normal Block, It Will Change The Image Of It Based On The Surrounding Mines Number, And If The Surrounding Mines
                      Number Of Is 0, The Function Will Run Again On All The Surrounding Area.
             */
             
            // ---------------------------------------------------------------------------------------------------------------------

            // Detecting The Type Of The Given Grid Location. ( Mine / An Area The User Pressed Before / Buffer )

            // ---------------------------------------------------------------------------------------------------------------------

            bool Pressed_On_Mine = false; // Indicates If The Given Grid Location Is A Mine.
            bool CheckBefore_Or_Buffer = false; // Indicates If The Given Grid Location Was Check Before / If Is A Part Of The Buffer.

            // Making Sure We Are Not Running The Test On An Already Tested Spot / Out Of Bounds / A Flagged Spot.
            if (Grid[Location_Row, Location_Column] == Grid_Code_Checked_Location || Grid[Location_Row, Location_Column] == Grid_Code_Buffer || Grid[Location_Row, Location_Column] == Grid_Code_Flagged_Spot || Grid[Location_Row, Location_Column] == Grid_Code_Flagged_Mine_Spot) {

                CheckBefore_Or_Buffer = true; // No Need To Check This Area.

            } else if (Grid[Location_Row, Location_Column] == Grid_Code_Mine) { // Player Lost. ( Pressed On A Mine )

                Pressed_On_Mine = true;
            }

            // ---------------------------------------------------------------------------------------------------------------------

            // Detecting If It Was The First Time The User Played, And If So Adding The Mines. ( Prevents Losing On The First Press )

            // ---------------------------------------------------------------------------------------------------------------------

            if (First_Press) { // Checking If It Was The First Time The User Preesed.

                First_Press = false; // Updating The 'First_Press' Bool.

                Grid[Location_Row, Location_Column] = Grid_Code_Checked_Location; // Updating The Location The User Have Pressed On.

                Add_Mines(); // Adding The Mines.
            }


            // ---------------------------------------------------------------------------------------------------------------------

            // Checking The Surrounding Area, If Past Checks Went Successfully.

            // * If The Surrounding Area Does Not Contain Any Mines ( Counter == 0 ) The Fucntion Will Run Again On All The Surrounding Blocks.

            // ---------------------------------------------------------------------------------------------------------------------

            int Counter = 0;

            if (Pressed_On_Mine == false && CheckBefore_Or_Buffer == false) { // User Haven't Pressed On A Mine / An Open Block.

                // Checking The Surrounding Area For Mines, And Updating The Counter Accordingly.

                if (Grid[Location_Row - 1, Location_Column + 1] == Grid_Code_Mine || Grid[Location_Row - 1, Location_Column + 1] == Grid_Code_Flagged_Mine_Spot) Counter++;   // Top - Right
                if (Grid[Location_Row - 1, Location_Column] == Grid_Code_Mine || Grid[Location_Row - 1, Location_Column] == Grid_Code_Flagged_Mine_Spot) Counter++;       // Top
                if (Grid[Location_Row - 1, Location_Column - 1] == Grid_Code_Mine || Grid[Location_Row - 1, Location_Column - 1] == Grid_Code_Flagged_Mine_Spot) Counter++;   // Top - Left
                if (Grid[Location_Row, Location_Column + 1] == Grid_Code_Mine || Grid[Location_Row, Location_Column + 1] == Grid_Code_Flagged_Mine_Spot) Counter++;       // Middle - Right
                if (Grid[Location_Row, Location_Column - 1] == Grid_Code_Mine || Grid[Location_Row, Location_Column - 1] == Grid_Code_Flagged_Mine_Spot) Counter++;       // Middle - Left
                if (Grid[Location_Row + 1, Location_Column + 1] == Grid_Code_Mine || Grid[Location_Row + 1, Location_Column + 1] == Grid_Code_Flagged_Mine_Spot) Counter++;   // Bottom - Right
                if (Grid[Location_Row + 1, Location_Column] == Grid_Code_Mine || Grid[Location_Row + 1, Location_Column] == Grid_Code_Flagged_Mine_Spot) Counter++;       // Bottom
                if (Grid[Location_Row + 1, Location_Column - 1] == Grid_Code_Mine || Grid[Location_Row + 1, Location_Column - 1] == Grid_Code_Flagged_Mine_Spot) Counter++;   // Bottom - Left


                Grid[Location_Row, Location_Column] = Grid_Code_Checked_Location; // Updating The Grid Array.


                if (Counter == 0) { // If No Mines Were Found. ( Based On The Rules Of The Game )

                    // Running The Function On All The Surrounding Area.

                    Check_Surrounding_Area(Location_Row - 1, Location_Column + 1);  // Top - Right
                    Check_Surrounding_Area(Location_Row - 1, Location_Column);      // Top
                    Check_Surrounding_Area(Location_Row - 1, Location_Column - 1);  // Top - Left 
                    Check_Surrounding_Area(Location_Row, Location_Column + 1);      // Middle - Right
                    Check_Surrounding_Area(Location_Row, Location_Column - 1);      // Middle - Left
                    Check_Surrounding_Area(Location_Row + 1, Location_Column + 1);  // Bottom - Right
                    Check_Surrounding_Area(Location_Row + 1, Location_Column);      // Bottom
                    Check_Surrounding_Area(Location_Row + 1, Location_Column - 1);  // Bottom - Left
                }

                // For Winning Mechanics \\

                Counter_Openned_Blocks++; // Incriminating The Openned Blocks Counter.

            }

            // ---------------------------------------------------------------------------------------------------------------------

            // Updating The Image Of The ImageView.

            // ---------------------------------------------------------------------------------------------------------------------

            if (Pressed_On_Mine == true) { // If The User Pressed On A Mine. ( Changing The Current Image To "Pressed_Mine" Image )

                Grid[Location_Row, Location_Column] = Grid_Code_Pressed_Mine;
                Grid_Views[Location_Row - 1, Location_Column - 1].SetImageResource(Resource.Drawable.pressed_mine);

                // Need To Create A Function That Shows All The Mines.
                return 0;

            } else if (CheckBefore_Or_Buffer == false) { // If We Haven't Updated This Block Image Before.

                // Retrieving The Image ID From The Drawable Folder Using A String. ( Based On The Value In Counter The Image Will Change )
                int Image_ID = Grid_Layout.Context.Resources.GetIdentifier("block_" + Counter.ToString(), "drawable", Grid_Layout.Context.PackageName);
                Grid_Views[Location_Row - 1, Location_Column - 1].SetImageResource(Image_ID); // Changing The Image Base On The Counter.
            }

            // ---------------------------------------------------------------------------------------------------------------------

            // Checking If The Player Have Won.
            if (Grid_Views.GetLength(0) * Grid_Views.GetLength(1) - Number_Of_Mines == Counter_Openned_Blocks) 
                return 1; // Player Won.

            return 2; // User Didn't Lose.
        }


        // Run Once Game Functions.

        void Setup_Grid() {

            /*
                This Function Sets-Up The Grid Elements, In 2 Parts.
                
                Part 1 :
                    - Changing The Value Of The Elements Within The "Border" To 'Grid_Code_Empty_Spot'. 

                Part 2 :
                    - Changing The Value Of The Elements At The Outskirts Of The Grid To 'Grid_Code_Buffer'. 
            */


            // Part 1 :
            // Changing The Values Withing The "Border".

            for (int Row_Index = 0; Row_Index < Grid.GetLength(0) - 2; Row_Index++) // Going Thru Each Row Of Elements.
                for (int Column_Index = 0; Column_Index < Grid.GetLength(1) - 2; Column_Index++) // Going Thru Each Column Of Elements.
                    Grid[Row_Index + 1, Column_Index + 1] = Grid_Code_Empty_Spot; // The "+ 1" Is Used To Update Only The Elements Withing The "Border".


            // Part 2 :
            // Adding The "Border" To Grid.

            Grid_Add_Border();
        }

        void Grid_Add_Border() {

            /*
                This Function Changes The Values Of The Elements At The Border Locations.
            */

            // Setting The Top & Bottom Border Value.
            for (int Column_Index = 0; Column_Index < Grid.GetLength(1); Column_Index++)
            {

                Grid[0, Column_Index] = Grid_Code_Buffer; // Top.
                Grid[Grid.GetLength(0) - 1, Column_Index] = Grid_Code_Buffer; // Bottom.
            }

            // Setting The Right & Left Border Value.
            for (int Row_Index = 0; Row_Index < Grid.GetLength(0) ; Row_Index++)
            {

                Grid[Row_Index, 0] = Grid_Code_Buffer; // Left.
                Grid[Row_Index, Grid.GetLength(1) - 1] = Grid_Code_Buffer; // Right.
            }
        }

        void Add_Mines() {

            /*
		        This Function Adds The Mines ( In Random Places ) To The Grid Array Based On The Specified Amount.
	        */

            Random Random_Generator = new Random(); // Used For Generating Random Mine Locaions. ( Random Numbers )

            // Adding The Mines One By One.
            for (int Mine_Counter = 0; Mine_Counter < Number_Of_Mines; Mine_Counter++) {

                // Genrating Random Spot Number.
                int Mine_Locaion = Random_Generator.Next(0, (Grid_Views.GetLength(1) * Grid_Views.GetLength(0)) - 1);

                // Checking If A Mine Doesn't Exists, In The Randomly Generated Mine Location.
                if (Grid[(Mine_Locaion / Grid_Views.GetLength(1)) + 1, (Mine_Locaion % Grid_Views.GetLength(1)) + 1] == Grid_Code_Empty_Spot) {

                    // Adding The Mine To The Grid.
                    Grid[(Mine_Locaion / Grid_Views.GetLength(1)) + 1, (Mine_Locaion % Grid_Views.GetLength(1)) + 1] = Grid_Code_Mine;

                } else { // If The Generated Mine Location Had Already Been Taken.

                    // Retring To Create A Mine.
                    Mine_Counter--;
                }
            }
        }


        // GUI

        public bool Change_Flag_State(int Location_Row, int Location_Column) {

            /*
                This Function Gets A Location In The Gird, Checkes It, And Updates It's Flag. ( Flagged Location On / Off )
            */

            // Creates The Flag. ( If The Spot Is Empty / Is A Mine )
            if (Grid[Location_Row, Location_Column] == Grid_Code_Empty_Spot || Grid[Location_Row, Location_Column] == Grid_Code_Mine)
            {

                // Updating The Grid Array.
                if (Grid[Location_Row, Location_Column] == Grid_Code_Empty_Spot)
                    Grid[Location_Row, Location_Column] = Grid_Code_Flagged_Spot;
                else
                    Grid[Location_Row, Location_Column] = Grid_Code_Flagged_Mine_Spot;

                // Changing The Image To A Flag Image.
                Grid_Views[Location_Row - 1, Location_Column - 1].SetImageResource(Resource.Drawable.flagged_block);

                return true;

            } else if (Grid[Location_Row, Location_Column] == Grid_Code_Flagged_Spot || Grid[Location_Row, Location_Column] == Grid_Code_Flagged_Mine_Spot) { // Changing The Flag Back To He's Original State.

                // Updating The Grid Array.
                if (Grid[Location_Row, Location_Column] == Grid_Code_Flagged_Spot)
                    Grid[Location_Row, Location_Column] = Grid_Code_Empty_Spot;
                else
                    Grid[Location_Row, Location_Column] = Grid_Code_Mine; 

                Grid_Views[Location_Row - 1, Location_Column - 1].SetImageResource(Resource.Drawable.unpressed_block); // Changing The Image To A Flag Image.

                return true;
            }

            return false;
        }

        public void Show_All_Mines() {

            /*
                This Function Revile The Locations Of All The Mines. ( Changing The Images Where The Mines Are Located )
            */

            for (int Row_Index = 0; Row_Index < Grid_Views.GetLength(0); Row_Index++) { // Going Thru Each Element.
                for (int Column_Index = 0; Column_Index < Grid_Views.GetLength(1); Column_Index++) {

                    if (Grid[Row_Index + 1, Column_Index + 1] == Grid_Code_Mine)
                        Grid_Views[Row_Index, Column_Index].SetImageResource(Resource.Drawable.mine);
                    else if (Grid[Row_Index + 1, Column_Index + 1] == Grid_Code_Flagged_Spot)
                        Grid_Views[Row_Index, Column_Index].SetImageResource(Resource.Drawable.flagged_mine);
                }
            }
        }

        void Create_Visual_Grid(int Block_Size) {

            // Removing All The Views In The Grid Layout.
            Grid_Layout.RemoveAllViews();

            for (int Row_Index = 0; Row_Index < Grid_Views.GetLength(0); Row_Index++) { // Going Thru Each Element.
                for (int Column_Index = 0; Column_Index < Grid_Views.GetLength(1); Column_Index++) {

                    // Adding The ImageView To 'Grid_Views' Array.
                    Grid_Views[Row_Index, Column_Index] = Create_ImageView(-Column_Index * Block_Size, Row_Index * Block_Size, Block_Size, Block_Size, Resource.Drawable.unpressed_block);

                    // Adding The ImageView To The Grid Layout.
                    Grid_Layout.AddView(Grid_Views[Row_Index, Column_Index]);
                }
            }
        }

        ImageView Create_ImageView(float X, float Y, int Width, int Height, int Image_Resource) {

            ImageView ImageView = new ImageView(Grid_Layout.Context); // Creating A New ImageView.

            // Setting The Location Of The ImageView.
            ImageView.SetX(X); // Setting The X Coordinates.
            ImageView.SetY(Y); // Setting The Y Coordinates.

            if (Height == 0)
                Height = WindowManagerLayoutParams.WrapContent;
            if (Width == 0)
                Width = WindowManagerLayoutParams.WrapContent;

            ImageView.LayoutParameters = new RelativeLayout.LayoutParams(Width, Height); // Setting The Width And The Height.
            ImageView.SetImageResource(Image_Resource); // Setting The Image Of The ImageView.

            // Resizing The Image To The Size Of The ImageView.

            ImageView.SetAdjustViewBounds(true);
            ImageView.SetScaleType(ImageView.ScaleType.FitXy);

            return ImageView;
        }


        // Restart Game.

        public void Restart_Game() {

            /*
                This Function Restarts The Game By Reseting The Grid Array And Re-Adding ImageViews To The Grid Layout.
            */

            // Reseting The Counter.
            Counter_Openned_Blocks = 0;

            First_Press = true;

            // Setting Up The Grid.
            Setup_Grid();

            // Creating The Grid On The Gird Layout.
            Create_Visual_Grid(Block_Size);
        }
        

        // Getters

        public ImageView[,] Get_Grid_Views() {

            return Grid_Views; // Returning The ImageView Array.
        }

    }
}