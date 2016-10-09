using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ConsoleApplication3
{

    class Program {
        public static  Computer prototype = new Computer("proto", true, 100000.0, new int?[] { 1, 2, 3, 4, 5 }, 20000);
        public static Computer userProto;

        static void Main(string[] args) {
            Computer[] comps = new Computer[10];
            int arrayPointer = 0;

             prototype = new Computer("proto", true, 100000.0, new int?[] { 1, 2, 3, 4, 5}, 20000);
            
            Console.WriteLine("Welcome to ComputerTracker 2.3 database application");
            int choice;
            while (true)
            {
                Console.WriteLine("\n Choose an option \n" +
                    "1. Add a new Computer \n" +
                    "2. Create a prototype (default) computer \n" +
                    "3. Get information about a specific Computer \n" +
                    "4. Get information about all the computers \n" +
                    "5. Get a information about a specific portion from the computers");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        if (arrayPointer == 10) {
                            Console.WriteLine("Overwrite which entry? (0-9)");
                            try {
                                comps[Int32.Parse(Console.ReadLine())] = NewComp();
                            }
                            catch {
                                Console.WriteLine("invalid entry. Overwriting last computer instead.");
                                comps[9] = NewComp();
                            }
                        } else {

                            comps[arrayPointer] = NewComp();
                            arrayPointer += (arrayPointer == 10) ? 0 : 1;
                        }
                        break;
                    case 2:
                        userProto = NewComp();
                        break;
                    case 3:
                        Console.WriteLine("Which computer would you like information on? (0-9)");
                        try
                        {
                            Console.WriteLine(comps[Int32.Parse(Console.ReadLine())].ToString());
                        } catch (NullReferenceException) { Console.WriteLine(prototype); } 
                        catch (IndexOutOfRangeException) { Console.WriteLine("invalid request ignored. Integer from 0-9 needed"); }
                        break;
                    case 4:
                        getSummary(comps.ToList().GetRange(0, arrayPointer).ToArray());
                        break;
                    case 5:
                        getRange(comps);
                        break;
                    default:
                        break;
                }
                

            }

        }

        static Computer NewComp()
        {
            String id;
            bool? hasAntenna;
            double? storageCap;
            int?[] licenses = new int?[5];
            int RAM;

            Console.WriteLine("input device id tag");
            id = Console.ReadLine();

            Console.WriteLine("device has antenna? (true/false/null)");
            try
            {
                hasAntenna = Boolean.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                hasAntenna = null;
            }

            Console.WriteLine(
                "What is the device storage capacity? (put a non numeric value to denote the device has no storage)");
            try
            {
                storageCap = Double.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                storageCap = null;
            }

            Console.WriteLine("any licenses? (yes/no)");
            if ("yes".Equals(Console.ReadLine()))
            {
                Console.WriteLine("input the licenses with commas seperating each entry \n" +
                                  "ex: 2345, 76543, null, 0, 23456");
                String[] licenseStrings = Console.ReadLine().Replace(" ", "").Split(',');

                for (int i = 0; i < licenseStrings.Length; i++)
                {
                    try {
                        licenses[i] = Int32.Parse(licenseStrings[i]);
                    } catch (Exception) {
                        licenses[i] = null;
                    }
                }
            }

            Console.WriteLine("What is the Ram size?");
            try
            {
                int size = Int32.Parse(Console.ReadLine());
                if(size < 1000) { throw new Exception();}
                RAM = size;
            } catch (Exception){
                Console.WriteLine("size not correct. Ram sized to 1000 instead");
                RAM = 1000;
            }

            return new Computer(id, hasAntenna, storageCap, licenses, RAM);
        }

        static void getRange(Computer[] comps) {
            int lowerBound;
            int upperBound;
            
            Console.WriteLine("Specify the lower bound you would like to use");
            lowerBound = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Specify the upper bound you would like to use");

            upperBound = Int32.Parse(Console.ReadLine());

            Computer[] compsBounded = new Computer[upperBound+1 - lowerBound];

            int counter = 0;
            for (int i = lowerBound; i <= upperBound; i++) {
                compsBounded[counter] = comps[i];
                counter++;
            }

            getSummary(compsBounded);
        }

        static void getSummary(Computer[] comps) {
            double avgRam = 0;

            double countAntenna = 0;
            double pcntAntenna = 0;

            double avgStorage = 0;
            double countStorage = 0;

            double avgLicenses = 0;
            double avgInstalls = 0;

            foreach (Computer comp in comps) {
                Computer compI = comp ?? userProto ?? prototype;

                avgRam += compI.TotalRam;

                if (compI.hasAntenna.HasValue) {
                    pcntAntenna += (compI.hasAntenna.Value) ? 1 : 0;
                    countAntenna++;
                }

                if (compI.StorageCap.HasValue) {
                    avgStorage += compI.StorageCap.Value;
                    countStorage++;
                }

                foreach (int? license in compI.licenses) {
                    if (license.HasValue) {
                        avgLicenses++;
                        avgInstalls += (license.Value != 0) ? 1 : 0;
                    }
                }

               
            }

            avgRam /= comps.Length; //only given until pointer 
            pcntAntenna /= countAntenna;
            avgStorage /= countStorage;
            avgLicenses /= comps.Length;
            avgInstalls /= comps.Length;

            Console.WriteLine("Summary: for " + comps.Length + " computer(s) \n" +
                              "------------------------------ \n" +
                              "   Average system storage:     " + avgStorage + " across " + countStorage + "systems \n" +
                              "   Average system memory:      " + avgRam + " across all systems \n" +
                              "   Percentage with Antenna:    " + pcntAntenna + " across " + countAntenna + "systems \n" +
                              "   Average number of licenses: " + avgLicenses + " across all systems \n" +
                              "   Average number of installs: " + avgInstalls + " across all systems \n" +
                              "-------------------------------");


        }

    }

    class Computer {
        private string id {get;}
        

        public bool? hasAntenna { get; }

        private double? storageCap;
        public double? StorageCap {
            get {
                return storageCap;
            }
            set {
                if(value < 0) {
                    Console.WriteLine("Error, storage must be positive. Value give was: " + value);
                }
            }
        }

        public int?[] licenses { get; }

        private int totalRam;
        public int TotalRam {
            get {
                int usedRam = 0;
                foreach(int? license in licenses){
                    if(license.HasValue && license.Value != 0) {
                        usedRam += 10;
                    }
                }
                //current RAM
                return totalRam - usedRam - (hasAntenna.HasValue && hasAntenna.Value ? 100 : 50);
            }

            set {
                if (value < 1000)
                {
                    Console.WriteLine(
                        "Computer can't have less than 1000 units of RAM. RAM not set and is instead still: " + totalRam);
                }
                totalRam = value;

            }
        }
            

        public Computer(String id, bool? hasAntenna, double? storageCap, int?[] licenses, int totalRam)
        {
            this.id = id;
            this.hasAntenna = hasAntenna;
            this.storageCap = storageCap;
            this.licenses = licenses;
            this.totalRam = totalRam;
        }

        public Computer()
        {
            
        }

        public override string ToString()
        {
            return "Computer Id: " + id +
                   "\n Storage capacity: " + storageCap +
                   "\n RAM: " + totalRam +
                   "\n This device has " + (hasAntenna.HasValue && hasAntenna.Value ? "an" : "NO") + " antenna" +
                   "\n licenses: " + String.Join(",", licenses.Select(s => s.ToString()).ToArray());
        }
    }
}
