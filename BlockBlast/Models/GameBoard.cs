
using Microsoft.Maui.Graphics;

namespace BlockBlast.Models
{
    public class GameBoard
    {
        public const int BoardSize = 8;

        public int[,] Grid { get; private set; }
        public Color?[,] CellColors { get; private set; }

        public int Score { get; private set; }
        public int Combo { get; private set; }
        public int LastBonus { get; private set; }

        // Viimase käiguga eemaldatud ridade ja veergude arv
        public int LastClearedLines { get; private set; }

        public GameBoard()
        {
            Grid = new int[BoardSize, BoardSize];
            CellColors = new Color?[BoardSize, BoardSize];
        }

        // Kontrollib, kas klots mahub mängulauale
        public bool CanPlaceBlock(Block block, int row, int col)
        {
            if (block == null)
                return false;

            for (int r = 0; r < block.Height; r++)
            {
                for (int c = 0; c < block.Width; c++)
                {
                    if (block.Shape[r, c] == 0)
                        continue;

                    int newRow = row + r;
                    int newCol = col + c;

                    if (newRow < 0 || newRow >= BoardSize ||
                        newCol < 0 || newCol >= BoardSize)
                    {
                        return false;
                    }

                    if (Grid[newRow, newCol] != 0)
                        return false;
                }
            }

            return true;
        }

        // Paigutab klotsi ja arvutab punktid
        public bool PlaceBlock(Block block, int row, int col)
        {
            if (!CanPlaceBlock(block, row, col))
                return false;

            int placedCells = 0;

            for (int r = 0; r < block.Height; r++)
            {
                for (int c = 0; c < block.Width; c++)
                {
                    if (block.Shape[r, c] == 0)
                        continue;

                    int newRow = row + r;
                    int newCol = col + c;

                    Grid[newRow, newCol] = 1;
                    CellColors[newRow, newCol] = block.Color;

                    placedCells++;
                }
            }

            // Iga ruut annab 10 punkti
            Score += placedCells * 10;

            // Kontrollib täidetud ridu ja veerge
            LastClearedLines = ClearCompletedLines();

            LastBonus = 0;

            if (LastClearedLines > 0)
            {
                Combo++;

                // Iga eemaldatud rida või veerg annab 100 punkti
                int linePoints = LastClearedLines * 100;

                // Kombo ja mitme rea boonus
                LastBonus = Combo * 50 +
                            (LastClearedLines - 1) * 100;

                Score += linePoints + LastBonus;
            }
            else
            {
                Combo = 0;
            }

            return true;
        }

        // Eemaldab täidetud read ja veerud
        private int ClearCompletedLines()
        {
            bool[] fullRows = new bool[BoardSize];
            bool[] fullCols = new bool[BoardSize];

            for (int i = 0; i < BoardSize; i++)
            {
                fullRows[i] = true;
                fullCols[i] = true;

                for (int j = 0; j < BoardSize; j++)
                {
                    if (Grid[i, j] == 0)
                        fullRows[i] = false;

                    if (Grid[j, i] == 0)
                        fullCols[i] = false;
                }
            }

            int clearedLines = 0;

            for (int i = 0; i < BoardSize; i++)
            {
                if (fullRows[i])
                    clearedLines++;

                if (fullCols[i])
                    clearedLines++;
            }

            // Eemaldab ruudud ja nende värvid
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    if (fullRows[r] || fullCols[c])
                    {
                        Grid[r, c] = 0;
                        CellColors[r, c] = null;
                    }
                }
            }

            return clearedLines;
        }

        // Kontrollib, kas klots mahub kuhugi
        public bool HasAvailableMove(Block block)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (CanPlaceBlock(block, row, col))
                        return true;
                }
            }

            return false;
        }

        // Alustab uut mängu
        public void Reset()
        {
            Grid = new int[BoardSize, BoardSize];
            CellColors = new Color?[BoardSize, BoardSize];

            Score = 0;
            Combo = 0;
            LastBonus = 0;
            LastClearedLines = 0;
        }
    }
}
