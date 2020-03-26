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

    public Card(int ID, string name, int attack, int hp, List<Effect> effects, bool divineShield, bool taunt, Type type, CardCreatorFactory.Cards cardID)
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
	}

    public bool typeMatches(Type t)
    {
        if (type == Type.All)
            return true;
        if (t == type)
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
        Console.WriteLine("damagebonus: " + tempAttackBonus+ "on "+getReadableName());
        tempAttackBonus = 0;

        return result;

    }

    public void performedAction(Action a, HearthstoneBoard hearthstoneBoard)
    {
        Console.WriteLine("Received action " + a.getName() + " on "+this.getReadableName());
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

    public void performAttack(Card target, HearthstoneBoard board)
    {
        Console.WriteLine("attacker: " + this.name + " defender: " + target.name);
        int returnAttack = target.getAttack(board);
        target.dealDamage(getAttack(board), board);
        this.dealDamage(returnAttack, board);

        attackPriority = Card.MAX_PRIORITY;
        Console.WriteLine("attack perform finished from " + this.getReadableName() + " to " + target.getReadableName());
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


    public void dealDamage(int damage, HearthstoneBoard board)
    {
        if (divineShield)
        {
            divineShield = false;
            return;
        }
        hp = hp - damage;
        if (hp <= 0)
        {
            this.performedAction(new DeadAction(), board);
            board.killOf(this);

        }
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
        return new Card(ID,name, attack, hp, newEffs, divineShield, taunt, type,cardID);
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
