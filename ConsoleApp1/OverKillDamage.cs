using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class OverKillDamage : Effect

    {
    int dmg;
    public OverKillDamage(int dmg) : base()
    {
        this.dmg = dmg;
    }

    public override Effect makeGolden()
    {
        return new OverKillDamage(dmg * 2);
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: overkill fire: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        BoardSide opponentBoard = board.getOpponentBoardFromMinion(user);
      
        if (opponentBoard.Count != 0)
        {
            for (int i = 0; i < opponentBoard.Count; i++)
                if (opponentBoard[i].isAlive())
                {
                    user.causeDamageToTarget(opponentBoard[i], board, dmg);
                    break;
                }
        }
           


      
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is OverKillAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is OverKillDamage))
            return false;
        if (dmg != ((OverKillDamage)other).dmg)
            return false;
        return true;
    }


}

