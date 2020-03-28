using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class SummonOnDmgTakenGolden : Effect

    {
    string summon;
    public SummonOnDmgTakenGolden(string summon) : base()
    {
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: summon on damage taken: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createGoldenFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user),0);
    }
    public override Effect makeGolden()
    {
        throw new NotImplementedException();
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DamageTakenAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is SummonOnDmgTakenGolden))
            return false;
        if (summon != ((SummonOnDmgTakenGolden)other).summon)
            return false;
        return true;
    }
}

