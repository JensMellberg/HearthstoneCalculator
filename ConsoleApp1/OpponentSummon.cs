using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class OpponentSummon : Effect

    {
    string summon;
    public OpponentSummon(string summon) : base()
    {
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattlesummon opponent: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        board.addNewMinionToBoard(board.getOpponentBoardFromMinion(user), CardCreatorFactory.createFromName(summon).setAttackPriority(Card.MAX_PRIORITY-1), -1,false);
    }
    public override Effect makeGolden()
    {
        return this;
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is OpponentSummon))
            return false;
        if (summon != ((OpponentSummon)other).summon)
            return false;
        return true;
    }
}

