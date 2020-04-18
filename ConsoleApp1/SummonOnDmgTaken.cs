using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SummonOnDmgTaken : Effect

    {
    string summon;
    public SummonOnDmgTaken(string summon) : base()
    {
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: summon on damage taken: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user),false);
    }
    public override Effect makeGolden()
    {
        return new SummonOnDmgTakenGolden(summon);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DamageTakenAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is SummonOnDmgTaken))
            return false;
        if (summon != ((SummonOnDmgTaken)other).summon)
            return false;
        return true;
    }
}

