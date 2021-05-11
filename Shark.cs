using System;
using System.Collections.Generic;
using System.Text;

namespace EcologicalSystemConsole
{
    class Shark : MarineAnimal
    {
        private int Weight { get; set; } //gets incremented after eating. Determines who will win when sharks fight

        public Shark(string type, int rn, int cn, int fcl, int rcl) : base(rn, cn)
        {
            this.Type = type;
            this.FoodClockLimit = fcl;
            this.ReproClockLimit = rcl;
            this.Weight = 0;
        }

        public bool Eat()
        {
            bool eatenByAnotherShark = false; //used to remove object from list outside of shark class

            //list of location animal can move to
            var opt = new List<int[]>();
            //list of locations that have edible creatures
            var optSar = new List<int[]>(); //sardines
            var optSha = new List<int[]>(); //other sharks
            var optEdi = new List<int[]>(); //sardines or sharks

            int[] arr; //current location option inside loop
            int y, x; //coordinate changes inside loop
            int index; //index of edible options
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

                if (isSafe(RowNumber + (y), ColumnNumber + (x))) //check option is inside the grid
                {
                    //store possible change of row and column number
                    int newRN = RowNumber + (y);
                    int newCN = ColumnNumber + (x);

                    //find if there is an animal in specified location
                    MarineAnimal ma = Program.GetElementByCoordinates(newRN, newCN);

                    if (ma != null) //determine if there's an animal or not
                    {
                        if (ma.Type == "sardine")
                        {
                            //add animal to sardine options list
                            optSar.Add(arr);
                        }
                        if (FoodClock == FoodClockLimit - 1) //only when animal is about to starve
                        {
                            if (ma.Type == "shark")
                            {
                                //add animal to shark options list
                                optSha.Add(arr);
                            }
                        }
                    }
                }
            }

            if (optSar.Count <= 0 && optSha.Count <= 0) //only move
            {
                Move();
                // increase animal food clock after moving and not eating
                FoodClock++;
                // check for survival limit because regardless it will not eat in this move. Thus it must die if limit is reached.
                if (FoodClock == FoodClockLimit)
                    Die();
            }
            else //eat and move
            {
                if (optSar.Count <= 0) //no sardines around
                    optEdi = optSha;
                else //only sharks around
                    optEdi = optSar;

                // save currrent row and column
                int curRN = RowNumber;
                int curCN = ColumnNumber;

                // ger random number between 0 and amount of options in ediOpt
                index = rand.Next(optEdi.Count);

                // selected coordinates
                int[] select = optEdi[index];

                //store possible change of row and column number
                int newRN = RowNumber + (select[0]);
                int newCN = ColumnNumber + (select[1]);

                //retrieve animal in specified location
                MarineAnimal ma = Program.GetElementByCoordinates(newRN, newCN);

                if (ma != null) //there's an animal
                {
                    if (ma.Type == "shark") //animal is another shark
                    {
                        //specified that marine animal is a shark
                        Shark sha = (Shark)ma; 

                        //the weight of both sharks determines who eats who
                        if (sha.Weight >= Weight) 
                        {
                            eatenByAnotherShark = true;
                            sha.FoodClock = 0;
                            sha.Weight++;
                            return eatenByAnotherShark;
                        }
                        else
                        {
                            Program.cloneList.Remove(sha);
                            Program.shaEatenCnt++;
                        }
                    }
                    else
                    {
                        Program.cloneList.Remove(ma);
                        Program.sarEatenCnt++;
                        //Console.WriteLine($"remove {Program.removeCnt}:{ma.Type}\t{ma.RowNumber}\t{ma.ColumnNumber}");
                    }
                }
                else
                    Console.WriteLine("Error obj to eat not found");

                // change row and column number to new values
                RowNumber = newRN;
                ColumnNumber = newCN;
                //Console.WriteLine($"Attacker won at {RowNumber}\t{ColumnNumber}\n");
                // store old values
                PrevRN = curRN;
                PrevCN = curCN;
                FoodClock = 0; // food clock resets after eating
                Weight++;
                ReproClock++; // increase clock after eating
                if (ReproClock >= ReproClockLimit)
                {
                    Reproduce();
                }
            }
            return eatenByAnotherShark;
        }
    }
}
