using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Effect
{



    


       public void performedAction(Action a, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        Console.WriteLine("Determining if action triggers effect ("+a.getName()+") on card: "+user.getReadableName());
        if (triggerFromAction(a))
            doAction(a,user, board, alwaysUse);

    }

    public abstract void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse);

    public virtual bool triggerFromAction(Action a)
    {
        return false;
    }


    public abstract bool Compare(Effect other);
        
        
}
