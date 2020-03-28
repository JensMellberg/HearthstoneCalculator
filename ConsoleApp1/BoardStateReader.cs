using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class BoardStateReader
    {
        public BoardStateReader()
        {

        }
        bool combat = false;
        bool waitingForStart = false;
        int taverntier = 1;
        public Dictionary<string, Card> minions = new Dictionary<string, Card>();
        public void run(string path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (true)
                    {
                      //Console.WriteLine("Before read..");
                        string moreData = reader.ReadToEnd();
                      //Console.WriteLine("Read data of "+moreData.Length+" characters...");
                        if (!moreData.Equals(""))
                        {
                            string[] lines = moreData.Split('\n');
                            for (int i = 0; i < lines.Length; i++)
                                handleInput(lines[i]);
                        }
                        
                        Thread.Sleep(4000);
                    }
                }
            }
        }

        HearthstoneBoard board = new HearthstoneBoard();
        int menacebuffswaiting = 0;
        public void handleInput(string line)
        {
            if (line.Equals(""))
                return;
            line = line.Substring(0, line.Length - 1) + "|";
            //if (line.Contains("D 03:39:38.6461687 ZoneChangeList.ProcessChanges() - id=225 local=False [entityName=Replicating Menace id=2466 zone=SETASIDE zonePos=0 cardId=BOT_312e player=9] zone from  -> "))
            //  Console.WriteLine(line);
            if (line.Contains("cardId=BOT_312e") && line.Contains("from  -> ") && line[line.Length - 2].Equals(' '))
            {

                Console.WriteLine("Next minion to recieve buff will get menace");
                menacebuffswaiting++;
                Console.ReadLine();
            }

          

            if (line.Contains("entityName="))
            {

                string[] a = getNameAndId(line);
                if (!minions.ContainsKey(a[1]))
                {
                    minions.Add(a[1], CardCreatorFactory.createFromName(a[0]).setId(int.Parse(a[1])));
                    //Console.WriteLine("New entity: " + nameIDString(line));
                    //Console.WriteLine(line);
                }

                //Console.WriteLine(line); getStringBetween(line, "entityName=", " id=")
            }

            if (combat && !waitingForStart)
            {
                if (line.Contains("TRANSITIONING") && line.Contains("entityName=Refresh") && line.Contains("FRIENDLY PLAY"))
                {
                    Console.WriteLine("COMBAT OVER!!!");
                    board = new HearthstoneBoard();
                    combat = false;
                }
                if (line.Contains("entityName="))
                {

                    string[] a = getNameAndId(line);
                    if (!minions.ContainsKey(a[1]))
                    {
                        minions.Add(a[1], CardCreatorFactory.createFromName(a[0]).setId(int.Parse(a[1])));
                        //Console.WriteLine("New entity: " + nameIDString(line));
                        //Console.WriteLine(line);
                    }

                    //Console.WriteLine(line); getStringBetween(line, "entityName=", " id=")
                }
                if (line.Contains("tag=COPIED_FROM_ENTITY_ID value=") && !line.Contains("tag=COPIED_FROM_ENTITY_ID value=0") && line.Contains("ProcessChanges()"))
                {
                    string newvalue = getStringBetween(line, "tag=COPIED_FROM_ENTITY_ID value=", " ");
                    Console.WriteLine(nameIDString(line)+ "getting copied over from "+ newvalue);
                    string[] a = getNameAndId(line);
                   minions[a[1]] = minions[newvalue].copy();

                }
                return;

            }


            if (line.Contains("zone from FRIENDLY PLAY -> ") && line[line.Length-2].Equals(' '))
            {
                string id = getNameAndId(line)[1];
                Console.WriteLine("Friendly minion sold:" + nameIDString(line));
                board.p1Board.Remove(minions[id]);
            }
            if (line.Contains("pos from") && line.Contains("->") && !combat)
            {
                string id = getNameAndId(line)[1];
                if (board.p1Board.Contains(minions[id]))
                {
                    Console.WriteLine("Friendly minion change pos:" + nameIDString(line)  +" to "+line[line.Length-2]);
                    minions[id].BoardPosition = int.Parse((line[line.Length - 2]).ToString());
                }
            }


            
                   if (line.Contains("TRANSITIONING card [entityName=Tavern Tier ") && line.Contains(" to ") && line[line.Length - 2].Equals(' ') && !combat)
            {
                string tier = line[line.IndexOf("Tavern Tier") + 12].ToString();
                Console.WriteLine("Upgrade to tier " + tier);
                taverntier = int.Parse(tier);
                //Console.WriteLine(line); getStringBetween(line, "entityName=", " id=")
            }

            if (line.Contains("TRANSITIONING card") && line.Contains("to FRIENDLY PLAY") && line[line.Length - 2].Equals('Y'))
            {
                string id = getNameAndId(line)[1];
                Console.WriteLine("Friendly minion added:" + nameIDString(line));
                board.p1Board.Add(minions[id]);
                minions[id].printState();
                //Console.WriteLine(line); getStringBetween(line, "entityName=", " id=")
            }


           

            if (line.Contains("TRANSITIONING card") && line.Contains("to OPPOSING PLAY") && line[line.Length - 2].Equals('Y') && combat)
            {
                string id = getNameAndId(line)[1];
                Console.WriteLine("Enemy minion added:" + getNameAndId(line)[0] + " id:" + id);
         
                board.p2Board.Add(minions[id]);
                minions[id].printState();
            }

           // if (line.Contains("TRANSITIONING card") && line.Contains("to OPPOSING PLAY (Hero)") && combat)
            //{
              //  string id = getNameAndId(line)[1];
               // Console.WriteLine("NU ÄR DET DAX VA");
                //Console.WriteLine(line);

   //         }

            if (line.Contains("tag=PLAYER_TECH_LEVEL value=") && line.Contains("ProcessChanges"))
            {
                //Console.WriteLine("Friendly minion added:" +getStringBetween(line, "entityName=", " id="));
                string newvalue = getStringBetween(line, "tag=PLAYER_TECH_LEVEL value=", " ");
                startRound(newvalue);

            }


            if (line.Contains("Thuzad") && line.Contains("OPPOSING PLAY (Hero)"))
            {
                Console.WriteLine("Kel'Thuzad detected");
                startRound("0");

            }

            if (line.Contains("TRANSITIONING") && line.Contains("entityName=Refresh") && line[line.Length - 2].Equals(' '))
            {
                Console.WriteLine("COMBAT STARTS!!!");
                waitingForStart = true;
                combat = true;
            }
     
            if (line.Contains("tag=ATK value=") && line.Contains("ProcessChanges"))
            {
                //Console.WriteLine("Friendly minion added:" +getStringBetween(line, "entityName=", " id="));
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
              
                string newvalue = getStringBetween(line, "tag=ATK value=", " ");
                c.setAtk(int.Parse(newvalue));
                if (menacebuffswaiting >0)
                {
                    Console.WriteLine("Added menace buff on " + nameIDString(line));
                    for (int i = 0; i < menacebuffswaiting; i++)
                        c.addEffect(new DeathRattleSummon(CardCreatorFactory.Cards.ReplicatingMenace, 3));
                    menacebuffswaiting = 0;
                }

               // if (board.containsCard(c))
                Console.WriteLine("Attack changed on "+nameIDString(line) +" to "+newvalue);


            }
            if (line.Contains("tag=HEALTH value=") && line.Contains("ProcessChanges"))
            {
                //Console.WriteLine("Friendly minion added:" +getStringBetween(line, "entityName=", " id="));
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];

                string newvalue = getStringBetween(line, "tag=HEALTH value=", " ");
                c.setHp(int.Parse(newvalue));

                if (board.containsCard(c))
                    Console.WriteLine("Health changed on " + nameIDString(line) + " to " + newvalue);
            }


           

            if (line.Contains("tag = BACON_MINION_IS_LEVEL_TWO"))
            {
                //Console.WriteLine("Friendly minion added:" +getStringBetween(line, "entityName=", " id="));
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
                if (c.golden)
                    return;
                c.makeGoldenEffects();
                Console.WriteLine("Found golden: " + nameIDString(line) + "!!!!");
            }
           



        }

        public void startRound(string newvalue)
        {
        


            if (waitingForStart)
            {
                Console.WriteLine("NU ÄR DET DAXXXXXXX!!!! IGEN (tavern tier: " + newvalue + ")");
                Card[] cards = new Card[board.p1Board.Count];
                foreach (Card c in board.p1Board)
                    cards[c.BoardPosition - 1] = c;
                board.p1Board = new BoardSide();
                board.p1Board.tavernTier = taverntier;
                board.p2Board.tavernTier = int.Parse(newvalue);
                for (int i = 0; i < cards.Length; i++)
                    board.p1Board.Add(cards[i]);
                board.printState();
                List<HearthstoneBoard> res = board.simulateResults(10);
                var dmgdist = StatisticsManager.calculateDmgDistributions(res);
                StatisticsManager.printReadableResult(dmgdist);

                waitingForStart = false;
            }
        }

        public string nameIDString(string line)
        {
            string[] a = getNameAndId(line);
            return a[0] + " id: " + a[1];
        }

        public string[] getNameAndId(string line)
        {
            string name = getStringBetween(line,"entityName=", " id=");
            int a = line.IndexOf("entityName=");
            string rest = line.Substring(a);
            int start = rest.IndexOf("id=");
            int point = start + 3;
            while (!rest[point].Equals(' ') && point < 1000)
                point++;
            if (point == 1000)
                Console.WriteLine("Wierd stuck on " + line);
            string id = rest.Substring(start + 3, point - start - 3);
            return new string[] { name, id };
        }

        public string getStringBetween(string line, string start, string end)
        {
            int a = line.IndexOf(start);
            int e = line.Substring(a+start.Length).IndexOf(end) + a+start.Length;
            return line.Substring(a + start.Length, e - a - start.Length);
        }


        public static void Run()
        {
            new BoardStateReader().run(@"D:\Program1\Program\Hearthstone\Logs\Zone.log");
        }


    }
}
