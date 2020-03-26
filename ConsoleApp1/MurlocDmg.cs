using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class MurlocDmg : Effect


    {
    int bonus;
    public MurlocDmg(int bonus) : base()
    {
        this.bonus = bonus;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: Murloc damage bonus " + user);
        user.tempAttackBonus -= bonus;
        foreach (Card c in board.p1Board)
        {
            if (c.typeMatches(Card.Type.Murloc))
                user.tempAttackBonus += bonus;
        }
        foreach (Card c in board.p2Board)
        {
            if (c.typeMatches(Card.Type.Murloc))
                user.tempAttackBonus += bonus;
        }
    }

    public override Effect makeGolden()
    {
        return new MurlocDmg(bonus * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is GetDamageAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is MurlocDmg))
            return false;
        if (bonus != ((MurlocDmg)other).bonus)
            return false;
        return true;
    }
}

