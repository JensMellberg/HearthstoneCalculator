using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public abstract class Effect
{



    


       public void performedAction(Action a, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
       // board.printDebugMessage("Determining if action triggers effect (" +a.getName()+") on card: "+user.getReadableName(), HearthstoneBoard.OutputPriority.ALL);
        if (triggerFromAction(a))
            doAction(a,user, board, alwaysUse);

    }
    public virtual void makeUpForReaderError(Card user, HearthstoneBoard board)
    {

    }

    public abstract void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse);

    public virtual bool triggerFromAction(Action a)
    {
        return false;
    }


    public abstract bool Compare(Effect other);

    public abstract Effect makeGolden();
        
}
