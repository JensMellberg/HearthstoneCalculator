using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DeathAoE : Effect

    {
    int dmg;
    public DeathAoE(int dmg)
    {
        this.dmg = dmg;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: death AoE: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        List<Card> targets = new List<Card>();
        foreach (Card c in board.p1Board)
        {
            if (c != user)
                targets.Add(c);
              
        }
        foreach (Card c in board.p2Board)
        {
            if (c != user)
                targets.Add(c);
        }
        foreach (Card c in targets)
            user.causeDamageToTarget(c, board, dmg);
    }
    public override Effect makeGolden()
    {
        return new DeathAoE(dmg * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is DeathAoE))
            return false;
        if (dmg != ((DeathAoE)other).dmg)
            return false;
        return true;
    }
}

