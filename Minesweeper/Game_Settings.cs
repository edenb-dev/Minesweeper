

namespace Minesweeper
{
    class Game_Settings
    {
        public int Grid_Width { get; set; }
        public int Grid_Heigth { get; set; }
        public int Number_Of_Mines { get; set; }

        public Game_Settings(int Grid_Width, int Grid_Heigth, int Number_Of_Mines) {

            this.Grid_Width = Grid_Width;
            this.Grid_Heigth = Grid_Heigth;
            this.Number_Of_Mines = Number_Of_Mines;
        }

    }
}