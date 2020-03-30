using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SoTDragonFire : Effect

    {
    int times;
    public SoTDragonFire(int times) : base()
    {
        this.times = times;
    }

    public override Effect makeGolden()
    {
        return new SoTDragonFire(times * 2);
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: start of turn fire: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        BoardSide opponentBoard = board.getOpponentBoardFromMinion(user);
        BoardSide current = board.getBoardFromMinion(user);
        int counter = 0;
        foreach (Card c in current)
            if (c.typeMatches(Card.Type.Dragon))
                counter++;
        for (int i = 0; i < times; i++)
        {
            if (opponentBoard.Count == 0)
                return;
            int target = board.getRandomNumber(0, opponentBoard.Count);
            user.causeDamageToTarget(opponentBoard[target], board, counter);
        }
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is StartofCombatAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is SoTDragonFire))
            return false;
        if (times != ((SoTDragonFire)other).times)
            return false;
        return true;
    }


}

