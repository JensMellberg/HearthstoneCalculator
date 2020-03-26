using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class BoardSide : List<Card>
    {
        public BoardSide copy()
    {
        BoardSide result = new BoardSide();
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

    public bool Compare(BoardSide other, HearthstoneBoard board, HearthstoneBoard otherBoard)
    {
        if (Count != other.Count)
            return false;
        for (int i = 0; i < Count; i++)
        {
            if (!this[i].Compare(other[i],board, otherBoard))
                return false;
        }
        return true;
    }
  
    }
