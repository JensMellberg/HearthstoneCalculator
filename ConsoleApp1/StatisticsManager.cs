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
    }
}
