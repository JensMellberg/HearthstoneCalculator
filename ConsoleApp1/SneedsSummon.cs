using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class SneedsSummon : Effect

    {
    int times;
    public SneedsSummon(int times) : base()
    {
        this.times =  times;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: random sneeds summon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < times; i++)
            board.addNewMinionToBoard(board.getPlayerFromMinion(user), getRandomCard().setAttackPriority(user.attackPriority), board.getPositionFromMinion(user));
    }
    public Card getRandomCard()
    {
        return CardCreatorFactory.createFromName(pool[HearthstoneBoard.getRandomNumber(0, pool.Length)]);
    }

    public override Effect makeGolden()
    {
        return new ShredderSummon(times * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is SneedsSummon))
            return false;
        if (times != ((SneedsSummon)other).times)
            return false;
        return true;
    }
    CardCreatorFactory.Cards[] pool = {CardCreatorFactory.Cards.OldMurkeye, CardCreatorFactory.Cards.WaxriderTogwaggle, CardCreatorFactory.Cards.Khadgar, CardCreatorFactory.Cards.ShifterZerus
    , CardCreatorFactory.Cards.TheBeast,CardCreatorFactory.Cards.BolvarFireblood,CardCreatorFactory.Cards.BaronRivendare,CardCreatorFactory.Cards.BrannBronzebeard,CardCreatorFactory.Cards.Goldrinn
    ,CardCreatorFactory.Cards.KingBagurgle,CardCreatorFactory.Cards.Murozond,CardCreatorFactory.Cards.MalGanis,CardCreatorFactory.Cards.Razorgore,CardCreatorFactory.Cards.FoeReaper,
    CardCreatorFactory.Cards.Kalecgos,CardCreatorFactory.Cards.Maexxna,CardCreatorFactory.Cards.NadinaTheRed,CardCreatorFactory.Cards.ZappSlywick};


}

