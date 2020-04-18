using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class GUIstate
    {
        public List<StatisticsManager.DmgDistEntry> dmgdist;
        public double[] dmgdist2;
        public DrawableHearthstoneBoard board;
        public List<int> worstValues;
        public List<int> bestValues;
        public int expectedHealth;
        public string opponent;
        public double ranking;
        public double deathodds;
        public GUIstate(List<StatisticsManager.DmgDistEntry> dmgdist, double[] dmgdist2, DrawableHearthstoneBoard board, List<int> worstValues,
          List<int> bestValues, int expectedHealth, string opponent, double deathodds, double ranking)
        {
            this.ranking = ranking;
            this.dmgdist = dmgdist;
            this.dmgdist2 = dmgdist2;
            this.board = board.copy();
            this.worstValues = worstValues;
            this.bestValues = bestValues;
            this.opponent = opponent;
            this.deathodds = deathodds;
            this.expectedHealth = expectedHealth;
        }
    }
}
