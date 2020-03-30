using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MalGanisSoT : Effect

    {
    int buff;
    public MalGanisSoT(int buff)
    {
        this.buff = buff;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: Mal'ganis hacky SoT effect " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        foreach (Card c in board.getBoardFromMinion(user))
        {
            if (c != user && c.typeMatches(Card.Type.Demon))
                c.addStats(buff, buff);
        }
    }
    public override Effect makeGolden()
    {
        return new MalGanisSoT(buff * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is FinilizedSoTAction)
            return true;
        return false;
    }

    public override void makeUpForReaderError(Card user, HearthstoneBoard board)
    {
        buff = 0;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is MalGanisSoT))
            return false;
        if (buff != ((MalGanisSoT)other).buff)
            return false;
        return true;
    }
}

