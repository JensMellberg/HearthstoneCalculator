using System;
using System.Collections.Generic;

public class Card
{
    int attack;
    public int tempAttackBonus;
    int hp;
    List<Effect> effects;
    public bool divineShield, taunt;
    string name;
    public int ID;
    public bool justCreated;
    Type type;
    public CardCreatorFactory.Cards cardID;
    public int attackPriority;
    public const int MAX_PRIORITY = 1000;
    public int tavernTier;
    bool poisonous;
    public bool golden = false;
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

    public Card(int ID, string name, int attack, int hp, List<Effect> effects, bool poisonous, bool divineShield, bool taunt, Type type,int tier, CardCreatorFactory.Cards cardID)
	{
        this.ID = ID == 0 ? ID = HearthstoneBoard.getRandomNumber(1, 9999) : ID;
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

    public bool isTaunt()
    {
        return taunt;
    }
    public int getAttack(HearthstoneBoard board)
    {
        foreach (Card c in board.getAdjacents(this))
            c.performAdjacantEffects(this, board);
        performedAction(new GetDamageAction(), board);
        int result = attack + tempAttackBonus;
        board.printDebugMessage("damagebonus: " + tempAttackBonus+ "on "+getReadableName(), HearthstoneBoard.OutputPriority.COMMUNICATION);
        tempAttackBonus = 0;

        return result;

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
        if (target.dealDamage(damage, board))
            performedAction(new OverKillAction(), board);
    }

    public void performAttack(Card target, HearthstoneBoard board)
    {
        board.printDebugMessage("attacker: " + this.name + " defender: " + target.name, HearthstoneBoard.OutputPriority.ATTACKERS);
        int returnAttack = target.poisonous ? 9999999 : target.getAttack(board);
        //target.dealDamage(getAttack(board), board);
        causeDamageToTarget(target, board, getAttack(board));
        this.dealDamage(returnAttack, board);

        attackPriority = Card.MAX_PRIORITY;
        board.printDebugMessage("attack perform finished from " + this.getReadableName() + " to " + target.getReadableName(), HearthstoneBoard.OutputPriority.ATTACKERS);
        if (board.printPriority >= HearthstoneBoard.OutputPriority.ATTACKERS)
        board.printState();
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

    public Card setDivineShield(bool b)
    {
        this.divineShield = b;
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
        golden = true;
        hp *= 2;
        attack *= 2;
        for (int i = 0; i < effects.Count; i++)
            effects[i] = effects[i].makeGolden();
        return this;

    }


    public bool dealDamage(int damage, HearthstoneBoard board)
    {
        if (divineShield)
        {
            divineShield = false;
            return false;
        }
        hp = hp - damage;
        if (hp <= 0)
        {
            this.performedAction(new DeadAction(), board);
            board.killOf(this);
            return true;
        }
        return false;
    }

    public void printState()
    {
        string bonus = "";
        if (divineShield)
            bonus += " (divine shield)";
        Console.WriteLine("ID: "+ID+" "+name + " Attack: " + attack+ " Hp: " + hp + bonus);
    }

    public Card copy()
    {
        List<Effect> newEffs = new List<Effect>();
        foreach (Effect e in effects)
            newEffs.Add(e);
        Card c = new Card(ID, name, attack, hp, newEffs, poisonous, divineShield, taunt, type, tavernTier, cardID);
        c.golden = golden;
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
