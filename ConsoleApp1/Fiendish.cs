using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class Fiendish : Effect

    {
    int times;
    public Fiendish(int times) : base()
    {
        this.times = times;
    }
    public override Effect makeGolden()
    {
        return new Fiendish(times * 2);
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattle dmg bonus: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);

        BoardSide userBoard = board.getBoardFromMinion(user);

        if (userBoard.Count == 1)
            return;


        for (int i = 0; i < times; i++)
        {
            while (true)
            {
                int target = HearthstoneBoard.getRandomNumber(0, userBoard.Count);
                if (userBoard[target] != user)
                {
                    board.printDebugMessage("Giving damage bonus to " + userBoard[target].getReadableName(),HearthstoneBoard.OutputPriority.INTENSEDEBUG);
                    userBoard[target].addAttack(user.getAttack(board));
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
        if (!(other is Fiendish))
            return false;
        if (times != ((Fiendish)other).times)
            return false;
        return true;
    }


}

