using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class BuffFromKill : Effect

    {
    int dmg;
    int hp;
    Card.Type type;
    public BuffFromKill(int dmg, int hp, Card.Type type)
    {
        this.dmg = dmg;
        this.hp = hp;
        this.type = type;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: buff from friendly kill: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.addAttack(dmg);
        user.addHp(hp);
    }
    public override Effect makeGolden()
    {
        return new BuffFromKill(dmg*2,hp*2,type);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is GotKillAction)
            if (((GotKillAction)a).Card().typeMatches(type))
                return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is BuffFromKill))
            return false;
        if (dmg != ((BuffFromKill)other).dmg)
            return false;
        if (type != ((BuffFromKill)other).type)
            return false;
        if (hp != ((BuffFromKill)other).hp)
            return false;
        return true;
    }
}

