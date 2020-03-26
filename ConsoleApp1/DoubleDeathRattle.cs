using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DoubleDeathRattle : Effect
{
    int times;
    public DoubleDeathRattle(int times)
    {
        this.times = times;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Double deathrattle on: " + ((CardKilledAction)cause).killedCard().getReadableName(), HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < times; i++)
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

    public override Effect makeGolden()
    {
        return new DoubleDeathRattle(times * 2);
    }
}
