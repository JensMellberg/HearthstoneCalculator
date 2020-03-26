using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class DeathRattleDivine : Effect

    {
    int times;
    public DeathRattleDivine(int times) : base()
    {
        this.times = times;
    }
    public override Effect makeGolden()
    {
        return new DeathRattleDivine(times * 2);
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattledivine: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        bool stop = true;
        BoardSide userBoard = board.getBoardFromMinion(user);

        for (int i = 0; i < times; i++)
        {
            foreach (Card c in userBoard)
            {
                if (c != user && !c.divineShield)
                {
                    stop = false;
                }
            }
            if (stop)
                return;
            while (true)
            {
                int target = HearthstoneBoard.getRandomNumber(0, userBoard.Count);
                if (!userBoard[target].divineShield && userBoard[target] != user)
                {
                    Console.WriteLine("Giving divine shield to " + userBoard[target].getReadableName());
                    userBoard[target].setDivineShield(true);
                    break; ;
                }
            }
        }
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }


    public override bool Compare(Effect other)
    {
        if (!(other is DeathRattleDivine))
            return false;
        if (times != ((DeathRattleDivine)other).times)
            return false;
        return true;
    }


}

