using System;
using System.Collections.Generic;
using System.Text;

namespace EcologicalSystemConsole
{
    public class GridSpace
    {
        // the size of the ocean n x n
        public static int Size { get; set; }

        //2d array of type cell
        public Cell[,] theGrid { get; set; }

        public GridSpace(int s)
        {
            //initial size of the board is defined by s.
            Size = s;
            //create a new 2D array of type cell.
            theGrid = new Cell[Size, Size];

            //fill the 2D array with new Cells, each with unique x and y coordinates.
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    theGrid[i, j] = new Cell(i, j);
                }
            }
        }
    }
}
