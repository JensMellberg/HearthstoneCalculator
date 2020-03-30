using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SpawnBuffEffect : Effect

    {
    Card.Type type;
    int hp, dmg;
    public SpawnBuffEffect(Card.Type type, int dmg, int hp) : base()
    {
        this.dmg = dmg;
        this.hp = hp;
        this.type = type;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: buff spawned minion: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        ((CardSpawnedAction)cause).spawnedCard().addAttack(dmg);
        ((CardSpawnedAction)cause).spawnedCard().addHp(hp);
    }
    public override bool triggerFromAction(Action a)
    {
        if (a is CardSpawnedAction)
            return ((CardSpawnedAction)a).spawnedCard().getCardType() == type;
        return false;
    }

    public override Effect makeGolden()
    {
        return new SpawnBuffEffect(type, dmg*2,hp*2);
    }

    public override bool Compare(Effect other)
    {
        if (!(other is SpawnBuffEffect))
            return false;
        if (type != ((SpawnBuffEffect)other).type)
            return false;
        if (dmg != ((SpawnBuffEffect)other).dmg)
            return false;
        if (hp != ((SpawnBuffEffect)other).hp)
            return false;
        return true;
    }


}

