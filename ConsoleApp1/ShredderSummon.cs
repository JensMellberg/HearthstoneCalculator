using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class ShredderSummon : Effect

    {
    int times;
    public ShredderSummon(int times) : base()
    {
        this.times =  times;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: random shredder summon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
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
        if (!(other is ShredderSummon))
            return false;
        if (times != ((ShredderSummon)other).times)
            return false;
        return true;
    }
    CardCreatorFactory.Cards[] pool = {CardCreatorFactory.Cards.DireWolfAlpha, CardCreatorFactory.Cards.VulgarHomunculus, CardCreatorFactory.Cards.MicroMachine, CardCreatorFactory.Cards.MurlocTidehunter
    , CardCreatorFactory.Cards.RockpoolHunter,CardCreatorFactory.Cards.DragonspawnLieutenant,CardCreatorFactory.Cards.KindlyGrandmother,CardCreatorFactory.Cards.ScavengingHyena,CardCreatorFactory.Cards.UnstableGhoul
    ,CardCreatorFactory.Cards.Khadgar};


}

