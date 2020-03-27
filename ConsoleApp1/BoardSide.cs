using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class BoardSide : List<Card>
    {
    public int tavernTier;
    public List<DeadCard> graveyard = new List<DeadCard>();

    public BoardSide(List<DeadCard> graveyard, int tier)
    {
        this.graveyard = graveyard;
        tavernTier = tier;
    }
        public BoardSide(int tier)
    {
        tavernTier = tier;
    }

    public BoardSide()
    {
    }

    public BoardSide copy()
    {
        List<DeadCard> newGy = new List<DeadCard>();
        foreach (DeadCard e in graveyard)
            newGy.Add(e);
        BoardSide result = new BoardSide(newGy,tavernTier);
        foreach (Card c in this)
            result.Add(c.copy());
        return result;
    }

    public List<Card> getTaunts()
    {
        List<Card> res = new List<Card>();
        foreach (Card c in this)
        {
            if (c.isTaunt())
                res.Add(c);
        }
        return res;
    }

    public Card getRandomCardAlive()
    {
        bool flag = false;
        foreach (Card d in this)
            if (d.isAlive())
            {
                flag = true;
                break;
            }
        if (!flag)
            return null;
        Card c = getRandomCard();
        while (!c.isAlive())
            c = getRandomCard();
        return c;
    }
    public Card getRandomCard()
    {
        return this[HearthstoneBoard.getRandomNumber(0, Count)];
    }

    public bool Compare(BoardSide other, HearthstoneBoard board, HearthstoneBoard otherBoard)
    {
        if (Count != other.Count)
            return false;
        for (int i = 0; i < Count; i++)
        {
            if (!this[i].Compare(other[i], board, otherBoard))
                return false;
        }

        return true;
    }

    public bool doStartOfTurnEffect(HearthstoneBoard board)
    {
        board.printDebugMessage("Doing start of turn effect on board with count "+Count, HearthstoneBoard.OutputPriority.INTENSEDEBUG);
        if (Count == 0)
            return false;
        int c = 0;

        while (c < Count)
        {
            if (this[c].attackPriority != Card.MAX_PRIORITY)
            {
                this[c].attackPriority = Card.MAX_PRIORITY;
                if (this[c].hasStartofTurnEffect())
                {
                    this[c].performedAction(new StartofCombatAction(), board);
                    board.deathCheck();
                    return true;
                }
            }
            c++;
        }
        return false;

    }

    public struct DeadCard
    {
        CardCreatorFactory.Cards card;
        bool gold;
        public Card.Type type;
        public DeadCard(CardCreatorFactory.Cards card, bool gold, Card.Type type)
        {
            this.type = type;
            this.card = card;
            this.gold = gold;
        }
        public bool Compare(DeadCard other)
        {
            if (type != other.type)
                return false;
            if (card != other.card)
                return false;
            if (gold != other.gold)
                return false;
            return true;
        }
        public Card revive()
        {
            if (gold)
                return CardCreatorFactory.createGoldenFromName(card);
            else
                return CardCreatorFactory.createFromName(card);
        }
    }
  
    }
