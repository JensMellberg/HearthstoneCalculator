using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

   public class GhastCoilerSummon : Effect

    {
    int times;
    public GhastCoilerSummon(int times) : base()
    {
        this.times =  times;
    }
    public override void doAction(Action cause, Card user, HearthstoneBoard board, List<Card> alwaysUse)
    {
        board.printDebugMessage("Performing action: random dr summon: " + user, HearthstoneBoard.OutputPriority.EFFECTTRIGGERS);
        for (int i = 0; i < times; i++)
            board.addNewMinionToBoard(board.getPlayerFromMinion(user), getRandomCard().setAttackPriority(user.attackPriority), board.getPositionFromMinion(user)+i,1);
    }
    public Card getRandomCard()
    {
        return CardCreatorFactory.createFromName(pool[HearthstoneBoard.getRandomNumber(0, pool.Length)]);
    }

    public override Effect makeGolden()
    {
        return new GhastCoilerSummon(times * 2);
    }

    public override bool triggerFromAction(Action a)
    {
        if (a is DeadAction)
            return true;
        return false;
    }
    public override bool Compare(Effect other)
    {
        if (!(other is GhastCoilerSummon))
            return false;
        if (times != ((GhastCoilerSummon)other).times)
            return false;
        return true;
    }
    string[] pool = {CardCreatorFactory.Cards.Goldrinn, CardCreatorFactory.Cards.FiendishServant, CardCreatorFactory.Cards.Imprisoner, CardCreatorFactory.Cards.KindlyGrandmother
    , CardCreatorFactory.Cards.UnstableGhoul,CardCreatorFactory.Cards.InfestedWolf,CardCreatorFactory.Cards.TheBeast,CardCreatorFactory.Cards.PilotedShredder,CardCreatorFactory.Cards.NadinaTheRed
    ,CardCreatorFactory.Cards.ReplicatingMenace,CardCreatorFactory.Cards.MechanoEgg,CardCreatorFactory.Cards.SavannahHighmane, CardCreatorFactory.Cards.Voidlord,CardCreatorFactory.Cards.SneedsOldShredder
    , CardCreatorFactory.Cards.SelflessHero, CardCreatorFactory.Cards.Mecharoo, CardCreatorFactory.Cards.SpawnOfNzoth, CardCreatorFactory.Cards.RatPack, CardCreatorFactory.Cards.HarvestToken
    ,CardCreatorFactory.Cards.KaboomBot, CardCreatorFactory.Cards.KingBagurgle, CardCreatorFactory.Cards.KangorsApprentice};


}

