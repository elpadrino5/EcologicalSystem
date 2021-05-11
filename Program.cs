using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EcologicalSystemConsole
{
    public class Program
    {
        //size of square grid
        public static int gridSize = 16;
        //create the grid with specified size
        public static GridSpace ocean = new GridSpace(gridSize);
        //data structure that will store all objects
        public static List<MarineAnimal> superList = new List<MarineAnimal>();
        //clone of superList 
        public static List<MarineAnimal> cloneList = new List<MarineAnimal>();
        //used to shuffle list
        private static Random rand = new Random();

        /*reproduction clock limit is the amount of generations in which animal must move or eat before reproducing.
        When reached animal can reproduce. Crabs don't reproduce*/
        public static int plaReproClockLimit = 3;
        public static int sarReproClockLimit = 5;
        public static int shaReproClockLimit = 10;

        /*food clock limit is the amount of generations animal can last without eating.
        When reached animal dies. Only sardines and shark can die.*/
        public static int sarFoodClockLimit = 3;
        public static int shaFoodClockLimit = 6;

        //count of offspring in a generation
        public static int plaBornCnt;
        public static int sarBornCnt;
        public static int shaBornCnt;
        public static int totalBornCnt;
        //count of animals that were eaten for specified specie
        public static int plaEatenCnt;
        public static int sarEatenCnt;
        public static int shaEatenCnt;
        public static int carEatenCnt;
        public static int totalEatenCnt;
        //count of animals that died by starvation
        public static int sarDeadCnt;
        public static int shaDeadCnt;
        public static int totalDeadCnt;

        static void Main(string[] args)
        {
            //this limits of the square grid will be used to 
            int rowLimit = gridSize;
            int columnLimit = gridSize;

            int numObstacle = 4;
            int numPlankton = 20;
            int numSardine = 10;
            int numShark = 5;
            int numCrabs = 5;

            // populate ocean(grid) with obstacles and marine animals
            //PopulateSpecie(numObstacle, "obstacle", rowLimit, columnLimit);
            PopulateSpecie(numPlankton, "plankton", rowLimit, columnLimit, reproClockLimit: plaReproClockLimit);
            PopulateSpecie(numSardine, "sardine", rowLimit, columnLimit, sarFoodClockLimit, sarReproClockLimit);
            PopulateSpecie(numShark, "shark", rowLimit, columnLimit, shaFoodClockLimit, shaReproClockLimit);
            PopulateSpecie(numCrabs, "crab", rowLimit, columnLimit);            

            //for testing purposes
            //PrintList(cloneList);

            int genCnt = 0;  //counter for the generation number
            int simgen; //number of generations to simulate
            int total;
            int[] oldCntArr = new int[6];
            
            do
            {   
                if (genCnt <= 0)
                {
                    simgen = 1;
                }
                else
                {
                    // ask for the amount of generations that will be simulated in one go  
                    Console.WriteLine("Press any key to simulate the next generation...");
                    Console.ReadKey();
                    simgen = 1;
                }

                // loop through simulating the generation and printing the grid for the number specified above
                for (int i = 0; i < simgen; i++)
                {
                    Console.Clear();
                    cloneList.AddRange(superList);
                    // each animal will have a chance to move, eat, reproduce, and/or die
                    SimulateGeneration();
                    oldCntArr = PrintGrid(ocean, genCnt, oldCntArr);
                    
                    superList.Clear();
                    superList.AddRange(cloneList);
                    cloneList.Clear();                   
                    genCnt += 1;
                }
            }
            while (true);
        }

        public static MarineAnimal GetElementByType(string type, int rn, int cn)
        {
            foreach (var obj in cloneList)
            {
                if (obj.RowNumber == rn && obj.ColumnNumber == cn && obj.Type == type)
                {
                    return obj;
                }
            }
            return null;
        }

        public static MarineAnimal GetElementByCoordinates(int rn, int cn)
        {
            foreach (var obj in cloneList)
            {
                if (obj.RowNumber == rn && obj.ColumnNumber == cn)
                {
                    return obj;
                }
            }
            return null;
        }

        public static int[] GetRandomNumber(int rowLimit, int columnLimit)
        {
            int y, x;
            do
            {
                var y_rand = new Random();
                y = y_rand.Next(rowLimit);
                var x_rand = new Random();
                x = x_rand.Next(columnLimit);
            }
            while (GetElementByCoordinates(y,x) != null);

            int[] coordinates = new int[] { y, x };
            return coordinates;
        }

        // instantiates objs and adds each to a list of type MarineAnimal (superclass). assigns obj to cell. returns list
        public static void PopulateSpecie(int numSpecie, string type, int rowLimit, int columnLimit, int foodClockLimit = 0, int reproClockLimit = 0)
        {
            // create int array to store coordinates
            int[] coordinates;            
            //loops the number of creatures to populate for that specie
            for (int i = 0; i < numSpecie; i++)
            { 
                coordinates = GetRandomNumber(rowLimit, columnLimit);
                var obj = new MarineAnimal(coordinates[0], coordinates[1]);
                switch (type)
                {
                    case "obstacle":
                        break;
                    case "plankton":
                         obj = new Plankton(type, coordinates[0], coordinates[1], reproClockLimit);
                        break;
                    case "sardine":
                        obj = new Sardine(type, coordinates[0], coordinates[1], foodClockLimit, reproClockLimit);
                        break;
                    case "shark":
                        obj = new Shark(type, coordinates[0], coordinates[1], foodClockLimit, reproClockLimit);
                        break;
                    case "crab":
                        obj = new Crab(type, coordinates[0], coordinates[1]);
                        break;
                    default:
                        break;
                }
                cloneList.Add(obj);
            }
        }

        public static void PopulateCreature(string type, int row, int column, int foodClockLimit = 0, int reproClockLimit = 0)
        {
            // instantiate an object with corresponding class depending on "type" of marine animal
            MarineAnimal obj = new MarineAnimal(row, column);
            switch (type)
            {
                case "obstacle":
                    //obj = new Obstacle(type, row, column, 5, reproClock);
                    break;
                case "plankton":
                    obj = new Plankton(type, row, column, reproClockLimit);
                    plaBornCnt++;
                    break;
                case "sardine":
                    obj = new Sardine(type, row, column, foodClockLimit, reproClockLimit);
                    break;
                    sarBornCnt++;
                case "shark":
                    obj = new Shark(type, row, column, foodClockLimit, reproClockLimit);
                    shaBornCnt++;
                    break;
                case "crab":
                    obj = new Crab(type, row, column);
                    break;
                default:
                    break;
            }
            //Console.WriteLine($"Offs {addCnt}: {obj.Type}\t{obj.RowNumber}\t{obj.ColumnNumber}");
            cloneList.Add(obj);            
        }

        private static void PrintList(List<MarineAnimal> list)
        {
            foreach (var k in list)
            {                
                Console.WriteLine($"{k.Type}\t{k.RowNumber}\t{k.ColumnNumber}\t");
            }
            Console.WriteLine($"Animals:{list.Count}\n");
        }

        private static int[] PrintGrid(GridSpace ocean, int generation, int [] oldCntArr)
        {
            int planktonCnt = 0;
            int sardineCnt = 0 ;
            int sharkCnt = 0;
            int crabCnt = 0;
            int carcassCnt = 0;
            int totalCnt;
            int trueCellCnt = 0;

            int oldplanktonCnt = 0;
            int oldsardineCnt = 0;
            int oldsharkCnt = 0;
            int oldcrabCnt = 0;
            int oldcarcassCnt = 0;

            Console.WriteLine("Generation:{0}", generation);
            for (int i = 0; i < GridSpace.Size; i++)
            {
                for (int k = 0; k < GridSpace.Size; k++)
                {
                    Console.Write("+---");
                }
                Console.Write("+\n");

                for (int j = 0; j < GridSpace.Size; j++)
                {
                    MarineAnimal ma = GetElementByCoordinates(i, j);
                    if (ma != null)
                    //if (ocean.theGrid[i, j].CurrentlyOccupied == true)
                    {
                        trueCellCnt++;
                        //string type = ocean.theGrid[i, j].marAnimal.Type;
                        string type = ma.Type;
                        switch (type)
                        {
                            case "obstacle":                                
                                break;
                            case "plankton":
                                Console.Write("| * ");
                                break;
                            case "sardine":
                                Console.Write("| o<");  
                                break;
                            case "shark":
                                Console.Write("| S ");
                                break;
                            case "crab":
                                Console.Write("|>=<");
                                break;
                            case "carcass":
                                Console.Write("| - ");
                                break;
                            default:
                                Console.Write("| ! ");
                                break;
                        }                        
                    }
                    else
                    {
                        Console.Write("|   ");
                    }
                }
                Console.Write("|\n");
            }

            for (int t = 0; t < GridSpace.Size; t++)
            {
                Console.Write("+---");
            }
            Console.Write("+\n\n");

            foreach (var obj in cloneList)
            {
                switch (obj.Type)
                {
                    case "plankton":
                        planktonCnt++;                         
                        break;
                    case "sardine":
                        sardineCnt++;
                        break;
                    case "shark":
                        sharkCnt++;
                        break;
                    case "crab":
                        crabCnt++;
                        break;
                    case "carcass":
                        carcassCnt++;
                        break;
                    default:
                        break;
                }
            }
            //calculate the totals
            totalCnt = planktonCnt + sardineCnt + sharkCnt + crabCnt + carcassCnt; //total amount of animals
            totalBornCnt = plaBornCnt + sarBornCnt + shaBornCnt; //total amount of born animals
            totalDeadCnt = sarDeadCnt + shaDeadCnt; //total amount of animals dead by starvation
            totalEatenCnt = plaEatenCnt + sarEatenCnt + shaEatenCnt; //total amount of eaten animals

            //display statistics 
            Console.WriteLine("\t\t  Plankton(*)\tSardine(o<)\tShark(S)\tCrab(>=<)\tCarcass(-)\tTotal"); //heading
            Console.WriteLine("\t\t+---------------------------------------------------------------------------------------+");
            Console.WriteLine("" + "      OldTotal  |  {0}\t\t {1}\t\t {2}\t\t {3}\t\t {4}\t\t|  {5}\t|", oldCntArr[0], oldCntArr[1], oldCntArr[2], oldCntArr[3], oldCntArr[4], oldCntArr[5]);
            Console.WriteLine($"\t  Born  | +{plaBornCnt}\t\t+{sarBornCnt}\t\t+{shaBornCnt}\t\t\t\t\t\t| +{totalBornCnt}\t|");
            Console.WriteLine($"\t  Dead  | \t\t-{sarDeadCnt}\t\t-{shaDeadCnt}\t\t\t\t+{totalDeadCnt}\t\t|\t|");
            Console.WriteLine($"\t Eaten  | -{plaEatenCnt}\t\t-{sarEatenCnt}\t\t-{shaEatenCnt}\t\t\t\t-{carEatenCnt}\t\t| -{totalEatenCnt}\t|");
            Console.WriteLine("\t\t+---------------------------------------------------------------------------------------+");
            Console.WriteLine($"\t Total\t|  {planktonCnt}\t\t {sardineCnt}\t\t {sharkCnt}\t\t {crabCnt}\t\t {carcassCnt}\t\t|  {totalCnt}\t|"); //total row
            Console.WriteLine("\t\t+---------------------------------------------------------------------------------------+\n\n\n\n");

            //store the amount of animals for each specie in an array, return it, and use it in next iteration
            int[] oldCnt = {planktonCnt,sardineCnt,sharkCnt,crabCnt,carcassCnt,totalCnt};

            //reset counters
            plaBornCnt = 0; sarBornCnt = 0; shaBornCnt = 0;  totalBornCnt = 0; //born animals counter
            plaEatenCnt = 0; sarEatenCnt = 0; shaEatenCnt = 0;  carEatenCnt = 0; totalEatenCnt = 0; //eaten animals counter
            sarDeadCnt = 0; shaDeadCnt = 0; totalDeadCnt = 0; //dead animals counter

            //return int[] of animal counters
            return oldCnt;
        }

        private static void SimulateGeneration()
        {
            //shuffle superlist before iterating so order is random.
            ShuffleList(superList);
            foreach (var i in superList)
            {
                if (cloneList.Contains(i))
                {
                    //Console.WriteLine($"{i.Type}\t{i.RowNumber}\t{i.ColumnNumber}");
                    // type of current obj
                    string type = i.Type;
                    bool eaten = false;
                    // switch base on the type of animal
                    switch (type)
                    {
                        case "obstacle":
                            break;

                        case "plankton":
                            Plankton pla = (Plankton)i;
                            pla.Move();
                            break;

                        case "sardine":
                            Sardine sar = (Sardine)i;
                            sar.Eat();
                            break;

                        case "shark":
                            Shark sha = (Shark)i;
                            eaten = sha.Eat();
                            if (eaten)
                            {
                                cloneList.Remove(sha);
                                shaEatenCnt++;
                            }
                            break;

                        case "crab":
                            Crab cra = (Crab)i;
                            cra.Eat();
                            break;

                        case "carcass":
                            break;

                        default:
                            break;
                    }              
                }
            }
        }

        private static void ShuffleList<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {                
                int e = rand.Next(n);
                n--;
                T value = list[e];
                list[e] = list[n];
                list[n] = value;
            }
        }
        
    }//end of class
}
