using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SummonImpMama : Effect

    {
    int times;
    public SummonImpMama(int times) : base()
    {
        this.times = times;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: summon demon on damage taken: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < times; i++)
        board.addNewMinionToBoard(board.getPlayerFromMinion(user), getRandomCard(board).setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i,0);
    }
    public Card getRandomCard(HearthstoneBoard board)
    {
        return CardCreatorFactory.createFromName(pool[board.getRandomNumber(0, pool.Length)]);
    }

    public override Effect makeGolden()
    {
        return new SummonImpMama(times*2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DamageTakenAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is SummonImpMama))
            return false;
        if (times != ((SummonImpMama)other).times)
            return false;
        return true;
    }
    string[] pool = {CardCreatorFactory.Cards.Voidlord, CardCreatorFactory.Cards.VulgarHomunculus, CardCreatorFactory.Cards.FiendishServant, CardCreatorFactory.Cards.AnnihilanBattlemaster
    , CardCreatorFactory.Cards.MalGanis,CardCreatorFactory.Cards.Siegebreaker,CardCreatorFactory.Cards.FloatingWatcher,CardCreatorFactory.Cards.ImpGangBoss,CardCreatorFactory.Cards.NathrezimOverseer
    ,CardCreatorFactory.Cards.Imprisoner};


}

