using System;
using System.Collections.Generic;
using System.Text;

namespace EcologicalSystemConsole
{
    public class Crab : MarineAnimal
    {
        public Crab(string type, int rn, int cn) : base(rn, cn)
        {
            this.Type = type;
        }

        public void Eat()
        {
            var ediOpt = new List<int[]>(); // list of edible options for sardines.
            var opt = new List<int[]>(); // list of all options for possible move
            int[] arr; // to be used as current option inside loop
            int y, x; // to be used as coordinate changes inside loop
            int index;
            var rand = new Random();

            // this options are the changes that will be added to the row and column number
            // thus covering all the possible cells object can move to. Also added each option to opt list
            int[] opt1 = { 0, 1 }; opt.Add(opt1);
            int[] opt2 = { 0, -1 }; opt.Add(opt2);
            int[] opt3 = { 1, 0 }; opt.Add(opt3);
            int[] opt4 = { -1, 0 }; opt.Add(opt4);
            int[] opt5 = { 1, 1 }; opt.Add(opt5);
            int[] opt6 = { 1, -1 }; opt.Add(opt6);
            int[] opt7 = { -1, 1 }; opt.Add(opt7);
            int[] opt8 = { -1, -1 }; opt.Add(opt8);

            // iterate the number of options
            for (int i = 0; i < opt.Count; i++)
            {
                arr = opt[i]; // change for coordinates
                y = arr[0]; // change for y-coordinate
                x = arr[1]; // change for x-coordinate

                if (isSafe(RowNumber + (y), ColumnNumber + (x)))
                {
                    int newRN = RowNumber + (y);
                    int newCN = ColumnNumber + (x);
                    MarineAnimal ma = Program.GetElementByCoordinates(newRN, newCN);

                    if (ma != null)
                    {
                        if (ma.Type == "carcass")
                        {
                            ediOpt.Add(arr);
                        }
                    }
                }
            }

            if (ediOpt.Count > 0)
            {
                // save currrent row and column
                int curRN = RowNumber;
                int curCN = ColumnNumber;
                // ger random number between 0 and amount of options in ediOpt
                index = rand.Next(ediOpt.Count);
                // selected coordinates
                int[] select = ediOpt[index];
                int newRN = RowNumber + (select[0]);
                int newCN = ColumnNumber + (select[1]);
                MarineAnimal ma = Program.GetElementByType("carcass", newRN, newCN);

                if (ma != null)
                {
                    Program.cloneList.Remove(ma);
                    Program.carEatenCnt++;
                    //Console.WriteLine($"remove {Program.removeCnt}:{ma.Type}\t{ma.RowNumber}\t{ma.ColumnNumber}");
                }
                else
                    Console.WriteLine("Error obj to eat not found");

                // change row and column number to new values
                RowNumber = newRN;
                ColumnNumber = newCN;
                // store old values
                PrevRN = curRN;
                PrevCN = curCN;
            }
        }//end of Eat() method
    }//end of class
}






