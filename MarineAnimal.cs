using System;
using System.Collections.Generic;
using System.Text;

namespace EcologicalSystemConsole
{
    public class MarineAnimal
    {
        public string Type { get; set; } //type of marine animal such as plankton, sardine, shark or crab
        public int RowNumber { get; set; } //y coordinate in Grid
        public int ColumnNumber { get; set; }  //x coordinate in Grid
        protected int FoodClockLimit { get; set; } //amount of generations animal can last without eating. When reached animal dies     
        protected int FoodClock { get; set; } //keeps track of moves that keep incrementing until limit is reached
        protected int ReproClockLimit { get; set; } //amount of generations that animal must wait before reproducing.
        protected int ReproClock { get; set; } //keeps track of moves that keep increments when animal eats or moves
        protected int PrevRN { get; set; } //row number before moving
        protected int PrevCN { get; set; } //column number before moving

        public MarineAnimal(int rn, int cn) //constructor
        {            
            //animals must be assigned an initial location
            RowNumber = rn;
            ColumnNumber = cn;
            //all animals start with a reproduction and food clock of 0 when they spawn or are born
            FoodClock = 0;
            ReproClock = 0; 
        }

        protected void Die() //called when animal dies by starvation due to reaching its food clock limit
        {
            // increase the count of dead for species sardine and shark. Only sardine and shark can die by starvation
            switch (Type)
            {
                case "sardine":
                    Program.sarDeadCnt++;
                    break;
                case "shark":
                    Program.shaDeadCnt++;
                    break;
                default:
                    break;
            }

            // animal becomes a carcass
            Type = "carcass";

            //the following get reset since they will no longer be used
            FoodClock = 0;
            FoodClockLimit = 0;
            ReproClock = 0;
            ReproClockLimit = 0;
        }

        protected bool isSafe(int rn, int cn) //determines if location in question is inside boundary of grid
        {
            if ((rn < 0 || rn > GridSpace.Size - 1) || (cn < 0 || cn > GridSpace.Size - 1))
                return false; 
            else 
                return true; 
        }


        protected void Reproduce() //occurs when creature is able to move and meets reproduction clock limit
        {
            // pass properties that will be inherited by new creature
            Program.PopulateCreature(Type, PrevRN, PrevCN, FoodClockLimit, ReproClockLimit);

            // reproduction clock is reset
            ReproClock = 0;
        }

        public void Move() //animal will simply move. No eating involved. All animals do this except crabs.
        {
            // save row and column number in case they change
            int curRN = RowNumber;
            int curCN = ColumnNumber;

            var rand = new Random(); 
            var opt = new List<int[]>(); //list of moving location options 
            int index; //index for options list
            int[] arr; //current moving location option inside loop
            int y, x; //coordinate changes inside loop
            int foundCell = 0; //flag var indicating empty cell was found
            int noOptions = 0; //flag var indicating no moving options were found                

            //location options animal will randomly move to if they are inside grid and empty
            int[] opt1 = { 0, 1 }; opt.Add(opt1);
            int[] opt2 = { 0, -1 }; opt.Add(opt2);
            int[] opt3 = { 1, 0 }; opt.Add(opt3);
            int[] opt4 = { -1, 0 }; opt.Add(opt4);
            int[] opt5 = { 1, 1 }; opt.Add(opt5);
            int[] opt6 = { 1, -1 }; opt.Add(opt6);
            int[] opt7 = { -1, 1 }; opt.Add(opt7);
            int[] opt8 = { -1, -1 }; opt.Add(opt8);

            do
            {
                if (opt.Count > 0)
                {
                    index = rand.Next(opt.Count); //store random number(0 to size of list) in index 
                    arr = opt[index]; // new coordinates
                    y = arr[0]; // new y-coordinate
                    x = arr[1]; // new x-coordinate

                    if (isSafe(RowNumber + (y), ColumnNumber + (x))) //check option is inside the grid
                    {
                        int newRN = RowNumber + (y); //store possible change of row number
                        int newCN = ColumnNumber + (x); //store possible change of column number
                        
                        //find if there is an animal in that location
                        MarineAnimal ma = Program.GetElementByCoordinates(newRN, newCN);

                        if (ma == null) //if space is empty
                        {
                            //row and column number changed to new cell location                            
                            RowNumber = newRN;
                            ColumnNumber = newCN; 
                            
                            //indicate an available cell was found
                            foundCell = 1;
                        }
                        else
                            opt.Remove(opt[index]); //remove option from list of options
                    }
                    else
                        opt.Remove(opt[index]); //remove option from list of options
                }
                else
                {
                    noOptions = 1; //indicate no available options were found
                }
            }
            while (foundCell == 0 && noOptions == 0); 

            // check if row and/or column number changed, meaning animal was able to move. If so, do the following
            if ((curRN != RowNumber) || (curCN != ColumnNumber))
            {
                //store previous location
                PrevRN = curRN;
                PrevCN = curCN;
                //reproduction clock increments
                ReproClock++;

                if (ReproClock >= ReproClockLimit)
                {
                    Reproduce(); //animal will leave offspring in previous location
                }
            }
        }

    }
}
