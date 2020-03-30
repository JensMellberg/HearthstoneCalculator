using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class FriendlyDmbBonus : Effect

    {
    Card.Type type;
    int dmg;
    public FriendlyDmbBonus(Card.Type type, int dmg) : base()
    {
        this.dmg = dmg;
        this.type = type;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: buff friendly minion dmg: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        ((CardLookingForAtkBonusAction)cause).card().tempAttackBonus += dmg;
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is CardLookingForAtkBonusAction)
            return ((CardLookingForAtkBonusAction)a).card().getCardType() == type;
        return false;
    }

    public override void makeUpForReaderError(Card user, HearthstoneBoard board)
    {
        foreach (Card c in board.getBoardFromMinion(user))
            if (c.typeMatches(type) && c!=user)
                c.addAttack(-dmg);
    }

    public override Effect makeGolden()
    {
        return new FriendlyDmbBonus(type, dmg*2);
    }

    public override bool Compare(Effect other)
    {
        if (!(other is FriendlyDmbBonus))
            return false;
        if (type != ((FriendlyDmbBonus)other).type)
            return false;
        if (dmg != ((FriendlyDmbBonus)other).dmg)
            return false;
        return true;
    }


}

