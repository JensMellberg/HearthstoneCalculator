using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class DeathBuff : Effect

    {
    int buff;
    public DeathBuff(int buff)
    {
        this.buff = buff;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        Console.WriteLine("Performing action: deathrattlebuff: " + user.getReadableName());
        foreach (Card c in board.getBoardFromMinion(user))
        {
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
        if (!(other is DeathBuff))
            return false;
        if (buff != ((DeathBuff)other).buff)
            return false;
        return true;
    }
}

