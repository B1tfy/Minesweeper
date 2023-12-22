using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Cell
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsMarked { get; private set; }  
        public bool IsUnkown { get; set; }
        public int NeighborMineCount { get; set; }

        public Cell(bool isMine) 
        {
            IsMine = isMine;
            IsRevealed = false;
            NeighborMineCount = 0;
            IsMarked = false;
            IsUnkown = false;

        }
        public void IncrementMine()
        {
            NeighborMineCount++;
        }
        public void Reveal(Board board, int row, int col)
        {
            if (!IsMarked && !IsRevealed && !IsUnkown)
            {
                IsRevealed = true;

                if (NeighborMineCount == 0)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int neighborRow = row + i;
                            int neighborCol = col + j;
                            board.ReveallCell(neighborRow, neighborCol);

                        }
                    }
                }
            }
        }


        public void ToggleMark()
        {
            if (!IsRevealed)
            {
                IsMarked = !IsMarked;
                
            }
            

        }
        public void ToggleUnknown()
        {
            if (!IsRevealed)
            {
                IsUnkown = !IsUnkown;
            }
        }
        public string GetDisplayValue()
        {
            if (IsRevealed)
            {
                if (IsMine)
                {
                    return "X";
                }
                else
                {
                    if (NeighborMineCount == 0)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return NeighborMineCount.ToString();
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }

}
