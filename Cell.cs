using System;
using System.Collections.Generic;
using System.Text;

namespace EcologicalSystemConsole
{
    public class Cell
    {
        public Cell (int y, int x)
        {
            RowNumber = y;
            ColumnNumber = x;
        }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public bool CurrentlyOccupied { get; set; }
    }
}
