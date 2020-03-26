﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class DeathTypeBuff : Effect

    {
    int buff;
    Card.Type type;
    public DeathTypeBuff(int buff, Card.Type type)
    {
        this.type = type;
        this.buff = buff;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        Console.WriteLine("Performing action: deathrattletypebuff: " + user.getReadableName());
        foreach (Card c in board.getBoardFromMinion(user))
        {
           if (c.typeMatches(type))
                c.addStats(buff, buff);
        }
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is DeathTypeBuff))
            return false;
        if (buff != ((DeathTypeBuff)other).buff)
            return false;
        if (type != ((DeathTypeBuff)other).type)
            return false;
        return true;
    }
}
