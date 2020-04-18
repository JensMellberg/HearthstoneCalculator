using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class StatisticsManager
    {
        public static double[] calculateAverageWins(List<HearthstoneBoard> boards)
        {
            int[] wins = { 0, 0, 0 };
            foreach (HearthstoneBoard b in boards)
                wins[b.getWinner()]++;
            return new double[] { (double)wins[0] / boards.Count, (double)wins[1] / boards.Count, (double)wins[2] / boards.Count };
        }
        public static string[] labels = { "Draw", "Win", "Loss" };
        public static ConsoleColor[] colors = { ConsoleColor.Gray, ConsoleColor.Green,ConsoleColor.Red };
        public static Color[] textcolors = { Color.Gray, Color.Green, Color.Red };
        public static void printExpectedResult(double[] avrg)
        {
            int res = expectedResult(avrg);
            ConsoleColor def = Console.ForegroundColor;
            Console.ForegroundColor = colors[res];
            Console.WriteLine("Expected result: " + labels[res]);
            Console.ForegroundColor = def;
        }
        public static Expected expectedResultAndColor(double[] avrg)
        {
            int res = expectedResult(avrg);
            return new Expected(labels[res], textcolors[res]);
        }

        //Calculate the chance to do "dmg" damage or more from a given distribution
        public static double chanceForDamageOrMore(int dmg, List<DmgDistEntry> dmgdist)
        {
            double percentage = 0;
            Func<int, int, bool> op = (dmg > 0) ? (Func<int, int, bool>)((x, y) => x >= y) : ((x, y) => x <= y);
             foreach (var d in dmgdist)
                 if (op(d.damage, dmg))
                     percentage += d.percentage;
            return percentage;
        }

        //Find the worst and best result from a list of board results
        public static HearthstoneBoard[] findWorstAndBest(List<HearthstoneBoard> boards)
        {

            HearthstoneBoard best = boards[0];
            HearthstoneBoard worst = boards[0];
            foreach (HearthstoneBoard b in boards)
            {
                if (b.getFinalizedDamage() > best.getFinalizedDamage())
                    best = b;
                if (b.getFinalizedDamage() < worst.getFinalizedDamage())
                    worst = b;
            }
            return new HearthstoneBoard[] { best, worst };
        }

        //"Draw", "Win", "Loss" 
        public static int expectedResult(double[] avrg)
        {
            double highest = avrg[0];
            int pos = 0;
            if (avrg[1] > highest)
            {
                pos = 1;
                highest = avrg[1];
            }
            if (avrg[2] > highest)
            {
                pos = 2;
                highest = avrg[2];
            }
            return pos;
        }

        public static List<DmgDistEntry> calculateDmgDistributions(List<HearthstoneBoard> boards)
        {
            List<DmgDistEntry> ret = new List<DmgDistEntry>();
            var counts = new SortedDictionary<int, int>();
            foreach (HearthstoneBoard b in boards)
            {
                int dmg = b.getFinalizedDamage();
                if (counts.ContainsKey(dmg))
                    counts[dmg] = counts[dmg] + 1;
                else
                    counts[dmg] = 1;
            }
            foreach (KeyValuePair<int, int> entry in counts)
                ret.Add(new DmgDistEntry(entry.Key, (double)entry.Value/(double)boards.Count));
            return ret;

        }

        public static void printReadableResult(List<DmgDistEntry> dmgdist)
        {
            Console.WriteLine("Damage distributions: ");
            foreach (DmgDistEntry d in dmgdist)
            {
                Console.WriteLine(d.damage + ": " + d.percentage + "%");
            }
            Console.WriteLine("---------");
            Console.WriteLine();
        }

        public static void printReadableResult(double[] avrgs)
        {
            Console.WriteLine("Chance of Draw: "+ Math.Round(avrgs[0] * 100, 1) + "%");
            Console.WriteLine("Chance of Win: " + Math.Round(avrgs[1] * 100, 1) + "%");
            Console.WriteLine("Chance of Loss: " + Math.Round(avrgs[2] * 100, 1) + "%");
            Console.WriteLine();
        }

        //Calculated expected value (väntevärde)
        public static int mostLikelyOutcome(List<DmgDistEntry> dmgdist)
        {
            double value = 0;
            foreach (DmgDistEntry d in dmgdist)
            {
                value += (double)d.damage * (d.percentage/100);
            }
            return (int)Math.Round(value,0);
        }
        /*   public static int mostLikelyOutcome(List<DmgDistEntry> dmgdist)
        {
            DmgDistEntry likely = new DmgDistEntry(1, 0);
            foreach (DmgDistEntry d in dmgdist)
            {
                if (d.percentage > likely.percentage)
                    likely = d;
            }
            return likely.damage;
        }*/

        public struct DmgDistEntry
        {
            public DmgDistEntry(int dmg, double perc)
            {
                damage = dmg;
                percentage = Math.Round(perc*100,1);
            }
          public  int damage;
            public double percentage;
        }

        public struct Expected
        {
            public Expected(string result, Color color)
            {
                this.result = result;
                this.color = color;
            }
            public string result;
            public Color color;
        }
    }
}
