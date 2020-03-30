using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class DragonDivine : Effect

    {
    Card.Type type;
    public DragonDivine(Card.Type type)
    {
        this.type = type;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattle dragon divine: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        foreach (Card c in board.getBoardFromMinion(user))
        {
            if (c.typeMatches(type))
                c.setDivineShield(true);
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
        if (!(other is DragonDivine))
            return false;
        if (type != ((DragonDivine)other).type)
            return false;
        return true;
    }

    public override Effect makeGolden()
    {
        return this;
    }
}

