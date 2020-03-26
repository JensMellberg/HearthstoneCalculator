using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


 public class CardSpawnedAction : Action
{
    Card c;
    public CardSpawnedAction(Card c)
    {
        this.c = c;
    }
    public Card spawnedCard()
    {
        return c;
    }
    public string getName()
    {
        return "CardSpawnedAction";
    }
}

