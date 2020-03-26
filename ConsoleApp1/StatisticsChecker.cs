using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class StatisticsChecker
    {
        public static void runTests()
        {
            performTest(testCase1);
            

            Console.ReadLine();





            return;


          

        }

        public static void performTest(Func<BoardSide, BoardSide, string> setUp)
        {
            ConsoleColor defaults = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            HearthstoneBoard b = new HearthstoneBoard();
            BoardSide b1 = new BoardSide();
            BoardSide b2 = new BoardSide();
            string testname = setUp(b1, b2);
            Console.WriteLine("Starting test: " + testname + "!--------------------------------------------------");
            Console.ForegroundColor = defaults;
            b.p1Board = b1;
            b.p2Board = b2;
            try
            {

                b.printState();
                Console.WriteLine("#####");
               // b.printEvents = true;
                List<HearthstoneBoard> res = b.simulateResults(10000);
                Console.WriteLine("REDSADSAD: " + HearthstoneBoard.counts[0] + " " + HearthstoneBoard.counts[1]);


                var dmgdist = StatisticsManager.calculateDmgDistributions(res);
                StatisticsManager.printReadableResult(dmgdist);
            


               



            }
            catch (ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne e)
            {
                Console.WriteLine("An error occured when performing test " + testname + ". Message: " + e.message);
                Console.ReadLine();
            }

        }


        public static string testCase0(BoardSide b1, BoardSide b2)
        {
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.KaboomBot));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.VulgarHomunculus).setStats(10, 10));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));



            return "RandTest";

        }

        public static string testCase1(BoardSide b1, BoardSide b2)
        {  
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b1.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha).setStats(10, 10));
            b2.Add(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DireWolfAlpha));
            


            return "Test 1";

        }



    

    }
}

