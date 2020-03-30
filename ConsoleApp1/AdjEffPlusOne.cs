using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
   public class AdjEffPlus : Effect


    {
    int bonus;
    public AdjEffPlus(int bonus) : base()
    {
        this.bonus = bonus;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: Adjacent + 1 from: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        foreach (Card c in alwaysUse)
        {
            c.tempAttackBonus += 1;
        }
    }

    public override Effect makeGolden()
    {
        return new AdjEffPlus(bonus * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is AdjacentAction)
            return true;
        return false;
    }

    public override void makeUpForReaderError(Card user, HearthstoneBoard board)
    {
        foreach (Card c in board.getAdjacents(user))
            c.addAttack(-1);
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

