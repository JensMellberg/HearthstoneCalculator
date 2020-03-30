using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Juggler : Effect

    {
    int times;
    Card.Type type;
    public Juggler(int times, Card.Type type)
    {
        this.type = type;
        this.times = times;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: juggle on friendly death: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        BoardSide opponentBoard = board.getOpponentBoardFromMinion(user);
        List<Card> targets = new List<Card>();
        for (int i = 0; i < times; i++)
        {
            if (opponentBoard.Count == 0)
                return;
            Card target = opponentBoard.getRandomCardAlive(board);
            if (target == null)
                return;
            user.causeDamageToTarget(target, board, 3);
            targets.Add(target);
        }
        foreach (Card c in targets)
            c.deathCheck(board);
    }
    public override Effect makeGolden()
    {
        return new Juggler(times* 2, type);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is CardKilledAction)
            if (((CardKilledAction)a).killedCard().typeMatches(type))
                return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is Juggler))
            return false;
        if (times != ((Juggler)other).times)
            return false;
        if (type != ((Juggler)other).type)
            return false;
        return true;
    }
}

