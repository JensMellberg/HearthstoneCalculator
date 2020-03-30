using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MalGanisDeath : Effect

    {
    int buff;
    public MalGanisDeath(int buff)
    {
        this.buff = buff;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: malganis death debuffer: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        foreach (Card c in board.getBoardFromMinion(user))
        {
            if (c != user && c.typeMatches(Card.Type.Demon))
            {
                c.addStats(-buff, -buff);
                if (c.getHp(board) < 1)
                    c.setHp(1);
            }
        }
    }
    public override Effect makeGolden()
    {
        return new MalGanisDeath(buff * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is MalGanisDeath))
            return false;
        if (buff != ((MalGanisDeath)other).buff)
            return false;
        return true;
    }
}

