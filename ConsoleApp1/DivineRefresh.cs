using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DivineRefresh : Effect

    {
    public DivineRefresh()
    {
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: refresh divine shield: " + user.getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        user.divineShield = true;
    }
    public override Effect makeGolden()
    {
        return this;
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DivineShieldLossAction)
                return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        return other is DivineRefresh;
    }
}

