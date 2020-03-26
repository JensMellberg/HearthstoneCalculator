using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class DeathRattleBomb : Effect

    {
    int times;
    public DeathRattleBomb(int times) : base()
    {
        this.times = times;
    }

    public override Effect makeGolden()
    {
        return new DeathRattleBomb(times * 2);
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: bomb deathrattle: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        BoardSide opponentBoard = board.getOpponentBoardFromMinion(user);

        for (int i = 0; i < times; i++)
        {
            if (opponentBoard.Count == 0)
                return;
            int target = HearthstoneBoard.getRandomNumber(0, opponentBoard.Count);
            user.causeDamageToTarget(opponentBoard[target], board, 4);
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
        if (!(other is DeathRattleBomb))
            return false;
        if (times != ((DeathRattleBomb)other).times)
            return false;
        return true;
    }


}

