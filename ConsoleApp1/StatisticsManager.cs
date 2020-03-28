using System;
using System.Collections.Generic;
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
            return new double[] { wins[0] / boards.Count, wins[1] / boards.Count, wins[2] / boards.Count };
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
    }
}
