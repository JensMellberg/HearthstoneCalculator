using System;
using System.Collections.Generic;

public class HearthstoneBoard
{
    public BoardSide p1Board = new BoardSide();
    public BoardSide p2Board = new BoardSide();
    public HearthstoneBoard()
    {

    }

    static Random rnd = new Random();
    public static int getRandomNumber(int start, int end)
    {
        return rnd.Next(start, end);
    }

    public int getPlayerFromMinion(Card c) {
        if (p1Board.Contains(c))
            return 1;
        else if (p2Board.Contains(c))
            return 2;
        else
            throw new CardDoesNotExistException("getPlayerFromMinion failed: Card does not exist: "+c.ID);
    }

    public void addNewMinionToBoard(int player, Card c, int position)
    {
        c.justCreated = true;
        Console.WriteLine("Minion added to board " + player + ": " + c.ID);
        BoardSide current = getBoardFromPlayer(player);
        Console.WriteLine("CURRENT IS NOW :" + current.Count);
        if (current.Count == 8)
            return;
        else
        {
            if (current.Count == position + 1)
                current.Add(c);
            else
                current.Insert(position + 1, c);
            foreach (Card d in current)
            {
                d.performedAction(new CardSpawnedAction(c), this);
            }

        }
    }
    public BoardSide getBoardFromMinion(Card c)
    {
       return  getBoardFromPlayer(getPlayerFromMinion(c));
    }
    public BoardSide getOpponentBoardFromMinion(Card c)
    {
        return getBoardFromPlayer(changePlayer(getPlayerFromMinion(c)));
    }

    public BoardSide getBoardFromPlayer(int player)
    {
      return player == 1 ? p1Board : p2Board;
    }



    public int changePlayer(int player)
    {
        return player == 1 ? 2 : 1;
    }

    public int getPositionFromMinion(Card c)
    {
        if (p1Board.Contains(c))
            return p1Board.IndexOf(c);
        else if (p2Board.Contains(c))
            return p2Board.IndexOf(c);
        else
            throw new CardDoesNotExistException("getPositionFromMinion failed: card does not exist: " + c.ID);
    }



    public List<Card> getAdjacents(Card c)
    {
        List<Card> current = null;

        if (p1Board.Contains(c))
            current = p1Board;
        else if (p2Board.Contains(c))
            current = p2Board;
        else
            throw new CardDoesNotExistException("getAdjacents failed: card does not exist " + c.getReadableName());
        if (current.Count == 1)
            return new List<Card>();
        if (current.IndexOf(c) == 0 && current.Count > 1)
            return new List<Card> { current[1] };
        else if (current.IndexOf(c) == current.Count - 1)
        {

            return new List<Card> { current[current.IndexOf(c) - 1] };
        }
        else return new List<Card> { current[current.IndexOf(c) - 1], current[current.IndexOf(c) + 1] };
    }

    public List<HearthstoneBoard> simulateResults(int iterations)
    {
        List<HearthstoneBoard> results = new List<HearthstoneBoard>();
        for (int i = 0; i < iterations; i++)
        {
            results.Add(simulateResult());
        }
        return results;
    }
    public HearthstoneBoard simulateResult()
    {
        HearthstoneBoard newBoard = this.copy();
        int attacker = 0;
        if (p1Board.Count > p2Board.Count)
            attacker = 1;
        else if (p2Board.Count > p1Board.Count)
            attacker = 2;
        else
        {
            Random rnd = new Random();
            attacker = rnd.Next(1, 3);
        }
        setAttackPriorities(1);
        setAttackPriorities(2);
        while (newBoard.doTurnIfNotOver(attacker))
            attacker = changePlayer(attacker);

        
        return newBoard;

    }

    public bool doTurnIfNotOver(int player)
    {
        if (p1Board.Count == 0 || p2Board.Count == 0)
            return false;
        else
        {
            doTurn(player);
            return true;
        }

    }

    public int getWinner()
    {
        if (p1Board.Count == 0 && p2Board.Count == 0)
            return 0;
        if (p1Board.Count == 0)
            return 2;
        return 1;

    }

    public void setAttackPriorities(int player)
    {
        Console.WriteLine("Setting attack priorities");
        BoardSide current = getBoardFromPlayer(player);
        for (int i = 0; i < current.Count; i++)
            current[i].attackPriority = i;

    }

    public Card getHighestPriorityCard(BoardSide b)
    {
        Card c = b[0];
        foreach (Card d in b)
            if (d.attackPriority < c.attackPriority)
                c = d;
        return c;
    }

    public void doTurn(int player)
    {
        Console.WriteLine("Player " + player + " attacking.");
        BoardSide current = getBoardFromPlayer(player);
        BoardSide other = player == 1 ? p2Board : p1Board;

        Card attacker = getHighestPriorityCard(current);
        if (attacker.attackPriority == Card.MAX_PRIORITY)
        {
            setAttackPriorities(player);
            attacker = getHighestPriorityCard(current);
        }

        Card target;
        Random rnd = new Random();
        List<Card> taunts = other.getTaunts();
        if (taunts.Count == 0)
            target = other[rnd.Next(0, other.Count)];
        else
            target = taunts[rnd.Next(0, taunts.Count)];
        attacker.performAttack(target, this);
    }

    public void killOf(Card c)
    {
        Console.WriteLine("Card is killed of: "+c.getReadableName());
        BoardSide current = p1Board.Contains(c) ? p1Board : p2Board;
        if (!p2Board.Contains(c) && !p1Board.Contains(c))
            throw new CardDoesNotExistException("killOf failed: card does not exist: "+c.ID);
        Card lastCard = null;
        for (int i = 0; i < current.Count; i++)
        {
            if (lastCard == current[i])
                continue;
            lastCard = current[i];
            current[i].performedAction(new CardKilledAction(c), this);
           
        }
        current.Remove(c);
      
    }

    public void printState()
    {
        Console.WriteLine("Board 1:");
        foreach (Card c in p1Board)
        {
            c.printState();
        }
        Console.WriteLine("Board 2:");
        foreach (Card c in p2Board)
        {
            c.printState();
        }

    }

    public bool compare(HearthstoneBoard board)
    {
        if (p1Board.Compare(board.p1Board, this, board) && p2Board.Compare(board.p2Board, this,  board))
            return true;
        return false;
    }




    public HearthstoneBoard copy() {
        HearthstoneBoard board = new HearthstoneBoard();
        board.p1Board = p1Board.copy();
        board.p2Board = p2Board.copy();
        return board;
    }
}
