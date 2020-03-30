using System;
using System.Collections.Generic;
using System.Threading;
[Serializable]
public class Card
{
    public int attack;
    public int tempAttackBonus;
    public int hp;
    public List<Effect> effects;
    public bool divineShield, taunt, poisonous, windfury;
    public string name;
    public int ID;
    public bool justCreated;
    public Type type;
    public string cardID;
    public int attackPriority;
    public const int MAX_PRIORITY = 1000;
    public int tavernTier;
    public bool golden = false;
    public int BoardPosition = 0;
    public enum Type
    {
        All,
        Demon,
        Mech,
        Murloc,
        Beast,
        Dragon,
        None
    };
  
    public Card(int ID, string name, int attack, int hp, List<Effect> effects, bool poisonous, bool divineShield, bool taunt, Type type,int tier, string cardID)
	{
        this.ID = ID == 0 ? ID = HearthstoneBoard.getRandomNumberMain(1, 9999) : ID;
        this.cardID = cardID;
        this.name = name;
        this.attack = attack;
        this.hp = hp;
        this.effects = effects;
        if (effects == null)
            this.effects = new List<Effect>();
        this.divineShield = divineShield;
        this.taunt = taunt;
        this.type = type;
        this.poisonous = poisonous;
        this.tavernTier = tier;
	}

    public bool typeMatches(Type t)
    {
        return typesMatches(t, type);
    }

    public static bool typesMatches(Type t, Type t2)
    {
        if (t == t2)
            return true;
        if (t == Type.All || t2 == Type.All)
            return true;
        return false;
    }
    public Card removeReborn()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
            if (effects[i] is Reborn)
                effects.RemoveAt(i);
        return this;
    }

    public bool isTaunt()
    {
        return taunt;
    }
    public int getAttack(HearthstoneBoard board)
    {
        foreach (Card c in board.getAdjacents(this))
            c.performAdjacantEffects(this, board);
        foreach (Card c in board.getBoardFromMinion(this))
            if (c != this)
                c.performedAction(new CardLookingForAtkBonusAction(this),board);
        performedAction(new GetDamageAction(), board);
        int result = attack + tempAttackBonus;
        board.printDebugMessage("damagebonus: " + tempAttackBonus+ "on "+getReadableName(), HearthstoneBoard.OutputPriority.COMMUNICATION);
        tempAttackBonus = 0;

        return result;

    }

    public bool hasStartofTurnEffect()
    {
        foreach (Effect e in effects)
            if (e.triggerFromAction(new StartofCombatAction()))
                return true;
        return false;
    }

    public bool hasEffect(Effect other)
    {
        foreach (Effect e in effects)
            if (e.Compare(other))
                return true;
        return false;
    }

    public void performedAction(Action a, HearthstoneBoard hearthstoneBoard)
    {
        hearthstoneBoard.printDebugMessage("Received action " + a.getName() + " on "+this.getReadableName(), HearthstoneBoard.OutputPriority.COMMUNICATION);
        foreach (Effect e in effects)
        {
            e.performedAction(a,this,  hearthstoneBoard, null);
        }
    }

    public Type getCardType()
    {
        return type;
    }

    public string getName()
    {
        return name;
    }


    public string getReadableName()
    {
        return name + " (ID: " + ID + ")";
    }

    public void performAdjacantEffects(Card middle, HearthstoneBoard hearthstoneBoard)
    {
        foreach (Effect e in effects)
        {
            e.performedAction(new AdjacentAction(),this,  hearthstoneBoard, new List<Card> { middle } );
        }
    }

    public void causeDamageToTarget(Card target, HearthstoneBoard board, int damage)
    {
        if (poisonous)
            damage = 999999;
        int res = target.dealDamage(damage, board);
        if (res == 2)
            performedAction(new OverKillAction(), board);
        if (res > 0)
            foreach (Card c in board.getBoardFromMinion(this))
                c.performedAction(new GotKillAction(this), board);
    }

    public void deathCheck(HearthstoneBoard board)
    {
        if (!isAlive() && board.containsCard(this))
        {
            this.performedAction(new DeadAction(), board);
            board.killOf(this);
        }
    }

    public Card setId(int id)
    {
        this.ID = id;
        return this;
    }

    public bool isAlive()
    {
        return hp > 0;
    }


    public void performAttack(Card target, HearthstoneBoard board)
    {
        board.printDebugMessage("attacker: " + this.getReadableName() + " defender: " + target.getReadableName(), HearthstoneBoard.OutputPriority.ATTACKERS);
        if (board.turnbyturn)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Attacker: " + this.getReadableName());
            Console.WriteLine("Defender: " + target.getReadableName());
            Console.ReadLine();
        }
        if (board.stopFlag)
        {
            board.attacker = this;
            board.defender = target;
                board.finishedWorkFlag = true;
            while (board.stopFlag)
                Thread.Sleep(100);
            board.finishedWorkFlag = false;
            board.stopFlag = true;
        }
        this.performedAction(new AttackingAction(target),board);
        int returnAttack = target.poisonous ? 9999999 : target.getAttack(board);
        //target.dealDamage(getAttack(board), board);
        causeDamageToTarget(target, board, getAttack(board));
        if (this.dealDamage(returnAttack, board) > 0)
            foreach (Card c in board.getBoardFromMinion(this))
                c.performedAction(new GotKillAction(this), board);
        

        board.deathCheck();
        //target.deathCheck(board);
        //this.deathCheck(board);
       


        attackPriority = Card.MAX_PRIORITY;
        board.printDebugMessage("attack perform finished from " + this.getReadableName() + " to " + target.getReadableName(), HearthstoneBoard.OutputPriority.ATTACKERS);
        if (board.printPriority >= HearthstoneBoard.OutputPriority.ATTACKERS)
        board.printState();

        if (board.turnbyturn)
        {
            Console.WriteLine("--------------------------------------");
            board.printState();
            Console.ReadLine();
        }
    }
    public void addStats(int attack, int hp)
    {
        addAttack(attack);
        addHp(hp);
    }

    public void addAttack(int atk)
    {
        attack += atk;
    }

    public void addHp(int hp)
    {
        this.hp += hp;
    }

    public int getHp(HearthstoneBoard board)
    {
        return hp;
    }
    public Card setAttackPriority(int prio)
    {
        attackPriority = prio;
        return this;
    }
    public Card setStats(int attack, int hp)
    {
        this.attack = attack;
        this.hp = hp;
        return this;
    }
    public Card setHp(int hp)
    {
        this.hp = hp;
        return this;
    }

    public Card setAtk(int atk)
    {
        attack = atk;
        return this;
    }

    public Card setDivineShield(bool b)
    {
        this.divineShield = b;
        return this;
    }

    public Card setWindfury(bool b)
    {
        this.windfury = b;
        return this;
    }

    public Card setPoisonous(bool b)
    {
        this.poisonous = b;
        return this;
    }

    public Card setTaunt(bool b)
    {
        this.taunt = b;
        return this;
    }


    public Card addEffect(Effect e)
    {
        this.effects.Add(e);
        return this;
    }

    public Card makeGolden()
    {
        hp *= 2;
        attack *= 2;
        return makeGoldenEffects();

    }

    public Card makeGoldenEffects()
    {
        golden = true;
        for (int i = 0; i < effects.Count; i++)
            effects[i] = effects[i].makeGolden();
        return this;

    }

    //0 = not dead, 1 = dead, 2 = overkill
    public int dealDamage(int damage, HearthstoneBoard board)
    {
        if (divineShield)
        {
            foreach (Card c in board.getBoardFromMinion(this))
                c.performedAction(new DivineShieldLossAction(this), board);
            divineShield = false;
            board.printDebugMessage("Damage taken: 0 (divine shield pop) on " + getReadableName(), HearthstoneBoard.OutputPriority.DAMAGES);
            return 0;
        }
        hp = hp - damage;
        board.printDebugMessage("Damage taken: " + damage + " on " + getReadableName(),HearthstoneBoard.OutputPriority.DAMAGES);
        performedAction(new DamageTakenAction(),board);
        if (hp <= 0)
        {
            board.addToPendingDeath(this);
            if (hp < 0)
                return 2;
            else
                return 1;
        }
        return 0;
    }

    public void makeUpForReaderError(HearthstoneBoard board)
    {
        foreach (Effect e in effects)
            e.makeUpForReaderError(this, board);
    }

    public void printState()
    {
        string bonus = "";
        if (divineShield)
            bonus += " (divine shield)";
        if (golden)
            bonus += " (golden)";
        Console.WriteLine("ID: "+ID+" "+name + " Attack: " + attack+ " Hp: " + hp + bonus);
    }

    public virtual Card copy()
    {
        List<Effect> newEffs = new List<Effect>();
        foreach (Effect e in effects)
            newEffs.Add(e);
        Card c = new Card(ID, name, attack, hp, newEffs, poisonous, divineShield, taunt, type, tavernTier, cardID);
        c.golden = golden;
        c.windfury = windfury;
        c.attackPriority = attackPriority;
        return c;
    }


    public bool Compare(Card other, HearthstoneBoard board, HearthstoneBoard otherBoard)
    {
        if (getAttack(board) != other.getAttack(otherBoard))
            return false;
        else if (getHp(board) != other.getHp(otherBoard))
            return false;
        else if (divineShield != other.divineShield)
            return false;
        else if (effects.Count != other.effects.Count)
            return false;
        for (int i = 0; i<  effects.Count; i++)
        {
            if (!effects[i].Compare(other.effects[i]))
                return false;
        }
        return true;
    }
}
