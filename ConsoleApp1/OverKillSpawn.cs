using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class OverKillSpawn : Effect

    {
    string spawn;
    public OverKillSpawn(string spawn)
    {
        this.spawn = spawn;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: overkillspawn: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createFromName(spawn).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user),false);
    }
    public override Effect makeGolden()
    {
        return new OverKillSpawnGolden(spawn);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is OverKillAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is OverKillSpawn))
            return false;
        if (spawn != ((OverKillSpawn)other).spawn)
            return false;
        return true;
    }
}

