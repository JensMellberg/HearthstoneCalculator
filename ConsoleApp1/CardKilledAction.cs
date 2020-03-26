using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 public class CardKilledAction : Action
{
    Card c;
    public CardKilledAction(Card c)
    {
        this.c = c;
    }
    public Card killedCard()
    {
        return c;
    }
    public string getName()
    {
        return "CardKilledAction";
    }
}

