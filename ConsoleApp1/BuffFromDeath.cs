using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class BuffFromDeath : Effect

    {
    int dmg;
    int hp;
    Card.Type type;
    public BuffFromDeath(int dmg, int hp, Card.Type type)
    {
        this.dmg = dmg;
        this.hp = hp;
        this.type = type;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: buff from friendly death: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.addAttack(dmg);
        user.addHp(hp);
    }
    public override Effect makeGolden()
    {
        return new BuffFromDeath(dmg*2,hp*2,type);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is CardKilledAction)
            if (((CardKilledAction)a).killedCard().typeMatches(type))
                return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        if (!(other is BuffFromDeath))
            return false;
        if (dmg != ((BuffFromDeath)other).dmg)
            return false;
        if (type != ((BuffFromDeath)other).type)
            return false;
        if (hp != ((BuffFromDeath)other).hp)
            return false;
        return true;
    }
}

