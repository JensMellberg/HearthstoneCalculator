﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class DeathRattleSummon : Effect

    {
    CardCreatorFactory.Cards summon;
    public DeathRattleSummon(CardCreatorFactory.Cards summon) : base()
    {
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: deathrattlesummon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user));
    }
    public override Effect makeGolden()
    {
        return new DeathRattleSummonGolden(summon);
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
        return true;
    }
}

