using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Minesweeper
{
    public class Board
    {
        public Cell[,] cells;
        private int rows;
        private int cols;
        private int mineCount;

        public int Rows => rows;
        public int Cols => cols;

        public Board(int rows, int cols, int mineCount)
        {
            this.rows = rows;
            this.cols = cols;
            this.mineCount = mineCount;

            InitializeBoard();
            PlaceMines();
            CalculateNeighborMines();

        }
        private void InitializeBoard()
        {
            cells = new Cell[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    cells[i, j] = new Cell(false);
                }
            }


        }
        private void PlaceMines()
        {
            Random random = new Random();

            for (int i = 0; i < mineCount; i++)
            {
                int randomRow = random.Next(0, rows);
                int randomCol = random.Next(0, cols);

                while (cells[randomRow, randomCol].IsMine)
                {
                    randomRow = random.Next(0, rows);
                    randomCol = random.Next(0, cols);
                }

                cells[randomRow, randomCol].IsMine = true;
            }
        }
        private void CalculateNeighborMines()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!cells[i, j].IsMine)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                int neighborRow = i + x;
                                int neighborCol = j + y;

                                if (neighborRow >= 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < cols)
                                {
                                    if (cells[neighborRow, neighborCol].IsMine)
                                    {
                                        cells[i, j].IncrementMine();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool IsGameOver { get; set; }

        public void ReveallCell(int row, int col)
        {
            if (row >= 0 && row < rows && col >= 0 && col < cols && !cells[row, col].IsRevealed)
            {
                cells[row, col].Reveal(this, row, col);

                if (cells[row, col].IsMine)
                {
                    IsGameOver = true;
                }
                else
                {
                    if (cells[row, col].NeighborMineCount == 0)
                    {
                        cells[row, col].Reveal(this, row, col);
                    }
                }
            }
        }

        public bool CheckForWin()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!cells[i, j].IsMine && !cells[i, j].IsRevealed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public Cell GetCell(int row, int col)
        {
            return cells[row, col];
        }
        public void ToggleMarkedCell(int row, int col)
        {
            if (!IsGameOver && row >= 0 && row < rows && col >= 0 && col < cols)
            {
                cells[row, col].ToggleMark();
            }
        }
        public void ToggleUnknownCell(int row, int col)
        {
            if (!IsGameOver && row >= 0 && row < rows && col >= 0 && col < cols)
            {
                cells[row, col].ToggleUnknown();
            }
        }

        public int CountMarkedCellsAround(int centerRow, int centerCol)
        {
            int count = 0;

            for (int i = Math.Max(0, centerRow - 1); i <= Math.Min(Rows - 1, centerRow + 1); i++)
            {
                for (int j = Math.Max(0, centerCol - 1); j <= Math.Min(Cols - 1, centerCol + 1); j++)
                {
                    Cell cell = GetCell(i, j);

                    if (cell.IsMarked)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

    }    
}
