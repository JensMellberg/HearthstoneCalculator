using System;
using System.Collections.Generic;

public class HearthstoneBoard
{
    public enum OutputPriority
    {
        NONE = 0,
        DAMAGES = 1,
        ATTACKERS = 2,
        BOARDCHANGES = 3,
        EFFECTTRIGGERS = 4,
        COMMUNICATION = 5,
        INTENSEDEBUG = 6,
        ALL = 7

    }

    public bool turnbyturn = false;
    public BoardSide p1Board = new BoardSide();
    public BoardSide p2Board = new BoardSide();
    public OutputPriority printPriority = OutputPriority.NONE;
    public List<Card> pendingDeaths = new List<Card>();
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
    public void addNewMinionToBoard(BoardSide current, Card c, int position)
    {
        c.justCreated = true;
        printDebugMessage("Minion added to board " + (current == p1Board ? 1 : 2) + ": " + c.ID, OutputPriority.BOARDCHANGES);
      
        printDebugMessage("CURRENT IS NOW :" + current.Count, OutputPriority.COMMUNICATION);
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
    public void addNewMinionToBoard(int player, Card c, int position)
    {
        addNewMinionToBoard(getBoardFromPlayer(player), c, position);
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

    public void printDebugMessage(string msg, OutputPriority prio)
    {
        if (printPriority >= prio)
            Console.WriteLine(msg);
    }

    public int changePlayer(int player)
    {
        return player == 1 ? 2 : 1;
    }

    public void addToPendingDeath(Card c)
    {
        pendingDeaths.Add(c);
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

    public void deathCheck()
    {
        if (pendingDeaths.Count == 0)
            return;
        pendingDeaths[0].deathCheck(this);
        pendingDeaths.RemoveAt(0);
           
        deathCheck();
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
    public HearthstoneBoard turnByTurnSimulation()
    {
        turnbyturn = true;
        printPriority = OutputPriority.NONE;
        return simulateResult();
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
            attacker = rnd.Next(1, 3);
        }
        newBoard.doStartOfTurnEffects(attacker);
        newBoard.setAttackPriorities(1);
        newBoard.setAttackPriorities(2);
       
        while (newBoard.doTurnIfNotOver(attacker))
            attacker = changePlayer(attacker);

        
        return newBoard;
    }

    public void doStartOfTurnEffects(int starter)
    {
        printDebugMessage("Doing start of turn effects", OutputPriority.INTENSEDEBUG);
        BoardSide start = getBoardFromPlayer(starter);
        BoardSide other = starter == 1 ? p2Board : p1Board;
        bool flag = true;
        while (flag)
        {
            flag = false;
            flag = start.doStartOfTurnEffect(this) || flag;
            flag = other.doStartOfTurnEffect(this) || flag;
        }

    }

   
    //Positive if player 1 win
    public int getFinalizedDamage()
    {
        int win = getWinner();
        BoardSide current = win == 1 ? p1Board : p2Board;
        int multiplier = win == 1 ? 1 : -1;
        if (win == 0)
            return 0;
        int total = 0;
        foreach (Card c in current)
            total += c.tavernTier;
        return (total + current.tavernTier) * multiplier;
        
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
      
        BoardSide current = getBoardFromPlayer(player);
        printDebugMessage("Setting attack priorities for player " + player + " count: " + current.Count, OutputPriority.COMMUNICATION);
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

    public bool doTurnIfNotOver(int player)
    {
        if (p1Board.Count == 0 || p2Board.Count == 0)
        {
            printDebugMessage("Determined that it is game over, one side is empty", OutputPriority.INTENSEDEBUG);
            return false;
        }
        else
        {
            printDebugMessage("Determining that its not game over: " + p1Board.Count + " " + p2Board.Count, OutputPriority.INTENSEDEBUG);
            doTurn(player);
            return true;
        }

    }
    public void doTurn(int player)
    {
        printDebugMessage("Player " + player + " attacking.",OutputPriority.ATTACKERS);
        BoardSide current = getBoardFromPlayer(player);
        BoardSide other = player == 1 ? p2Board : p1Board;

        Card attacker = getHighestPriorityCard(current);
        if (attacker.attackPriority == Card.MAX_PRIORITY)
        {
            setAttackPriorities(player);
            attacker = getHighestPriorityCard(current);
        }

        Card target;
        
        List<Card> taunts = other.getTaunts();
        if (taunts.Count == 0)
        {
            int res = rnd.Next(0, other.Count);
            if (other.Count== 2)
            {
                counts[res]++;
            }
            target = other[res];
        }
        else
            target = taunts[rnd.Next(0, taunts.Count)];
        attacker.performAttack(target, this);
    }
    public static int[] counts = new int[2];

    public bool containsCard(Card c)
    {
        return p2Board.Contains(c) || p1Board.Contains(c);
            
    }

    public void killOf(Card c)
    {
        printDebugMessage("Card is killed of: " +c.getReadableName(), OutputPriority.DAMAGES);
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
        current.graveyard.Add(new BoardSide.DeadCard(c.cardID, c.golden, c.getCardType()));

        printDebugMessage("Card killed: count is now: " + current.Count, OutputPriority.INTENSEDEBUG);
        printDebugMessage("Card killed: count is now on p2: " + p2Board.Count, OutputPriority.INTENSEDEBUG);
    }

    public void printState()
    {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================");
        Console.ForegroundColor = color;
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
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("====================");
        Console.ForegroundColor = color;

    }

    public bool compare(HearthstoneBoard board)
    {
        if (p1Board.Compare(board.p1Board, this, board) && p2Board.Compare(board.p2Board, this,  board))
            return true;
        return false;
    }




    public HearthstoneBoard copy() {
        HearthstoneBoard board = new HearthstoneBoard();
        board.printPriority = printPriority;
        board.turnbyturn = turnbyturn;
        board.p1Board = p1Board.copy();
        board.p2Board = p2Board.copy();
        return board;
    }
}
