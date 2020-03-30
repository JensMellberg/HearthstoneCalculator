using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 public class GotKillAction : Action
{
    Card c;
    public GotKillAction(Card c)
    {
        this.c = c;
    }
    public Card Card()
    {
        return c;
    }
    public string getName()
    {
        return "GotKillAction";
    }
}

