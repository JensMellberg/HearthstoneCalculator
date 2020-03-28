using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class BuffFromDivine : Effect

    {
    int dmg;
    int hp;
    public BuffFromDivine(int dmg, int hp)
    {
        this.dmg = dmg;
        this.hp = hp;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: buff from divine shield pop: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.addAttack(dmg);
        user.addHp(hp);
    }
    public override Effect makeGolden()
    {
        return new BuffFromDivine(dmg*2,hp*2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DivineShieldLossAction)
                return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is BuffFromDivine))
            return false;
        if (dmg != ((BuffFromDivine)other).dmg)
            return false;
        if (hp != ((BuffFromDivine)other).hp)
            return false;
        return true;
    }
}

