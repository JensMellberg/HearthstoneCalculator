using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class KangorSummon : Effect

    {
    int count;
    public KangorSummon(int count) : base()
    {
        this.count = count;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: kangorssummon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        int currentCount = count;
        BoardSide current = board.getBoardFromMinion(user);
        int counterBcLazy = 0;
        for (int i = 0; i < current.graveyard.Count && currentCount > 0; i++)
        {
            if (Card.typesMatches(current.graveyard[i].type, Card.Type.Mech))
            {
                board.addNewMinionToBoard(board.getPlayerFromMinion(user), current.graveyard[i].revive().setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+counterBcLazy);
                counterBcLazy++;
                currentCount--;
            }
        }
        
    }
    public override Effect makeGolden()
    {
        return new KangorSummon(count * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is KangorSummon))
            return false;
        if (count != ((KangorSummon)other).count)
            return false;
        return true;
    }
}

