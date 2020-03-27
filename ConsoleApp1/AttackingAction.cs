using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class AttackingAction : Action
    {
   public Card target;
    public AttackingAction(Card target)
    {
        this.target = target;
    }
    public string getName()
    {
        return "AttackingAction";
    }
}

