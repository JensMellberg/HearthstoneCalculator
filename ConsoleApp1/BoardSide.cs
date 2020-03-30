using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable] 
public class BoardSide : List<Card>
    {
    public int tavernTier;
    [NonSerialized]
    public List<DeadCard> graveyard;

    public BoardSide(List<DeadCard> graveyard, int tier)
    {
        this.graveyard = graveyard;
        tavernTier = tier;
    }
        public BoardSide(int tier)
    {
        tavernTier = tier;
        graveyard = new List<DeadCard>();
    }

    public BoardSide()
    {
        graveyard = new List<DeadCard>();
    }

    public BoardSide copy()
    {
        List<DeadCard> newGy = new List<DeadCard>();
        if (graveyard != null)
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

    public Card getRandomCardAlive(HearthstoneBoard board)
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
        Card c = getRandomCard(board);
        while (!c.isAlive())
            c = getRandomCard(board);
        return c;
    }
    public Card getRandomCard(HearthstoneBoard board)
    {
        return this[board.getRandomNumber(0, Count)];
    }

    public bool hasAvailableAttackers(HearthstoneBoard board)
    {
        foreach (Card c in this)
            if (c.getAttack(board) > 0)
                return true;
        return false;
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

    public List<Card> getLowestAtks(HearthstoneBoard board)
    {
        List<Card> ret = new List<Card>();
        Card lowest = this[0];
        foreach (Card d in this)
            if (d.getAttack(board) < lowest.getAttack(board))
                lowest = d;
        foreach (Card d in this)
            if (d.getAttack(board) == lowest.getAttack(board))
                ret.Add(d);
        return ret;
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
        string card;
        bool gold;
        public Card.Type type;
        public DeadCard(string card, bool gold, Card.Type type)
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
