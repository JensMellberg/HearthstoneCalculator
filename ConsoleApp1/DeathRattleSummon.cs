using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class DeathRattleSummon : Effect

    {
    string summon;
    int count;
    public DeathRattleSummon(string summon, int count) : base()
    {
        this.count = count;
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattlesummon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < count; i++)
            board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i,1);
    }
    public override Effect makeGolden()
    {
        return new DeathRattleSummonGolden(summon,count);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is DeathRattleSummon))
            return false;
        if (summon != ((DeathRattleSummon)other).summon)
            return false;
        if (count != ((DeathRattleSummon)other).count)
            return false;
        return true;
    }
}

