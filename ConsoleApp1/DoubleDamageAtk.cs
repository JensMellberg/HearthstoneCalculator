using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DoubleDmanageAtk : Effect

    {
    int multiplier;
    public DoubleDmanageAtk(int multiplier) : base()
    {
        this.multiplier = multiplier;
    }
    public override Effect makeGolden()
    {
        return new DoubleDmanageAtk(3);
    }

    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: double damage attack: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.setAtk(user.getAttack(board) * multiplier);
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is AttackingAction)
            return true;
        return false;
    }


    public override bool Compare(Effect other)
    {
        if (!(other is DoubleDmanageAtk))
            return false;
        if (multiplier != ((DoubleDmanageAtk)other).multiplier)
            return false;
        return true;
    }


}

