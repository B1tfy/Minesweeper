using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            for(int i = 0; i < rows; i++)
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

            for (int i = 0;i < mineCount; i++) 
            {
                int randomRow = random.Next(0, rows);
                int randomCol = random.Next(0, cols);

                while (cells[randomRow,randomCol].IsMine) 
                {
                    randomRow = random.Next(0,rows);
                    randomCol = random.Next(0,cols);
                }

                cells[randomRow, randomCol].IsMine = true;
            }
        }
        private void CalculateNeighborMines()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j=0; j < cols; j++)
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
        public bool IsGameOver {  get; private set; }

        public void ReveallCell(int row, int col)
        {
            if (!IsGameOver && row >= 0 && row < rows && col >= 0 && col < cols && !cells[row, col].IsRevealed)
            {
                cells[row, col].Reveal();

                if (cells[row, col].IsMine)
                {
                    IsGameOver = true;
                }
                else
                {
                    if (cells[row,col].NeighborMineCount == 0)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1;y <= 1; y++)
                            {
                                ReveallCell(row + x, col + y);
                            }
                        }
                    }
                    if (CheckForWin())
                    {
                        IsGameOver = true;
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
                    if (!cells[i,j].IsMine && !cells[i, j].IsRevealed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        


    }
}
