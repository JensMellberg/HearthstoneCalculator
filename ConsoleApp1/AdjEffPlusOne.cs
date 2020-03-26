using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class AdjEffPlus : Effect


    {
    int bonus;
    public AdjEffPlus(int bonus) : base()
    {
        this.bonus = bonus;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        Console.WriteLine("Performing action: Adjacent + 1 from: " + user);
        foreach (Card c in alwaysUse)
        {
            c.tempAttackBonus += 1;
        }
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is AdjacentAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is AdjEffPlus))
            return false;
        if (bonus != ((AdjEffPlus)other).bonus)
            return false;
        return true;
    }
}

