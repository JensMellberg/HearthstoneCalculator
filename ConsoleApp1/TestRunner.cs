using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TestRunner
    {
        public static HearthstoneBoard.OutputPriority outputPriority = HearthstoneBoard.OutputPriority.ATTACKERS;
        public static void runTests()
        {
            performTest(testCase0);
            performTest(testCase2);
            performTest(testMinibotDivine);
            performTest(testDeathBuff);
            performTest(testKaboom);
            performTest(testCobalt);
            performTest(testRivendare);
            performTest(testSelfless);
            performTest(testReborn);
            performTest(testMurloc);
            performTest(testDeathBeastBuff);
            performTest(testRatPack);
            performTest(testMecharooFullBoard);
            performTest(testRatPackFullBoard);
            performTest(testMamaBear);
            performTest(testOverkillIronhide);
            performTest(testOverkillDragon);
            performTest(testPoison);
            performTest(testKangor);
            performTest(testFiendish);
            performTest(testSoT);
            performTest(testGlyphGuardian);
            performTest(testHydra);
            performTest(testMultipleSummon);
            performTest(testNadina);
            performTest(testJunkbot);
            performTest(testGhoul);
            Console.ReadLine();





            return;


          

        }

        public static void performTest(Func<BoardSide, BoardSide, List<HearthstoneBoard>, string> setUp)
        {
            ConsoleColor defaults = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            HearthstoneBoard b = new HearthstoneBoard();
            BoardSide b1 = new BoardSide();
            BoardSide b2 = new BoardSide();
            List<HearthstoneBoard> checkers = new List<HearthstoneBoard>();
            string testname = setUp(b1, b2, checkers);
            Console.WriteLine("Starting test: " + testname + "!--------------------------------------------------");
            Console.ForegroundColor = defaults;
            b.p1Board = b1;
            b.p2Board = b2;
            try
            {

                b.printState();
                Console.WriteLine("#####");
                b.printPriority = outputPriority;
                HearthstoneBoard res = b.simulateResults(1)[0];
                Console.WriteLine(testname + " done. Board state: ");
                res.printState();

                foreach (HearthstoneBoard h in checkers)
                    if (res.compare(h))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Test (" + testname + ") passed!!!");
                        Console.WriteLine("----------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.ForegroundColor = defaults;

                        return;
                    }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Test (" + testname + ") failed. Board was :");
                res.printState();
                Console.WriteLine("But should be: ");
                checkers[0].printState();
                Console.ForegroundColor = defaults;
                Console.ReadLine();

            }
            catch (ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne e)
            {
                Console.WriteLine("An error occured when performing test " + testname + ". Message: " + e.message);
                Console.ReadLine();
            }

        }

    


        public static string testCase0(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {  
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RighteousProtector));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha) };
            expected.Add(exp1);
            return "Test 1";

        }

        public static string testCase2(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            expected.Add(exp1);
            return "Test 1";

        }

        public static string testMinibotDivine(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.ShieldedMinibot));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            expected.Add(exp1);
            return "test minibot divine";

        }

        public static string testCobalt(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.CobaltGuardian));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo).setTaunt(true));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.ShieldedMinibot));
   
            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo) };

            HearthstoneBoard exp2 = new HearthstoneBoard();
            exp2.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.CobaltGuardian).setStats(6, 1).setDivineShield(true) };

            expected.Add(exp1);
            expected.Add(exp2);
            return "test cobalt guardian";

        }

        public static string testKaboom(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.KaboomBot));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Annoyomodule));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Annoyomodule).setDivineShield(false));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p2Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Annoyomodule).setDivineShield(false) };
            expected.Add(exp1);
            return "test kaboom";

        }

        public static string testDeathBuff(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.SpawnOfNzoth));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha).setStats(3, 1) };
            expected.Add(exp1);
            return "test deathbuff";

        }

        public static string testReborn(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.BronzeWarden));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.KaboomBot));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.BronzeWarden) };
            expected.Add(exp1);
            return "test reborn";

        }


        public static string testSelfless(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.SelflessHero));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha) };
            expected.Add(exp1);
            return "test selfless hero";

        }

        public static string testRivendare(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.BaronRivendare));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
      
            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.BaronRivendare) };

            HearthstoneBoard exp2 = new HearthstoneBoard();
            exp2.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.BaronRivendare).setStats(1, 5) };

            expected.Add(exp1);
            expected.Add(exp2);
            return "test rivendare";

        }

        public static string testMurloc(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.OldMurkeye));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.ColdlightSeer));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.OldMurkeye).setStats(2, 2) };
            expected.Add(exp1);
            return "test murloc";

        }

        public static string testDeathBeastBuff(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Goldrinn));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Goldrinn).setStats(5,5));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat).setStats(5, 5) };
            expected.Add(exp1);
            return "test beastbuffdeath";

        }

        public static string testRatPack(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack).setStats(3,2));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha) };
            expected.Add(exp1);
            return "test ratpack";
        }

        public static string testMecharooFullBoard(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo).setTaunt(true));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo),
                CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo),
                  CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo),  CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken)   };
            expected.Add(exp1);
            return "test mecharoo full board";
        }

        public static string testRatPackFullBoard(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack).setStats(6,6).setTaunt(true));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(8,8));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack),
                CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack),
                  CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack),  CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken)   };
            expected.Add(exp1);
            return "test ratpack full board";
        }

        public static string testMamaBear(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatPack));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MamaBear));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo).setTaunt(true));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(3,3));

            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken).setStats(6,6), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RatToken).setStats(6, 6),
                CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MamaBear), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken) };
            expected.Add(exp1);
            return "test mama bear";
        }

        public static string testOverkillIronhide(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.IronhideDirehorn));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(1, 3));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(1,20));


            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.IronhideDirehorn).setStats(7,3), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.IronhideToken) };
            expected.Add(exp1);
            return "test overkill ironhide";
        }

        public static string testOverkillDragon(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.HeraldOfFlame));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));
          


            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.HeraldOfFlame).setStats(5, 3)};
            expected.Add(exp1);
            return "test overkill dragon";
        }

        public static string testKangor(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.IronSensei));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.KangorsApprentice));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(1,1));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(1, 1));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(8,8));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken) };
            expected.Add(exp1);
            return "test kangor";
        }

        public static string testPoison(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Maexxna));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Maexxna));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            expected.Add(exp1);
            return "test poison";
        }

        public static string testSoT(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp));
           // b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp) };
            expected.Add(exp1);
            return "test red whelp start of turn";
        }

        public static string testGlyphGuardian(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.GlyphGuardian));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat).setStats(1,6));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));
            // b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            
            expected.Add(exp1);
            return "test glyph guardian";
        }

        public static string testFiendish(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.FiendishServant));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(4,9));
            // b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.RedWhelp));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha) };
            expected.Add(exp1);

            HearthstoneBoard exp2 = new HearthstoneBoard();
            expected.Add(exp2);

            return "test fiendish";
        }


        public static string testHydra(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(1, 2));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.HarvestGolem));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.CaveHydra));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.HarvestGolem).setStats(2, 1) };
            expected.Add(exp1);
            return "test hydra";
        }

        public static string testMultipleSummon(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat).setStats(10,10));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Voidlord));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat).setStats(9,3));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat).setStats(10, 8),
            CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat),CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat),CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat),CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat)
            ,CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Voidwalker),CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Voidwalker)};
            expected.Add(exp1);
            return "test multiple summon";
        }
        public static string testNadina(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.NadinaTheRed));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.GlyphGuardian));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.CaveHydra).setStats(4,4));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.GlyphGuardian).setDivineShield(true),CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Alleycat) };
            expected.Add(exp1);
            return "test nadina";
        }

        public static string testJunkbot(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.IronSensei).setTaunt(true));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Junkbot));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(2,2));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.ScavengingHyena));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p1Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Junkbot).setStats(3,5)};
            expected.Add(exp1);
            return "test junkbot";
        }

        public static string testGhoul(BoardSide b1, BoardSide b2, List<HearthstoneBoard> expected)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.UnstableGhoul).setStats(2,2));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Annoyomodule));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Mecharoo));



            HearthstoneBoard exp1 = new HearthstoneBoard();
            exp1.p2Board = new BoardSide { CardCreatorFactory.createFromName(CardCreatorFactory.Cards.Annoyomodule).setStats(2, 2).setDivineShield(false), CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MecharooToken) };
            expected.Add(exp1);
            return "test ghoul";
        }





    }
}

