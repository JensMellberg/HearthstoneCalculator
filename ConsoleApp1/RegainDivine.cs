using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class RegainDivine : Effect

    {
    int dmg;
    public RegainDivine(int dmg) : base()
    {
        this.dmg = dmg;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: regain divine shield: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.setDivineShield(true);
        user.addAttack(dmg);
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is CardSpawnedAction)
            return ((CardSpawnedAction)a).spawnedCard().getCardType() == Card.Type.Mech;
        return false;
    }


    public override bool Compare(Effect other)
    {
        if (!(other is RegainDivine))
            return false;
        if (dmg != ((RegainDivine)other).dmg)
            return false;
        return true;
    }
    public override Effect makeGolden()
    {
        return new RegainDivine(dmg*2);
    }


}

