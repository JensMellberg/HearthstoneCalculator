using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 public  class DivineShieldLossAction : Action
{
    Card c;
    public DivineShieldLossAction(Card user)
    {
        c = user;
    }
    public Card getCard()
    {
        return c;
    }
    public string getName()
    {
        return "DivineShieldLossAction";
    }

}

