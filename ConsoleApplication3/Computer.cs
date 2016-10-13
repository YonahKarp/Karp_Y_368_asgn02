using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3 {
    class Computer {
        private string id { get; }


        public bool? hasAntenna { get; }

        private double? storageCap;
        public double? StorageCap {
            get {
                return storageCap;
            }
            set {
                if (value < 0) {
                    Console.WriteLine("Error, storage must be positive. Value give was: " + value
                        +"\nstorage set to 2000 instead");
                    storageCap = 2000;
                }
            }
        }

        public int?[] licenses { get; }

        private int totalRam;
        public int TotalRam {
            get {
                int usedRam = 0;
                foreach (int? license in licenses) {
                    if (license.HasValue && license.Value != 0) {
                        usedRam += 10;
                    }
                }
                //current RAM
                return totalRam - usedRam - (hasAntenna.HasValue && hasAntenna.Value ? 100 : 50);
            }

            set {
                if (value < 1000) {
                    Console.WriteLine(
                        "Computer can't have less than 1000 units of RAM. RAM not set and is instead still: " + totalRam);
                }
                totalRam = value;
            }
        }

        public Computer(String id, bool? hasAntenna, double? storageCap, int?[] licenses, int totalRam) {
            this.id = id;
            this.hasAntenna = hasAntenna;
            StorageCap = storageCap;
            this.licenses = licenses;
            TotalRam = totalRam;
        }

        public Computer() {
        }

        public override string ToString() {
            return "Computer Id: " + id +
                   "\n Storage capacity: " + storageCap +
                   "\n RAM: " + totalRam +
                   "\n This device has " + (hasAntenna.HasValue && hasAntenna.Value ? "an" : "NO") + " antenna" +
                   "\n licenses: " + String.Join(",", licenses.Select(s => s.ToString()).ToArray());
        }
    }
}
