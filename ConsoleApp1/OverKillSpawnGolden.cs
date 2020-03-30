using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class OverKillSpawnGolden : Effect

    {
    string spawn;
    public OverKillSpawnGolden(string spawn)
    {
        this.spawn = spawn;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: overkillspawn: " + user.getReadableName(),HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createGoldenFromName(spawn).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user),0);
    }
    public override Effect makeGolden()
    {
        throw new NotImplementedException();
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is OverKillAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is OverKillSpawnGolden))
            return false;
        if (spawn != ((OverKillSpawnGolden)other).spawn)
            return false;
        return true;
    }
}

