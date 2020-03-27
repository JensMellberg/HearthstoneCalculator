using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class Reborn : Effect

    {
    bool golden;
    public Reborn(bool golden)
    {
        this.golden = golden;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: reborn: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        if (golden)
             board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createGoldenFromName(user.cardID).setHp(1).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user));
        else
            board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createFromName(user.cardID).setHp(1).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user));

    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is Reborn))
            return false;
        if (golden != ((Reborn)other).golden)
            return false;
        return true;
    }

    public override Effect makeGolden()
    {
        return new Reborn(true);
    }
}

