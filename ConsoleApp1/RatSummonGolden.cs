﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class RatSummonGolden : Effect

    {
    CardCreatorFactory.Cards summon;
    public RatSummonGolden(CardCreatorFactory.Cards summon) : base()
    {
        this.summon = summon;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: ratsummon: " + user);
        int count = user.getAttack(board);
        for (int i = 0; i < count; i++)
             board.addNewMinionToBoard(board.getPlayerFromMinion(user), CardCreatorFactory.createGoldenFromName(summon).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i);
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

