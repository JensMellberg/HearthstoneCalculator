using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class RatSummonGolden : Effect

    {
    string summon;
    public RatSummonGolden(string summon) : base()
    {
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: ratsummon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        int count = user.getAttack(board);
        for (int i = 0; i < count; i++)
             board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createGoldenFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i,1);
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is RatSummonGolden))
            return false;
        if (summon != ((RatSummonGolden)other).summon)
            return false;
        return true;
    }

    public override Effect makeGolden()
    {
        throw new NotImplementedException();
    }
}

