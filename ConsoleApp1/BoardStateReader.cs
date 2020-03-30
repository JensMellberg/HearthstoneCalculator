using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public class BoardStateReader
    {
        public const string PathID = "Path";
        public const string SimulationID = "Simulations";
        public string path;
        public BoardStateReader(Dictionary<string, string> settings)
        {
            this.settings = settings;
        }
        bool combat = false;
        bool waitingForStart = false;
        int taverntier = 1;
        FileStream s2;
        StreamReader powerReader;
        string[] powerData;
        public int simulationCount = 10000;
        int powerPointer;
        GUI gui;
        //BoardGUI boardgui;
        public bool pause = false;
        public bool initialized = false;
        public Dictionary<string, DrawableCard> minions = new Dictionary<string, DrawableCard>();

        public void initializeSettings()
        {
            if (settings.ContainsKey(PathID))
                path = settings[PathID];
            if (settings.ContainsKey(SimulationID))
                simulationCount = int.Parse(settings[SimulationID]);
        }

        public void run()
        {
            initializeSettings();
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Blizzard\Hearthstone\log.config";
            if (!File.Exists(configPath))
            {

                Console.WriteLine("Creating log config file. Restart hearthstone for it to be active");
                try
                {
                    using (StreamWriter sw = new StreamWriter(configPath))
                    {
                        sw.WriteLine("[Zone]");
                        sw.WriteLine("LogLevel=1");
                        sw.WriteLine("FilePrinting=true");
                        sw.WriteLine("ConsolePrinting=true");
                        sw.WriteLine("ScreenPrinting=false");
                        sw.WriteLine("[Power]");
                        sw.WriteLine("LogLevel=1");
                        sw.WriteLine("FilePrinting=true");
                        sw.WriteLine("ConsolePrinting=true");
                        sw.Write("ScreenPrinting=false");
                    }
                }
                catch
                {
                    Console.WriteLine("Error creating log file for hearthstone. Please manually add it, instructions can be found at https://github.com/jleclanche/fireplace/wiki/How-to-enable-logging. Enable Power and Zone logging");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                }
            }
            if (path == null)
            {
                Console.WriteLine("Could not detect a path for the hearthstone directory. Please select it to begin.");
                Console.WriteLine("It is usually installed in C:\\Program Files (x86)\\Hearthstone");
                Console.WriteLine("If you already have hearthstone running you will need to restart it if this is your first time using this program");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
                demandPath();
            }

            while (true)
            {
                try
                {
                   
                    runLoop();
                } catch (FileNotFoundException)
                {
                    Console.WriteLine("Cannot find log files, make sure that the path is correctly set");
                    Console.ReadLine();
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("The hearthstone directory path entered is invalid, please select a new one.");
                    Console.WriteLine("It is usually installed in C:\\Program Files (x86)\\Hearthstone");
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    demandPath();
                }
            }
           
          
        }

        public void demandPath()
        {

               
              
                OpenFileDialog folderBrowser = new OpenFileDialog();
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckFileExists = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.FileName = "Folder";

                while (true)
                {
                    if (folderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                        path = folderPath + @"\Logs\";
                        settings[PathID] = path;
                        saveSettings();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please select a folder. Press enter to continue");
                        Console.ReadLine();
                    }
                }
        }

        public void runLoop()
        {
            using (FileStream stream = File.Open(path + "Zone.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    s2 = File.Open(path + "Power.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    powerReader = new StreamReader(s2);
                    string firstData = reader.ReadToEnd();
                    if (!firstData.Equals(""))
                    {
                        Console.WriteLine("Already existing game information detected, simulate on the existing data? (y/n)");
                        while (true)
                        {
                           string ans = Console.ReadLine();
                            if (ans.Equals("y") || ans.Equals("yes"))
                            {
                                initialized = true;
                                Thread thr2 = new Thread(new ThreadStart(FormRun));
                                thr2.SetApartmentState(ApartmentState.STA);
                                thr2.Start();
                                string[] lineses = firstData.Split('\n');
                                    for (int i = 0; i < lineses.Length; i++)
                                        handleInput(lineses[i]);
                                break;
                            }
                            else if (ans.Equals("n") || ans.Equals("no"))
                            {
                                initialized = true;
                                Thread thr2 = new Thread(new ThreadStart(FormRun));
                                thr2.SetApartmentState(ApartmentState.STA);
                                thr2.Start();
                                powerReader.ReadToEnd();
                                break;
                            }

                        }
                    }
                  

                    while (true)
                    {
                        while (pause)
                            Thread.Sleep(100);
                        //Console.WriteLine("Before read..");
                        string moreData = reader.ReadToEnd();
                       // Console.WriteLine("Read data of "+moreData.Length+" characters...");
                        if (!moreData.Equals(""))
                        {
                            string[] lines = moreData.Split('\n');
                            for (int i = 0; i < lines.Length; i++)
                                handleInput(lines[i]);
                        }
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        public void saveSettings()
        {
            using (StreamWriter sw = new StreamWriter("Settings.txt"))
            {
                if (settings.ContainsKey(PathID))
                    sw.WriteLine(PathID + "=" + settings[PathID]);
                if (settings.ContainsKey(SimulationID))
                    sw.WriteLine(SimulationID + "=" + settings[SimulationID]);
            }
            initializeSettings();

        }

        public string getBoard(BoardSide b1, BoardSide b2)
        {
            foreach (DrawableCard c in board.p1Board)
                b1.Add(c.copy());
            foreach (DrawableCard c in board.p2Board)
                b2.Add(c.copy());
            return "runtime test";
        }

        public void readInput()
        {
            while (!initialized)
                Thread.Sleep(100);
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("pause"))
                    pause = true;
                else if (input.Equals("unpause"))
                    pause = false;
                else if (input.Equals("print board"))
                    board.printState();
                else if (input.Equals("test"))
                    TurnByTurnChecker.performTest(getBoard);
            }
        }


        public void startNewGame()
        {
            expectedHealth = 40;
            board = new DrawableHearthstoneBoard();
           combat = false;
           waitingForStart = false;
            minions = new Dictionary<string, DrawableCard>();
            menacebuffswaiting = 0;
            luckywins = 0;
            unluckyloss = 0;
        }
        DrawableHearthstoneBoard board = new DrawableHearthstoneBoard();
        int menacebuffswaiting = 0;
        string heroId = "null";
        bool expectedLoss;
        int luckywins = 0;
        int unluckyloss = 0;
        int expectedHealth = 40;
        public void handleInput(string line)
        {
            if (line.Equals(""))
                return;
           // while (boardgui == null)
             //   Thread.Sleep(100);
            line = line.Substring(0, line.Length - 1) + "|";

            if (line.Contains("TRANSITIONING card") && line.Contains("to FRIENDLY PLAY (Hero)"))
            {
                if (getNameAndId(line)[0].Equals("BaconPHhero"))
                    return;
                string id = getNameAndId(line)[1];
                Console.WriteLine("Hero Discovered:" + nameIDString(line)+ " initializing new game");
                heroId = id;
                startNewGame();
            }


            if (line.Contains("cardId=BOT_312e") && line.Contains("from  -> ") && line[line.Length - 2].Equals(' '))
            {

                Console.WriteLine("Next minion to recieve buff will get menace");
                menacebuffswaiting++;
            }
         //   if (line.Contains("DEATHRATTLE value=1") && line.Contains("ProcessChanges()"))
           // {
             //   string[] a = getNameAndId(line);
               // Card c = minions[a[1]];
                //if (c.typeMatches(Card.Type.Murloc))
                //{
                  //  c.addEffect(new ArtificialSummon(CardCreatorFactory.Cards.Plant, 2));
                    //    Console.WriteLine("Recieved plant buff on " + nameIDString(line));
                    //Console.ReadLine();
               // }
           // }

            

            if (line.Contains("entityName="))
            {

                string[] a = getNameAndId(line);
                if (!minions.ContainsKey(a[1]))
                {
                    minions.Add(a[1],DrawableCard.makeDrawable(CardCreatorFactory.createFromName(a[0]).setId(int.Parse(a[1]))));
                }

            }

            if (line.Contains("tag=BACON_MINION_IS_LEVEL_TWO"))
            {



                string[] a = getNameAndId(line);
                DrawableCard c = minions[a[1]];
                if (c.golden)
                    return;
                c.makeGolden();
                Console.WriteLine("Found golden: " + nameIDString(line) + "!!!!");
            }

            if (combat && !waitingForStart)
            {
                if (line.Contains("TRANSITIONING") && line.Contains("entityName=Refresh") && line.Contains("FRIENDLY PLAY"))
                {
                    Console.WriteLine("COMBAT OVER!!!");
                    lookForAttacker();
                    board = new DrawableHearthstoneBoard();
                    combat = false;
                }
                if (line.Contains("entityName="))
                {

                    string[] a = getNameAndId(line);
                    if (!minions.ContainsKey(a[1]))
                    {
                        minions.Add(a[1], DrawableCard.makeDrawable(CardCreatorFactory.createFromName(a[0]).setId(int.Parse(a[1]))));
                    }

                }
                if (line.Contains("tag=COPIED_FROM_ENTITY_ID value=") && !line.Contains("tag=COPIED_FROM_ENTITY_ID value=0") && line.Contains("ProcessChanges()"))
                {
                    string newvalue = getStringBetween(line, "tag=COPIED_FROM_ENTITY_ID value=", " ");
                    Console.WriteLine(nameIDString(line)+ "getting copied over from "+ newvalue);
                    string[] a = getNameAndId(line);
                   minions[a[1]] =((DrawableCard) minions[newvalue].copy());

                }
                return;

            }


            if (line.Contains("zone from FRIENDLY PLAY -> ") && line[line.Length-2].Equals(' '))
            {
                string id = getNameAndId(line)[1];
                Console.WriteLine("Friendly minion sold:" + nameIDString(line));
                board.p1Board.Remove(minions[id]);
                //boardgui.redrawBoard(board);
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
            }

            if (line.Contains("TRANSITIONING card") && line.Contains("to FRIENDLY PLAY") && line[line.Length - 2].Equals('Y'))
            {
                string id = getNameAndId(line)[1];
                Console.WriteLine("Friendly minion added:" + nameIDString(line));
                string cardID = getStringBetween(line, "cardId=", " ");
                minions[id].pictureID = cardID;
                board.p1Board.Add(minions[id]);
                minions[id].printState();
             //   boardgui.redrawBoard(board);
            }

            if (line.Contains("TRANSITIONING card") && line.Contains("to OPPOSING PLAY") && line[line.Length - 2].Equals('Y') && combat)
            {
                string id = getNameAndId(line)[1];
                Console.WriteLine("Enemy minion added:" + getNameAndId(line)[0] + " id:" + id);
                if (lookForEntity(id))
                {
                    minions[id].makeGolden();
                }
                board.p2Board.Add(minions[id]);
                string cardID = getStringBetween(line, "cardId=", " ");
                minions[id].pictureID = cardID;
                minions[id].printState();
            }

            if (line.Contains("tag=PLAYER_TECH_LEVEL value=") && line.Contains("ProcessChanges"))
            {
                string newvalue = getStringBetween(line, "tag=PLAYER_TECH_LEVEL value=", " ");
                startRound(newvalue,getStringBetween(line,"name=","]"));
            }

            if (line.Contains("Thuzad") && line.Contains("OPPOSING PLAY (Hero)"))
            {
                Console.WriteLine("Kel'Thuzad detected");
                startRound("0","Kel'Thuzad");

            }

            if (line.Contains("TRANSITIONING") && line.Contains("entityName=Refresh") && line[line.Length - 2].Equals(' '))
            {
                Console.WriteLine("COMBAT STARTS!!!");
                //boardgui.disableButtons();
                waitingForStart = true;
                combat = true;
            }
     
            if (line.Contains("tag=ATK value=") && line.Contains("ProcessChanges"))
            {
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
              
                string newvalue = getStringBetween(line, "tag=ATK value=", " ");
                c.setAtk(int.Parse(newvalue));
                if (menacebuffswaiting >0)
                {
                    Console.WriteLine("Added menace buff on " + nameIDString(line));
                    for (int i = 0; i < menacebuffswaiting; i++)
                        c.addEffect(new ArtificialSummon(CardCreatorFactory.Cards.Microbot, 3));
                    menacebuffswaiting = 0;
                }

               if (board.containsCard(c))
                Console.WriteLine("Attack changed on "+nameIDString(line) +" to "+newvalue);


            }
            if (line.Contains("tag=HEALTH value=") && line.Contains("ProcessChanges"))
            {
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];

                string newvalue = getStringBetween(line, "tag=HEALTH value=", " ");
                c.setHp(int.Parse(newvalue));

                if (board.containsCard(c))
                    Console.WriteLine("Health changed on " + nameIDString(line) + " to " + newvalue);
            }
            if (line.Contains("tag=DIVINE_SHIELD value=1") && line.Contains("ProcessChanges"))
            {
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
                c.setDivineShield(true);
                if (board.containsCard(c))
                    Console.WriteLine("Recieved Divine Shield on " + nameIDString(line));
            }

            if (line.Contains("tag=POISONOUS value=1") && line.Contains("ProcessChanges"))
            {
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
                c.setPoisonous(true);
                if (board.containsCard(c))
                    Console.WriteLine("Recieved poisonous on " + nameIDString(line));
            }

            if (line.Contains("tag=WINDFURY value=1") && line.Contains("ProcessChanges"))
            {
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
                c.setWindfury(true);
                if (board.containsCard(c))
                    Console.WriteLine("Recieved Windfury on " + nameIDString(line));
            }

            if (line.Contains("tag=TAUNT value=1") && line.Contains("ProcessChanges"))
            {
                string[] a = getNameAndId(line);
                Card c = minions[a[1]];
                c.setTaunt(true);
                if (board.containsCard(c))
                    Console.WriteLine("Recieved taunt on " + nameIDString(line));
            }



        }

        public bool lookForEntity(string id)
        {
            if (powerData == null || powerPointer == -1)
            {
                powerData = powerReader.ReadToEnd().Split('\n');
                powerPointer = 0;
            }
            while (powerPointer < powerData.Length)
            {
                
                   
                if (powerData[powerPointer].Contains("Creating ID=" + id))
                {
             
                    powerPointer++;
                    while (powerData[powerPointer].Contains("   tag"))
                    {
                        if (powerData[powerPointer].Contains("BACON_MINION_IS_LEVEL_TWO"))
                        {
                            powerPointer++;
                            Console.WriteLine("golden minion found");
                            return true;
                        }
                        powerPointer++;
                    
                    }
                  
                    return false;
                }
                powerPointer++;
            }
            powerPointer = -1;
            return lookForEntity(id);
        }

        public void lookForEffects()
        {
            if (powerData == null || powerPointer == -1)
            {
                powerData = powerReader.ReadToEnd().Split('\n');
                powerPointer = 0;

            }
            while (powerPointer < powerData.Length)
            {
                if (powerData[powerPointer].Contains("BLOCK_START BlockType=ATTACK"))
                {
                    powerPointer++;
                    return;
                }
                if (powerData[powerPointer].Contains("tag=REBORN value=1") && powerData[powerPointer].Contains("entityName"))
                {
                    string id = getNameAndId(powerData[powerPointer])[1];

                    Effect e = new Reborn(minions[id].golden);
                    if (minions[id].hasEffect(e))
                    {
                        minions[id].addEffect(e);
                        Console.WriteLine("reborn found on " + nameIDString(powerData[powerPointer]));
                    }
                }
                powerPointer++;
            }
            powerPointer = -1;
            lookForAttacker();
   
        }

        public bool lookForAttacker()
        {
            if (powerData == null || powerPointer == -1)
            {
                powerData = powerReader.ReadToEnd().Split('\n');
                powerPointer = 0;

            }
            while (powerPointer < powerData.Length)
            {
                if (powerData[powerPointer].Contains("tag=REBORN value=1") && powerData[powerPointer].Contains("entityName"))
                {
                    Console.WriteLine("reborn found" + nameIDString(powerData[powerPointer]));
                    Console.ReadLine();
                }
                if (powerData[powerPointer].Contains("tag=ATTACKING value=1") && powerData[powerPointer].Contains("cardId=TB_BaconShop_HERO") && powerData[powerPointer].Contains("GameState"))
                {
                    string[] name = getNameAndId(powerData[powerPointer]);
                    Console.WriteLine("Attacking hero: "+name[0]);
                    if (name[1].Equals(heroId))
                    {

                        if (expectedLoss)
                        {
                            Console.WriteLine("Lucky win ;)");
                            luckywins++;
                        }
                        else
                            Console.WriteLine("WIN!!");
                    }
                    else
                    {
                        if (!expectedLoss)
                        {
                            Console.WriteLine("Unlucky loss ;)");
                            unluckyloss++;
                        }
                        else
                            Console.WriteLine("Loss!!");
                    }
                    powerPointer++;
                    gui.setLucks(luckywins, unluckyloss);
                    return true;
                }

                if (powerData[powerPointer].Contains("GameEntity tag=BOARD_VISUAL_STATE value=1"))
                {
                    Console.WriteLine("No attacker? Draw i guess");
                    if (expectedLoss)
                    {
                        luckywins++;
                        Console.WriteLine("Lucky draw ;)");
                    }
                    powerPointer++;
                    return true;
                }

              


                powerPointer++;
            }
            powerPointer = -1;
            return lookForAttacker();
        }

        public void startRound(string newvalue, string opponent)
        {
      
            if (waitingForStart)
            {
                Console.WriteLine("Finished building enemy board. (tavern tier: " + newvalue + ")");
                lookForEffects();
                Card[] cards = new Card[board.p1Board.Count];
                foreach (Card c in board.p1Board)
                    cards[c.BoardPosition - 1] = c;
                board.p1Board = new BoardSide();
                board.p1Board.tavernTier = taverntier;
                board.p2Board.tavernTier = int.Parse(newvalue);
                for (int i = 0; i < cards.Length; i++)
                    board.p1Board.Add(cards[i]);
                board.printState();
                board.recievedRandomValues = new List<int>();
                //boardgui.redrawBoard(board);
                board.makeUpForReaderError();
                
                List<HearthstoneBoard> res = new List<HearthstoneBoard>();
                try
                {
                   res = board.simulateResults(simulationCount);
                } catch(ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne e)
                {
                    Console.WriteLine("Exception encountered on board simulation: " + e.Message);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    waitingForStart = false;
                    while (pause)
                        Thread.Sleep(100);
                    return;
                }
                while (pause)
                    Thread.Sleep(100);
                //Calculate and print all damage distribution percentages
                var dmgdist = StatisticsManager.calculateDmgDistributions(res);
                StatisticsManager.printReadableResult(dmgdist);

                //Calculate and print win/loss chances
                var dmgdist2 = StatisticsManager.calculateAverageWins(res);
                StatisticsManager.printReadableResult(dmgdist2);

                while (gui == null)
                    Thread.Sleep(100);

                //Draw charts and set win/loss label text in GUI
                gui.drawChart(dmgdist);
                gui.setLabels(dmgdist2, opponent);

                //Find worst and best boards and set them in GUI
                var wAndB = StatisticsManager.findWorstAndBest(res);
                gui.setWorstAndBest(board,wAndB[1].recievedRandomValues, wAndB[0].recievedRandomValues);

                //Calculate most likely game damage outcome and set expected health
                int likelydmg = StatisticsManager.mostLikelyOutcome(dmgdist);
                if (likelydmg < 0)
                {
                    expectedHealth += likelydmg;
                    gui.setExpected(expectedHealth);
                }

                //Set expected outcome, used for lucky win/unlucky loss
                if (dmgdist2[2] > 0.5)
                    expectedLoss = true;
                else
                    expectedLoss = false;
                StatisticsManager.printExpectedResult(dmgdist2);
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

        public Dictionary<string, string> settings = new Dictionary<string, string>();
        public static void Run()
        {
            if (!File.Exists("Settings.txt"))
                File.Create("Settings.txt").Close();
            var sets = new Dictionary<string, string>();
            using (FileStream stream = File.Open("Settings.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string text = reader.ReadToEnd();
                    string[] tokens = text.Split('\n');

                    for (int i = 0; i < tokens.Length; i++)
                        if (tokens[i].Contains("=")) {
                            string value = tokens[i].Split('=')[1];
                            sets.Add(tokens[i].Split('=')[0], value.Remove(value.Length-1));
                        }
                }
            }

        var obj = new BoardStateReader(sets);
            Thread thr = new Thread(new ThreadStart(obj.readInput));
            thr.Start();
         
        

//            Thread thr3 = new Thread(new ThreadStart(obj.FormRun2));
  //          thr3.SetApartmentState(ApartmentState.STA);
    //        thr3.Start();

            obj.run();
            // new BoardStateReader().run();
        }
        public void FormRun()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            gui = new GUI(this);
            Application.Run(gui);
        }
        public void FormRun2()
        {
          //  Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //boardgui = new BoardGUI();
            //Application.Run(boardgui);
        }


    }
}
