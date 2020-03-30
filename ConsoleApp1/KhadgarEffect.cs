using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class KhadgarEffect : Effect

    {

    int count;
    int current;
    public KhadgarEffect(int count) : base()
    {
        this.count = count;
        current = count;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: make extra summon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        BoardSide userboard = board.getBoardFromMinion(user);
        for (int i = 0; i < userboard.IndexOf(user); i++)
            if (userboard[i].hasEffect(this))
                return;
        if (current < 1)
            current = count;
        else
        {
            Card c = ((CardSpawnedAction)cause).spawnedCard();
            if (c == user)
                return;
            current--;
            board.printDebugMessage("khadgar effect used, count is now " + current, HearthstoneBoard.OutputPriority.INTENSEDEBUG);
            board.addNewMinionToBoard(board.getPlayerFromMinion(user), c.copy().setId(board.getRandomNumber(1, 9999)), board.getPositionFromMinion(c), 0);

        }
    }
    public override Effect makeGolden()
    {
        return new KhadgarEffect(count*2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is CardSpawnedAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is KhadgarEffect))
            return false;
        if (count != ((KhadgarEffect)other).count)
            return false;
        return true;
    }
}

