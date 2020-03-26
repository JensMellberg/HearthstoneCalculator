using System;
using System.Collections.Generic;

public abstract class Card
{
    int attack;
    int hp;
    List<Effect> effects;
    bool divineShield, taunt;

    public Card(int attack, int hp, List<Effect> effects, bool divineShield, bool taunt)
	{
        this.attack = attack;
        this.hp = hp;
        this.effects = effects;
        this.divineShield = divineShield;
        this.taunt = taunt;
	}

    public void performAction(Action a)
    {

    }

}
