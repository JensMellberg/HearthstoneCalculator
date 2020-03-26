using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DoubleDeathRattle : Effect
{
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        Console.WriteLine("Killed card: " + ((CardKilledAction)cause).killedCard().getReadableName());
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
