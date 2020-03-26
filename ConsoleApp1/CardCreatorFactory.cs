using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class CardCreatorFactory
{
    public enum Cards
    {
        RighteousProtector,
        DireWolfAlpha,
        VulgarHomunculus,
        Mecharoo, 
        Alleycat,
        OldMurkeye,
        ColdlightSeer,
        SelflessHero,
        SpawnOfNzoth,
        ShieldedMinibot,
        CobaltGuardian,
        KaboomBot,
        MecharooToken,
        GoldarooToken,
        BaronRivendare,
        BronzeWarden,
        Goldrinn,
        KingBagurgle,
        PackLeader, 
        MamaBear,
        RatPack,
        RatToken,
        RatGoldToken,
        HarvestToken,
        HarvestGolem,
        IronhideDirehorn,
        IronhideToken,
        HeraldOfFlame,
        Maexxna,
        KangorsApprentice,
        IronSensei
    }
    public static Card createFromName(Cards name)
    {
        switch (name)
        {
            case Cards.RighteousProtector:
                return new Card(0, "Righteous Protector", 1, 1, null,false, true, true, Card.Type.None,1, name);
            case Cards.DireWolfAlpha:
                return new Card(0, "Dire Wolf Alpha", 2, 2, new List<Effect> { new AdjEffPlus(1) }, false, false, false, Card.Type.Beast,1, name);
            case Cards.VulgarHomunculus:
                return new Card(0, "Vulgar Homunculus", 2, 4, null, false, false, true, Card.Type.Demon,1, name);
            case Cards.Mecharoo:
                return new Card(0, "Mecharoo", 1, 1, new List<Effect> { new DeathRattleSummon(Cards.MecharooToken) }, false, false, false, Card.Type.Mech,1, name);
            case Cards.Alleycat:
                return new Card(0, "Alleycat", 1, 1, null, false, false, false, Card.Type.Beast,1, name);
            case Cards.OldMurkeye:
                return new Card(0, "Old Murk-eye", 2, 4, new List<Effect> { new MurlocDmg(1) }, false, false, false, Card.Type.Murloc,2, name);
            case Cards.ColdlightSeer:
                return new Card(0, "Coldlight Seer", 2, 3, null, false, false, false, Card.Type.Murloc,3, name);
            case Cards.SelflessHero:
                return new Card(0, "Selfless Hero", 2, 1, new List<Effect> { new DeathRattleDivine(1) }, false, false, false, Card.Type.None,1, name);
            case Cards.ShieldedMinibot:
                return new Card(0, "Shielded Minibot", 2, 2, null, false, true, false, Card.Type.Mech,2, name);
            case Cards.SpawnOfNzoth:
                return new Card(0, "Spawn of N'Zoth", 2, 2, new List<Effect> { new DeathBuff(1) }, false, false, false, Card.Type.None,2, name);
            case Cards.KaboomBot:
                return new Card(0, "Kaboom Bot", 2, 2, new List<Effect> { new DeathRattleBomb(1) }, false, false, false, Card.Type.Mech,2, name);
            case Cards.CobaltGuardian:
                return new Card(0, "Cobalt Guardian", 6, 3, new List<Effect> { new RegainDivine() }, false, false, false, Card.Type.Mech,3, name);
            case Cards.MecharooToken:
                return new Card(0, "Mecharoo-token", 1, 1, null, false, false, false, Card.Type.Mech,1, name);
            case Cards.BaronRivendare:
                return new Card(0, "Baron Rivendare", 1, 7, new List<Effect> { new DoubleDeathRattle(1) }, false, false, false, Card.Type.None,5, name);
            case Cards.BronzeWarden:
                return new Card(0, "Bronze Warden", 2, 1, new List<Effect> { new Reborn(false) }, false, true, false, Card.Type.Dragon,3, name);
            case Cards.Goldrinn:
                return new Card(0, "Goldrinn, the Great Wolf", 4, 4, new List<Effect> { new DeathTypeBuff(4, Card.Type.Beast) }, false, false, false, Card.Type.Beast,5, name);
            case Cards.KingBagurgle:
                return new Card(0, "King Bagurgle", 6, 3, new List<Effect> { new DeathTypeBuff(2, Card.Type.Murloc) }, false, false, false, Card.Type.Murloc,5, name);
            case Cards.PackLeader:
                return new Card(0, "Pack Leader", 3, 3, new List<Effect> { new SpawnBuffEffect( Card.Type.Beast,3,0) }, false, false, false, Card.Type.None,3, name);
            case Cards.MamaBear:
                return new Card(0, "Mama Bear", 5, 5, new List<Effect> { new SpawnBuffEffect(Card.Type.Beast, 5, 5) }, false, false, false, Card.Type.Beast,6, name);
            case Cards.RatPack:
                return new Card(0, "Rat Pack", 2, 2, new List<Effect> { new RatSummon(Cards.RatToken) }, false, false, false, Card.Type.Beast,2, name);
            case Cards.HarvestGolem:
                return new Card(0, "Harvest Golem", 2, 3, new List<Effect> { new DeathRattleSummon(Cards.HarvestToken) }, false, false, false, Card.Type.Mech, 2, name);
            case Cards.HarvestToken:
                return new Card(0, "Harvest-Token", 2, 1, null, false, false, false, Card.Type.Mech, 1, name);
            case Cards.RatToken:
                return new Card(0, "Rat-Token", 1, 1, null, false, false, false, Card.Type.Beast,1, name);
            case Cards.IronSensei:
                return new Card(0, "Iron Sensei", 2, 2, null, false, false, false, Card.Type.Mech, 4, name);
            case Cards.IronhideDirehorn:
                return new Card(0, "Ironhide Direhorn", 7, 7, new List<Effect> { new OverKillSpawn(Cards.IronhideToken) }, false, false, false, Card.Type.Beast, 5, name);
            case Cards.IronhideToken:
                return new Card(0, "Ironhide-Token", 5, 5, null, false, false, false, Card.Type.Beast, 1, name);
            case Cards.HeraldOfFlame:
                return new Card(0, "Herald of Flame", 5, 6, new List<Effect> { new OverKillDamage(3)}, false, false, false, Card.Type.Dragon, 4, name);
            case Cards.Maexxna:
                return new Card(0, "Maexxna", 2, 8, null, true, false, false, Card.Type.Beast, 6, name);
            case Cards.KangorsApprentice:
                return new Card(0, "Kangor's Apprentice", 3, 6, new List<Effect> { new KangorSummon(2)}, true, false, false, Card.Type.None, 6, name);

                
            default:
                throw new UnknownCardException("Uknown card: " + name);

        }
    }
    public static Card createGoldenFromName(Cards name)
    {
        return createFromName(name).makeGolden();
    }
}