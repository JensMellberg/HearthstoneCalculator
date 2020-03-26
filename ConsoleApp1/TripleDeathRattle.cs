using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TripleDeathRattle : Effect
{
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        ((CardKilledAction)cause).killedCard().performedAction(new DeadAction(), board);
        ((CardKilledAction)cause).killedCard().performedAction(new DeadAction(), board);

    }
    public override bool triggerFromAction(Action a)
    {
        if (a is CardKilledAction)
            return true;
        return false;
    }

    public override bool Compare(Effect other)
    {
        return other is DoubleDeathRattle;
    }
}
