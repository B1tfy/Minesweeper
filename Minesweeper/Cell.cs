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
        public int NeighborMineCount { get; set; }

        public Cell(bool isMine) 
        {
            IsMine = isMine;
            IsRevealed = false;
            NeighborMineCount = 0;
            IsMarked = false;

        }
        public void IncrementMine()
        {
            NeighborMineCount++;
        }
        public void Reveal()
        {
            if (!IsMarked)
            {
                IsRevealed = true;
            }

        }
        public void ToggleMark()
        {
            if (!IsRevealed)
            {
                IsMarked = !IsMarked;
                
            }
            

        }
        public string GetDisplayValue()
        {
            if (IsRevealed)
            {
                if (IsMine)
                {
                    return "*";
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
                return " ";
            }
        }
    }

}
