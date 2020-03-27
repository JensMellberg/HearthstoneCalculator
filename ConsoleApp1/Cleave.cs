using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class Cleave : Effect

    {
    public Cleave() : base()
    {
    }
    public override Effect makeGolden()
    {
        return this;
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: cleave attack: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        Card target = ((AttackingAction)cause).target;
        var adj = board.getAdjacents(target);
        foreach (Card c in adj)
            user.causeDamageToTarget(c, board, user.getAttack(board));
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is AttackingAction)
            return true;
        return false;
    }


    public override bool Compare(Effect other)
    {
        if (!(other is Cleave))
            return false;
        return true;
    }


}

