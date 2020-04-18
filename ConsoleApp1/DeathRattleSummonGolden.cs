using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DeathRattleSummonGolden : Effect

    {
    string summon;
    int count;
    public DeathRattleSummonGolden(string summon, int count) : base()
    {
        this.count = count;
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattlesummon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < count; i++)
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createGoldenFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i,true);
    }

    public override Effect makeGolden()
    {
        throw new NotImplementedException();
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is DeathRattleSummonGolden))
            return false;
        if (summon != ((DeathRattleSummonGolden)other).summon)
            return false;
        if (count != ((DeathRattleSummonGolden)other).count)
            return false;
        return true;
    }
}

