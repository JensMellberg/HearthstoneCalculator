using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ArtificialSummon : Effect

    {
    string summon;
    int count;
    public ArtificialSummon(string summon, int count) : base()
    {
        this.count = count;
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattlesummon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < count; i++)
            board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i, true);
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
        if (!(other is ArtificialSummon))
            return false;
        if (summon != ((ArtificialSummon)other).summon)
            return false;
        if (count != ((ArtificialSummon)other).count)
            return false;
        return true;
    }
}

