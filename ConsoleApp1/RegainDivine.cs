using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class RegainDivine : Effect

    {
    public RegainDivine() : base()
    {
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: regain divine shield: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.setDivineShield(true);
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is CardSpawnedAction)
            return ((CardSpawnedAction)a).spawnedCard().getCardType() == Card.Type.Mech;
        return false;
    }


    public override bool Compare(Effect other)
    {
        return other is RegainDivine;
    }
    public override Effect makeGolden()
    {
        return this;
    }


}

