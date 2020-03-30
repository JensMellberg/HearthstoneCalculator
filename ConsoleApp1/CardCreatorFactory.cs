using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class CardCreatorFactory
{
    public class Cards
    {
        public const string RighteousProtector = "Righteous Protector",
        DireWolfAlpha = "Dire Wolf Alpha",
        VulgarHomunculus = "Vulgar Homunculus",
        Mecharoo = "Mecharoo",
        Alleycat = "Alleycat",
        OldMurkeye = "Old Murk-Eye",
        Amalgam = "Amalgam",
        ColdlightSeer = "Coldlight Seer",
        SelflessHero = "Selfless Hero",
        SpawnOfNzoth = "Spawn of N'Zoth",
        ShieldedMinibot = "Shielded Minibot",
        KaboomBot = "Kaboom Bot",
        MecharooToken = "Jo-E Bot",
        BaronRivendare = "Baron Rivendare",
        BronzeWarden = "Bronze Warden",
        Goldrinn = "Goldrinn, the Great Wolf",
        KingBagurgle = "King Bagurgle",
        PackLeader = "Pack Leader",
        MamaBear = "Mama Bear",
        RatPack = "Rat Pack",
        RatToken = "Rat",
        HarvestToken = "Damaged Golem",
        HarvestGolem = "Harvest Golem",
        IronhideDirehorn = "Ironhide Direhorn",
        IronhideToken = "Ironhide Runt",
        Plant = "Plant",
        HeraldOfFlame = "Herald of Flame",
        Maexxna = "Maexxna",
        KangorsApprentice = "Kangor's Apprentice",
        IronSensei = "Iron Sensei",
        RedWhelp = "Red Whelp",
        FiendishServant = "Fiendish Servant",
        KindlyGrandmother = "Kindly Grandmother",
        Imprisoner = "Imprisoner",
        UnstableGhoul = "Unstable Ghoul",
        InfestedWolf = "Infested Wolf",
        TheBeast = "The Beast",
        PilotedShredder = "Piloted Shredder",
        ReplicatingMenace = "Replicating Menace",
        MechanoEgg = "Mechano Egg",
        MurlocScout = "Murloc Scout",
        Tabbycat = "Tabbycat",
        SavannahHighmane = "Savannah Highmane",
        Voidlord = "Voidlord",
        SneedsOldShredder = "Sneed's Old Shredder",
        NadinaTheRed = "Nadina the Red",
        GhastCoiler = "Ghastcoiler",
        GlyphGuardian = "Glyph Guardian",
        CaveHydra = "Cave Hydra",
        MicroMachine = "Micro Machine",
        MurlocTidehunter = "Murloc Tidehunter",
        RockpoolHunter = "Rockpool Hunter",
        DragonspawnLieutenant = "Dragonspawn Lieutenant",
        ScavengingHyena = "Scavenging Hyena",
        Khadgar = "Khadgar",
        WaxriderTogwaggle = "Waxrider Togwaggle",
        ShifterZerus = "Shifter Zerus",
        BolvarFireblood = "Bolvar, Fireblood",
        BrannBronzebeard = "Brann Bronzebeard",
        Murozond = "Murozond",
        MalGanis = "Mal'Ganis",
        Razorgore = "Razorgore, the Untamed",
        FoeReaper = "Foe Reaper 4000",
        Kalecgos = "Kalecgos, Arcane Aspect",
        ZappSlywick = "Zapp Slywick",
        MurlocTidecaller = "Murloc Tidecaller",
        WrathWeaver = "Wrath Weaver",
        MetaltoothLeaper = "Metaltooth Leaper",
        MurlocWarleader = "Murloc Warleader",
        NathrezimOverseer = "Nathrezim Overseer",
        PogoHopper = "Pogo-Hopper",
        StewardOfTime = "Steward of Time",
        Zoobot = "Zoobot",
        ImpToken = "Imp",
        GMToken = "Big Bad Wolf",
        CrowdFavorite = "Crowd Favorite",
        CrystalWeaver = "Crystalweaver",
        Deflectobot = "Deflect-o-Bot",
        FelfinNavigator = "Felfin Navigator",
        HangryDragon = "Hangry Dragon",
        Houndmaster = "Houndmaster",
        ImpGangBoss = "Imp Gang Boss",
        ScrewjankClunker = "Screwjank Clunker",
        SoulJuggler = "Soul Juggler",
        TwilightEmissary = "Twilight Emissary",
        Annoyomodule = "Annoy-o-Module",
        CobaltScalebane = "Cobalt Scalebane",
        DefenderofArgus = "Defender of Argus",
        DrakonidEnforcer = "Drakonid Enforcer",
        FloatingWatcher = "Floating Watcher",
        MenagerieMagician = "Menagerie Magician",
        SecurityRover = "Security Rover",
        Siegebreaker = "Siegebreaker",
        VirmenSensei = "Virmen Sensei",
        Toxfin = "Toxfin",
        AnnihilanBattlemaster = "Annihilan Battlemaster",
        Junkbot = "Junkbot",
        LightfangEnforcer = "Lightfang Enforcer",
        PrimalfinLookout = "Primalfin Lookout",
        StrongshellScavenger = "Strongshell Scavenger",
        GentleMegasaur = "Gentle Megasaur",
        HolyMackerel = "Holy Mackerel",
        ImpMama = "Imp Mama",
        EggToken = "Robosaur",
        TheBeastToken = "Finkle Einhorn",
        Voidwalker = "Voidwalker",
        Hyena = "Hyena",
        Microbot = "Microbot",
        Spider = "Spider",
        RoverToken = "Guard Bot";



    }
    public static Card createFromName(string name)
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
                return new Card(0, "Mecharoo", 1, 1, new List<Effect> { new DeathRattleSummon(Cards.MecharooToken,1) }, false, false, false, Card.Type.Mech,1, name);
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
            case Cards.Deflectobot:
                return new Card(0, "Deflect-o-Bot", 3, 2, new List<Effect> { new RegainDivine(1) }, false, true, false, Card.Type.Mech,3, name);
            case Cards.MecharooToken:
                return new Card(0, "Jo-E Bot", 1, 1, null, false, false, false, Card.Type.Mech,1, name);
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
            case Cards.MurlocScout:
                return new Card(0, "Murloc Scout", 1, 1, null, false, false, false, Card.Type.Murloc, 1, name);
            case Cards.Tabbycat:
                return new Card(0, "Tabbycat", 1, 1, null, false, false, false, Card.Type.Beast, 1, name);
            case Cards.MamaBear:
                return new Card(0, "Mama Bear", 5, 5, new List<Effect> { new SpawnBuffEffect(Card.Type.Beast, 5, 5) }, false, false, false, Card.Type.Beast,6, name);
            case Cards.RatPack:
                return new Card(0, "Rat Pack", 2, 2, new List<Effect> { new RatSummon(Cards.RatToken) }, false, false, false, Card.Type.Beast,2, name);
            case Cards.HarvestGolem:
                return new Card(0, "Harvest Golem", 2, 3, new List<Effect> { new DeathRattleSummon(Cards.HarvestToken,1) }, false, false, false, Card.Type.Mech, 2, name);
            case Cards.HarvestToken:
                return new Card(0, "Damaged Golem", 2, 1, null, false, false, false, Card.Type.Mech, 1, name);
            case Cards.RatToken:
                return new Card(0, "Rat", 1, 1, null, false, false, false, Card.Type.Beast,1, name);
            case Cards.IronSensei:
                return new Card(0, "Iron Sensei", 2, 2, null, false, false, false, Card.Type.Mech, 4, name);
            case Cards.IronhideDirehorn:
                return new Card(0, "Ironhide Direhorn", 7, 7, new List<Effect> { new OverKillSpawn(Cards.IronhideToken) }, false, false, false, Card.Type.Beast, 5, name);
            case Cards.IronhideToken:
                return new Card(0, "Ironhide Runt", 5, 5, null, false, false, false, Card.Type.Beast, 1, name);
            case Cards.HeraldOfFlame:
                return new Card(0, "Herald of Flame", 5, 6, new List<Effect> { new OverKillDamage(3)}, false, false, false, Card.Type.Dragon, 4, name);
            case Cards.Maexxna:
                return new Card(0, "Maexxna", 2, 8, null, true, false, false, Card.Type.Beast, 6, name);
            case Cards.KangorsApprentice:
                return new Card(0, "Kangor's Apprentice", 3, 6, new List<Effect> { new KangorSummon(2)}, true, false, false, Card.Type.None, 6, name);
            case Cards.RedWhelp:
                return new Card(0, "Red Whelp", 1, 2, new List<Effect> { new SoTDragonFire(1) }, false, false, false, Card.Type.Dragon, 1, name);
            case Cards.GhastCoiler:
                return new Card(0, "Ghastcoiler", 7, 7, new List<Effect> { new GhastCoilerSummon(2) }, false, false, false, Card.Type.Beast, 6, name);
            case Cards.GlyphGuardian:
                return new Card(0, "Glyph Guardian", 2, 4, new List<Effect> { new DoubleDmanageAtk(2) }, false, false, false, Card.Type.Dragon, 2, name);
            case Cards.CaveHydra:
                return new Card(0, "Cave Hydra", 2, 4, new List<Effect> { new Cleave() }, false, false, false, Card.Type.Beast, 4, name);
            case Cards.PilotedShredder:
                return new Card(0, "Piloted Shredder", 4, 3, new List<Effect> { new ShredderSummon(1) }, false, false, false, Card.Type.Mech, 3, name);
            case Cards.SneedsOldShredder:
                return new Card(0, "Sneed's Old Shredder", 5, 7, new List<Effect> { new SneedsSummon(1) }, false, false, false, Card.Type.Mech, 5, name);
            case Cards.DragonspawnLieutenant:
                return new Card(0, "Dragonspawn Lieutenant", 2, 3, null, false, false, true, Card.Type.Dragon, 1, name);
            case Cards.TheBeast:
                return new Card(0, "The Beast", 9, 7, new List<Effect> { new OpponentSummon(Cards.TheBeastToken) }, false, false, false, Card.Type.Beast, 3, name);
            case Cards.TheBeastToken:
                return new Card(0, "Finkle Einhorn", 3, 3, null , false, false, false, Card.Type.None, 1, name);
            case Cards.MicroMachine:
                return new Card(0, "Micro Machine", 1, 2, null, false, false, false, Card.Type.Mech, 1, name);
            case Cards.MurlocTidecaller:
                return new Card(0, "Murloc Tidecaller", 1, 2, null, false, false, false, Card.Type.Murloc, 1, name);
            case Cards.MurlocTidehunter:
                return new Card(0, "Murloc Tidehunter", 2, 1, null, false, false, false, Card.Type.Murloc, 1, name);
            case Cards.RockpoolHunter:
                return new Card(0, "Rockpool Hunter", 2, 3, null, false, false, false, Card.Type.Murloc, 1, name);
            case Cards.WrathWeaver:
                return new Card(0, "Wrath Weaver", 1, 1, null, false, false, false, Card.Type.None, 1, name);
            case Cards.Imprisoner:
                return new Card(0, "Imprisoner", 3, 3, new List<Effect> { new DeathRattleSummon(Cards.ImpToken,1) }, false, false, true, Card.Type.Demon, 2, name);
            case Cards.ImpToken:
                return new Card(0, "Imp", 1, 1, null, false, false, false, Card.Type.Demon, 1, name);
            case Cards.KindlyGrandmother:
                return new Card(0, "Kindly Grandmother", 1, 1, new List<Effect> { new DeathRattleSummon(Cards.GMToken,1) }, false, false, false, Card.Type.Beast, 2, name);
            case Cards.GMToken:
                return new Card(0, "Big Bad Wolf", 3, 2, null, false, false, false, Card.Type.Beast, 1, name);
            case Cards.MetaltoothLeaper:
                return new Card(0, "Metaltooth Leaper", 3, 3, null, false, false, false, Card.Type.Mech, 2, name);
            case Cards.NathrezimOverseer:
                return new Card(0, "Nathrezim Overseer", 2, 3,null, false, false, false, Card.Type.Demon, 2, name);
            case Cards.PogoHopper:
                return new Card(0, "Pogo-Hopper", 1, 1, null, false, false, false, Card.Type.Mech, 2, name);
            case Cards.StewardOfTime:
                return new Card(0, "Steward of Time", 3, 4, null, false, false, false, Card.Type.Dragon, 2, name);
            case Cards.Zoobot:
                return new Card(0, "Zoobot", 3, 3, null, false, false, false, Card.Type.Mech, 2, name);
            case Cards.CrowdFavorite:
                return new Card(0, "Crowd Favorite", 4, 4, null, false, false, false, Card.Type.None, 3, name);
            case Cards.CrystalWeaver:
                return new Card(0, "Crystal Weaver", 5, 4, null, false, false, false, Card.Type.None, 3, name);
            case Cards.FelfinNavigator:
                return new Card(0, "Felfin Navigator", 4, 4, null, false, false, false, Card.Type.Murloc, 3, name);
            case Cards.HangryDragon:
                return new Card(0, "Hangry Dragon", 4, 4, null, false, false, false, Card.Type.Dragon, 3, name);
            case Cards.Houndmaster:
                return new Card(0, "Houndmaster", 4, 3, null, false, false, false, Card.Type.None, 3, name);
            case Cards.ScrewjankClunker:
                return new Card(0, "Screwjank Clunker", 2, 5, null, false, false, false, Card.Type.Mech, 3, name);
            case Cards.ShifterZerus:
                return new Card(0, "Shifter Zerus", 1, 1, null, false, false, false, Card.Type.None, 3, name);
            case Cards.TwilightEmissary:
                return new Card(0, "Twilight Emissary", 4, 4, null, false, false, true, Card.Type.Dragon, 3, name);
            case Cards.Annoyomodule:
                return new Card(0, "Annoy-o-Module", 2, 4, null, false, true, true, Card.Type.Mech, 4, name);
            case Cards.CobaltScalebane:
                return new Card(0, "Cobalt Scalebane", 5, 5, null, false, false, false, Card.Type.Dragon, 4, name);
            case Cards.DefenderofArgus:
                return new Card(0, "Defender of Argus", 2, 3, null, false, false, false, Card.Type.None, 4, name);
            case Cards.FloatingWatcher:
                return new Card(0, "Floating Watcher", 4, 4, null, false, false, false, Card.Type.Demon, 4, name);
            case Cards.MechanoEgg:
                return new Card(0, "Mechano Egg", 0, 5, new List<Effect> { new DeathRattleSummon(Cards.EggToken,1) }, false, false, false, Card.Type.Mech, 4, name);
            case Cards.EggToken:
                return new Card(0, "Robosaur", 8, 8, null, false, false, false, Card.Type.Mech, 1, name);
            case Cards.Amalgam:
                return new Card(0, "Amalgam", 1, 1, null, false, false, false, Card.Type.All, 1, name);
            case Cards.MenagerieMagician:
                return new Card(0, "Menagerie Magician", 4, 4, null, false, false, false, Card.Type.None, 4, name);
            case Cards.Toxfin:
                return new Card(0, "Toxfin", 1, 2, null, false, false, false, Card.Type.Murloc, 4, name);
            case Cards.VirmenSensei:
                return new Card(0, "Virmen Sensei", 4, 5, null, false, false, false, Card.Type.None, 4, name);
            case Cards.AnnihilanBattlemaster:
                return new Card(0, "Annihilan Battlemaster", 3, 1, null, false, false, false, Card.Type.Demon, 5, name);
            case Cards.BrannBronzebeard:
                return new Card(0, "Brann Bronzebeard", 2, 4, null, false, false, false, Card.Type.None, 5, name);
            case Cards.LightfangEnforcer:
                return new Card(0, "Lightfang Enforcer", 2, 2, null, false, false, false, Card.Type.None, 5, name);
            case Cards.Murozond:
                return new Card(0, "Murozond", 5, 5, null, false, false, false, Card.Type.Dragon, 5, name);
            case Cards.PrimalfinLookout:
                return new Card(0, "Primalfin Lookout", 3, 2, null, false, false, false, Card.Type.Murloc, 5, name);
            case Cards.Razorgore:
                return new Card(0, "Razorgore, the Untamed", 2, 4, null, false, false, false, Card.Type.Dragon, 5, name);
            case Cards.StrongshellScavenger:
                return new Card(0, "Strongshell Scavenger", 2, 3, null, false, false, false, Card.Type.None, 5, name);
            case Cards.FoeReaper:
                return new Card(0, "Foe Reaper 4000", 6, 9, new List<Effect> { new Cleave() }, false, false, false, Card.Type.Mech, 6, name);
            case Cards.GentleMegasaur:
                return new Card(0, "Gentle Megasaur", 5, 4, null, false, false, false, Card.Type.Beast, 6, name);
            case Cards.Kalecgos:
                return new Card(0, "Kalecgos, Arcane Aspect", 4, 12, null, false, false, false, Card.Type.Dragon, 6, name);
            case Cards.Voidwalker:
                return new Card(0, "Voidwalker", 1, 3, null, false, false, true, Card.Type.Demon, 1, name);
            case Cards.Voidlord:
                return new Card(0, "Voidlord", 3, 9, new List<Effect> { new DeathRattleSummon(Cards.Voidwalker,3) }, false, false, true, Card.Type.Demon, 5, name);
            case Cards.Hyena:
                return new Card(0, "Hyena", 2, 2, null, false, false, false, Card.Type.Beast, 1, name);
            case Cards.Microbot:
                return new Card(0, "Microbot", 1, 1, null, false, false, false, Card.Type.Mech, 1, name);
            case Cards.Spider:
                return new Card(0, "Spider", 1, 1, null, false, false, false, Card.Type.Beast, 1, name);
            case Cards.SavannahHighmane:
                return new Card(0, "Savannah Highmane", 6, 5, new List<Effect> { new DeathRattleSummon(Cards.Hyena, 2) }, false, false, false, Card.Type.Beast, 4, name);
            case Cards.ReplicatingMenace:
                return new Card(0, "Replicating Menace", 3, 1, new List<Effect> { new DeathRattleSummon(Cards.Microbot, 3) }, false, false, false, Card.Type.Mech, 3, name);
            case Cards.InfestedWolf:
                return new Card(0, "Infested Wolf", 3, 3, new List<Effect> { new DeathRattleSummon(Cards.Spider, 2) }, false, false, false, Card.Type.Beast, 3, name);
            case Cards.FiendishServant:
                return new Card(0, "Fiendish Servant", 2, 1, new List<Effect> { new Fiendish(1) }, false, false, false, Card.Type.Demon, 1, name);
            case Cards.NadinaTheRed:
                return new Card(0, "Nadina the Red", 7, 4, new List<Effect> { new DragonDivine(Card.Type.Dragon) }, false, false, false, Card.Type.None, 6, name);
            case Cards.Junkbot:
                return new Card(0, "Junkbot", 1, 5, new List<Effect> { new BuffFromDeath(2,2,Card.Type.Mech) }, false, false, false, Card.Type.Mech, 5, name);
            case Cards.MalGanis:
                return new Card(0, "Mal'Ganis", 9, 7, new List<Effect> { new MalGanisSoT(2),new SpawnBuffEffect(Card.Type.Demon,2,2), new MalGanisDeath(2) }, false, false, false, Card.Type.Demon, 5, name);
            case Cards.ScavengingHyena:
                return new Card(0, "Scavenging Hyena", 2, 2, new List<Effect> { new BuffFromDeath(2, 1, Card.Type.Beast) }, false, false, false, Card.Type.Beast, 2, name);
            case Cards.UnstableGhoul:
                return new Card(0, "Unstable Ghoul", 1, 3, new List<Effect> { new DeathAoE(1) }, false, false, true, Card.Type.None, 2, name);
            case Cards.MurlocWarleader:
                return new Card(0, "Murloc Warleader", 3, 3, new List<Effect> { new FriendlyDmbBonus(Card.Type.Murloc,2) }, false, false, false, Card.Type.Murloc, 2, name);
            case Cards.Siegebreaker:
                return new Card(0, "Siegebreaker", 5, 8, new List<Effect> { new FriendlyDmbBonus(Card.Type.Demon, 1) }, false, false, true, Card.Type.Demon, 4, name);
            case Cards.ImpGangBoss:
                return new Card(0, "Imp Gang Boss", 2, 4, new List<Effect> { new SummonOnDmgTaken(Cards.ImpToken) }, false, false, false, Card.Type.Demon, 3, name);
            case Cards.SecurityRover:
                return new Card(0, "Security Rover", 2, 6, new List<Effect> { new SummonOnDmgTaken(Cards.RoverToken) }, false, false, false, Card.Type.Mech, 4, name);
            case Cards.RoverToken:
                return new Card(0, "Guard Bot", 2, 3, null, false, false, true, Card.Type.Mech, 1, name);
            case Cards.BolvarFireblood:
                return new Card(0, "Bolvar, Fireblood", 1, 7, new List<Effect> { new BuffFromDivine(2,0) }, false, true, false, Card.Type.None, 4, name);
            case Cards.DrakonidEnforcer:
                return new Card(0, "Drakonid Enforcer", 3, 6, new List<Effect> { new BuffFromDivine(2, 2) }, false, false, false, Card.Type.Dragon, 4, name);
            case Cards.SoulJuggler:
                return new Card(0, "Soul Juggler", 3, 3, new List<Effect> { new Juggler(1,Card.Type.Demon) }, false, false, false, Card.Type.None, 3, name);
            case Cards.ZappSlywick:
                return new Card(0, "Zapp Slywick", 7, 10, null, false, false, false, Card.Type.None, 6, name).setWindfury(true);
            case Cards.WaxriderTogwaggle:
                return new Card(0, "Waxrider Togwaggle", 1, 2, new List<Effect> { new BuffFromKill(2,2, Card.Type.Dragon) }, false, false, false, Card.Type.None, 2, name);
            case Cards.ImpMama:
                return new Card(0, "Imp Mama", 6, 10, new List<Effect> { new SummonImpMama(1) }, false, false, false, Card.Type.Demon, 6, name);
            case Cards.HolyMackerel:
                return new Card(0, "Holy Mackerel", 8, 4, new List<Effect> { new DivineRefresh() }, false, false, false, Card.Type.Murloc, 6, name);
            case Cards.Khadgar:
                return new Card(0, "Khadgar", 2, 2, new List<Effect> { new KhadgarEffect(1) }, false, false, false, Card.Type.None, 3, name);
            case Cards.Plant:
                return new Card(0, "Plant", 1, 1, null, false, false, false, Card.Type.None, 1, name);
            default:
                return new Card(0, "UknownCard",0,0,null,false,false,false,Card.Type.None,1,"UknownCard");

        }
    }
    public static Card createGoldenFromName(string name)
    {
        return createFromName(name).makeGolden();
    }
}