using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 public class CardLookingForAtkBonusAction : Action
{
    Card c;
    public CardLookingForAtkBonusAction(Card c)
    {
        this.c = c;
    }
    public Card card()
    {
        return c;
    }
    public string getName()
    {
        return "CardLookingForAtkBonusAction";
    }
}

