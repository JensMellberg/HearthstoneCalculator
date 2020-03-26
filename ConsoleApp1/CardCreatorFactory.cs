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
        RatGoldToken
    }
    public static Card createFromName(Cards name)
    {
        switch (name)
        {
            case Cards.RighteousProtector:
                return new Card(0, "Righteous Protector", 1, 1, null, true, true, Card.Type.None, name);
            case Cards.DireWolfAlpha:
                return new Card(0, "Dire Wolf Alpha", 2, 2, new List<Effect> { new AdjEffPlus(1) }, false, false, Card.Type.Beast, name);
            case Cards.VulgarHomunculus:
                return new Card(0, "Vulgar Homunculus", 2, 4, null, false, true, Card.Type.Demon, name);
            case Cards.Mecharoo:
                return new Card(0, "Mecharoo", 1, 1, new List<Effect> { new DeathRattleSummon(Cards.MecharooToken) }, false, false, Card.Type.Mech, name);
            case Cards.Alleycat:
                return new Card(0, "Alleycat", 1, 1, null, false, false, Card.Type.Beast, name);
            case Cards.OldMurkeye:
                return new Card(0, "Old Murk-eye", 2, 4, new List<Effect> { new MurlocDmg(1) }, false, false, Card.Type.Murloc, name);
            case Cards.ColdlightSeer:
                return new Card(0, "Coldlight Seer", 2, 3, null, false, false, Card.Type.Murloc, name);
            case Cards.SelflessHero:
                return new Card(0, "Selfless Hero", 2, 1, new List<Effect> { new DeathRattleDivine(1) }, false, false, Card.Type.None, name);
            case Cards.ShieldedMinibot:
                return new Card(0, "Shielded Minibot", 2, 2, null, true, false, Card.Type.Mech, name);
            case Cards.SpawnOfNzoth:
                return new Card(0, "Spawn of N'Zoth", 2, 2, new List<Effect> { new DeathBuff(1) }, false, false, Card.Type.None, name);
            case Cards.KaboomBot:
                return new Card(0, "Kaboom Bot", 2, 2, new List<Effect> { new DeathRattleBomb(1) }, false, false, Card.Type.Mech, name);
            case Cards.CobaltGuardian:
                return new Card(0, "Cobalt Guardian", 6, 3, new List<Effect> { new RegainDivine() }, false, false, Card.Type.Mech, name);
            case Cards.MecharooToken:
                return new Card(0, "Mecharoo-token", 1, 1, null, false, false, Card.Type.Mech, name);
            case Cards.GoldarooToken:
                return new Card(0, "Goldaroo-token", 2, 2, null, false, false, Card.Type.Mech, name);
            case Cards.BaronRivendare:
                return new Card(0, "Baron Rivendare", 1, 7, new List<Effect> { new DoubleDeathRattle() }, false, false, Card.Type.None, name);
            case Cards.BronzeWarden:
                return new Card(0, "Bronze Warden", 2, 1, new List<Effect> { new Reborn(false) }, true, false, Card.Type.Dragon, name);
            case Cards.Goldrinn:
                return new Card(0, "Goldrinn, the Great Wolf", 4, 4, new List<Effect> { new DeathTypeBuff(4, Card.Type.Beast) }, false, false, Card.Type.Beast, name);
            case Cards.KingBagurgle:
                return new Card(0, "King Bagurgle", 6, 3, new List<Effect> { new DeathTypeBuff(2, Card.Type.Murloc) }, false, false, Card.Type.Murloc, name);
            case Cards.PackLeader:
                return new Card(0, "Pack Leader", 3, 3, new List<Effect> { new SpawnBuffEffect( Card.Type.Beast,3,0) }, false, false, Card.Type.None, name);
            case Cards.MamaBear:
                return new Card(0, "Mama Bear", 5, 5, new List<Effect> { new SpawnBuffEffect(Card.Type.Beast, 5, 5) }, false, false, Card.Type.Beast, name);
            case Cards.RatPack:
                return new Card(0, "Rat Pack", 2, 2, new List<Effect> { new RatSummon(Cards.RatToken) }, false, false, Card.Type.Beast, name);
            case Cards.RatToken:
                return new Card(0, "Rat-Token", 1, 1, null, false, false, Card.Type.Beast, name);
            case Cards.RatGoldToken:
                return new Card(0, "Rat-GoldToken", 2, 2, null, false, false, Card.Type.Beast, name);
            default:
                throw new UnknownCardException("Uknown card: " + name);

        }
    }
    public static Card createGoldenFromName(Cards name)
    {
        switch (name)
        {
            case Cards.RighteousProtector:
                return new Card(0, "Righteous Protector", 2, 2, null, true, true, Card.Type.None, name);
            case Cards.DireWolfAlpha:
                return new Card(0, "Dire Wolf Alpha", 4, 4, new List<Effect> { new AdjEffPlus(2) }, false, false, Card.Type.Beast, name);
            case Cards.VulgarHomunculus:
                return new Card(0, "Vulgar Homunculus", 4, 8, null, false, true, Card.Type.Demon, name);
            case Cards.Mecharoo:
                return new Card(0, "Mecharoo", 2, 2, new List<Effect> { new DeathRattleSummon(Cards.GoldarooToken) }, false, false, Card.Type.Mech, name);
            case Cards.Alleycat:
                return new Card(0, "Alleycat", 2, 2, null, false, false, Card.Type.Beast, name);
            case Cards.OldMurkeye:
                return new Card(0, "Old Murk-eye", 4, 8, new List<Effect> { new MurlocDmg(2) }, false, false, Card.Type.Murloc, name);
            case Cards.ColdlightSeer:
                return new Card(0, "Coldlight Seer", 4, 6, null, false, false, Card.Type.Murloc, name);
            case Cards.SelflessHero:
                return new Card(0, "Selfless Hero", 4, 2, new List<Effect> { new DeathRattleDivine(2) }, false, false, Card.Type.None, name);
            case Cards.ShieldedMinibot:
                return new Card(0, "Shielded Minibot", 4, 4, null, true, false, Card.Type.Mech, name);
            case Cards.SpawnOfNzoth:
                return new Card(0, "Spawn of N'Zoth", 4, 4, new List<Effect> { new DeathBuff(2) }, false, false, Card.Type.None, name);
            case Cards.KaboomBot:
                return new Card(0, "Kaboom Bot", 4, 4, new List<Effect> { new DeathRattleBomb(2) }, false, false, Card.Type.Mech, name);
            case Cards.BaronRivendare:
                return new Card(0, "Baron Rivendare", 2, 14, new List<Effect> { new DoubleDeathRattle() }, false, false, Card.Type.None, name);
            case Cards.CobaltGuardian:
                return new Card(0, "Cobalt Guardian", 12, 6, new List<Effect> { new RegainDivine() }, false, false, Card.Type.Mech, name);
            case Cards.BronzeWarden:
                return new Card(0, "Bronze Warden", 4, 2, new List<Effect> { new Reborn(true) }, true, false, Card.Type.Dragon, name);
            case Cards.Goldrinn:
                return new Card(0, "Goldrinn, the Great Wolf", 8, 8, new List<Effect> { new DeathTypeBuff(8, Card.Type.Beast) }, false, false, Card.Type.Beast, name);
            case Cards.KingBagurgle:
                return new Card(0, "King Bagurgle", 12, 6, new List<Effect> { new DeathTypeBuff(4, Card.Type.Murloc) }, false, false, Card.Type.Murloc, name);
            case Cards.PackLeader:
                return new Card(0, "Pack Leader", 6, 6, new List<Effect> { new SpawnBuffEffect(Card.Type.Beast, 6, 0) }, false, false, Card.Type.None, name);
            case Cards.MamaBear:
                return new Card(0, "Mama Bear", 10, 10, new List<Effect> { new SpawnBuffEffect(Card.Type.Beast, 10, 10) }, false, false, Card.Type.Beast, name);
            case Cards.RatPack:
                return new Card(0, "Rat Pack", 4, 4, new List<Effect> { new RatSummon(Cards.RatGoldToken) }, false, false, Card.Type.Beast, name);
            default:
                throw new UnknownCardException("Uknown card: " + name);

        }
    }
}