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
        public const string StrengthID = "StrengthSim";
        public const string ChartType = "Chart";
        public const string SaveBoards = "SaveBoard";

        public class HeroNames
        {
            public const string Illidan = "Illidan Stormrage";
            public const string Patchwerk = "Patchwerk";
            public const string Deathwing = "Deathwing";
            public const string Bob = "BaconPHhero";
            public const string Finley = "Sir Finley Mrrgglton";
            public const string KelThuzad = "Kel'Thuzad";
            public const string Nefarian = "Nefarian";

        }
        public class HeroPower
        {
            public const string Deathwing = "ALL Will Burn!";
            public const string Illidan = "Wingmen";
            public const string Patchwerk = "All Patched Up";
            public const string Nefarian = "Nefarious Fire";
            
        }
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
        public int strengthSimuls = 2000;
        int powerPointer;
        GUI gui;
        //BoardGUI boardgui;
        public bool pause = false;
        public bool initialized = false;
        public bool playerIsIllidan = false;
        public bool playerIsDW = false;
        public bool saveBoards = true;
        public Dictionary<string, DrawableCard> minions = new Dictionary<string, DrawableCard>();
        public int round;
        DrawableHearthstoneBoard board = new DrawableHearthstoneBoard();
        int menacebuffswaiting = 0;
        string heroId = "null";
        string heroName = "";
        string playerHeroPwr = "";
        string opponentHeroPWR = "";
        bool expectedLoss;
        int luckywins = 0;
        int unluckyloss = 0;
        int expectedHealth = 40;
        int actualHealth = 40;
        int maxHealth = 40;
        public void initializeSettings()
        {
            if (settings.ContainsKey(PathID))
                path = settings[PathID];
            if (settings.ContainsKey(SimulationID))
                simulationCount = int.Parse(settings[SimulationID]);
            if (settings.ContainsKey(StrengthID))
                strengthSimuls = int.Parse(settings[StrengthID]);
            if (settings.ContainsKey(ChartType) && gui != null)
                gui.setChartType(int.Parse(settings[ChartType]));
            else if (!settings.ContainsKey(ChartType))
                settings.Add(ChartType, "0");
            if (settings.ContainsKey(SaveBoards))
                if (settings[SaveBoards].Equals("0"))
                    saveBoards = false;
                else if (settings[SaveBoards].Equals("1"))
                    saveBoards = true;
        }
        const string _Version = "1.0";
        public void run()
        {
            initializeSettings();
            Console.WriteLine("Welcome to Battleground Simulator v"+_Version+" :)");
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Blizzard\Hearthstone\log.config";
            //If log.config file does not exist. Attempt to create it
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
            //If path to log files is not set, prompt it
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
                //Try to start reading from log files, if error occurs, ask for new path.
                try
                {
                    runLoop(true);
                } catch (FileNotFoundException)
                {
                    Console.WriteLine("Cannot find log files, make sure that the path is correctly set");
                    Console.WriteLine("Change path? (y/n)");
                    string ans = Console.ReadLine();
                    if (ans.Equals("y") || ans.Equals("yes"))
                        demandPath();

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

        //Opens up file dialog and asks user for where hearthstone directory is
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

        public void runLoop(bool firstTime)
        {
            using (FileStream stream = File.Open(path + "Zone.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    s2 = File.Open(path + "Power.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    powerReader = new StreamReader(s2);
                    string firstData = reader.ReadToEnd();
                    if (!firstData.Equals("") && firstTime)
                    {
                        Console.WriteLine("Already existing game information detected, simulate on the existing data? (y/n)");
                        while (true)
                        {
                           string ans = Console.ReadLine();
                            if (ans.Equals("y") || ans.Equals("yes"))
                            {
                                startGUI();
                                int temp = simulationCount;
                                int temp2 = strengthSimuls;
                               simulationCount = 10;
                                strengthSimuls = 100;
                                string[] lineses = firstData.Split('\n');
                                    for (int i = 0; i < lineses.Length; i++)
                                        handleInput(lineses[i]);
                                simulationCount = temp;
                                strengthSimuls = temp;
                                
                                break;
                            }
                            else if (ans.Equals("n") || ans.Equals("no"))
                            {
                                startGUI();
                                powerReader.ReadToEnd();
                                break;
                            }

                        }
                    }
                    else if (firstTime){ startGUI(); }

                    long lastLength = 0;
                    while (true)
                    {
                        while (pause)
                            Thread.Sleep(100);
                     //   Console.WriteLine("Before read..");
                        FileInfo fi = new FileInfo(path + "Zone.log");
                        if (fi.Length < lastLength)
                        {
                            Console.WriteLine("File size changed, restarting reader");
                            lastLength = 0;
                            runLoop(false);

                        }
                        lastLength = fi.Length;
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
                if (settings.ContainsKey(StrengthID))
                    sw.WriteLine(StrengthID + "=" + settings[StrengthID]);
                if (settings.ContainsKey(ChartType))
                    sw.WriteLine(ChartType + "=" + settings[ChartType]);
                if (settings.ContainsKey(SaveBoards))
                    sw.WriteLine(SaveBoards + "=" + settings[SaveBoards]);
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
            actualHealth = 40;
            maxHealth = 40;
            board = new DrawableHearthstoneBoard();
           combat = false;
           waitingForStart = false;
            minions = new Dictionary<string, DrawableCard>();
            menacebuffswaiting = 0;
            taverntier = 1;
            luckywins = 0;
            unluckyloss = 0;
            round = 0;
            opponentHeroPWR = "";
            playerHeroPwr = "";
            if (gui != null)
                gui.resetState();
            playerIsIllidan = false;
            playerIsDW = false;
        }

        public void handleInput(string line)
        {
            if (line.Equals(""))
                return;
            line = line.Substring(0, line.Length - 1) + "|";

            

            if (line.Contains("TRANSITIONING card") && line.Contains("to FRIENDLY PLAY (Hero)"))
            {
                string name = getNameAndId(line)[0];
                if (name.Equals(HeroNames.Bob))
                    return;
                string id = getNameAndId(line)[1];
                Console.WriteLine("Hero Discovered:" + nameIDString(line)+ " initializing new game");
                heroId = id;
                heroName = getNameAndId(line)[0];
                startNewGame();

            }
            if (line.Contains("TRANSITIONING") && line.Contains("FRIENDLY PLAY (Hero Power)"))
            {

                string name = getNameAndId(line)[0];
                switch (name)
                {
                    case HeroPower.Deathwing: Console.WriteLine("Hero power discovered: deathwing"); playerIsDW = true;  break;
                    case HeroPower.Illidan: Console.WriteLine("Hero power discovered: illidan"); playerIsIllidan = true; break;
                    case HeroPower.Patchwerk: Console.WriteLine("Hero power discovered: patchwerk"); expectedHealth = 50; actualHealth = 50; maxHealth = 50; break;
                }
                playerHeroPwr = name;
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
                    minions.Add(a[1], DrawableCard.makeDrawable(CardCreatorFactory.createFromName(a[0]).setId(int.Parse(a[1]))));
                }
                else
                    checkIfNameMatches(a);

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
                    Console.WriteLine(nameIDString(line) + "getting copied over from " + newvalue);
                    string[] a = getNameAndId(line);
                    if (minions.ContainsKey(newvalue))
                        minions[a[1]] = ((DrawableCard)minions[newvalue].copy().setId(int.Parse(a[1])));

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
                checkIfNameMatches(getNameAndId(line));
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
                if (board.p1Board.Contains(minions[id]))
                    return;
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
                startRound(newvalue,getStringBetween(line,"name=","]"), opponentHeroPWR);
            }

            if (line.Contains("Thuzad") && line.Contains("OPPOSING PLAY (Hero)"))
            {
                Console.WriteLine("Kel'Thuzad detected");
                startRound("0",HeroNames.KelThuzad,"");

            }
            if (line.Contains("TRANSITIONING") && line.Contains("OPPOSING PLAY (Hero Power)"))
            {
              
                string name = getNameAndId(line)[0];
                switch (name)
                {
                    case HeroPower.Deathwing: Console.WriteLine("Hero power discovered: deathwing"); break;
                    case HeroPower.Illidan: Console.WriteLine("Hero power discovered: illidan"); break;
                }
                opponentHeroPWR = name;
            }
            /* if (line.Contains("tag=COPIED_FROM_ENTITY_ID value=") && !line.Contains("tag=COPIED_FROM_ENTITY_ID value=0") && line.Contains("ProcessChanges()"))
             {
                 string newvalue = getStringBetween(line, "tag=COPIED_FROM_ENTITY_ID value=", " ");
                 Console.WriteLine(nameIDString(line) + "getting copied over from " + newvalue);
                 string[] a = getNameAndId(line);
                 if (minions.ContainsKey(newvalue))
                     minions[a[1]] = ((DrawableCard)minions[newvalue].copy());
                 Console.ReadLine();

             }*/

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
                checkIfNameMatches(a);
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
        //Check if stored name matches name from line. If neq, minion has been transformed -> save the new minion instead
        public void checkIfNameMatches(string[] nameandid)
        {
            string id = nameandid[1];
            string newName = nameandid[0];
            DrawableCard c = minions[id];
            if (c.getName().Equals("UnknownCard"))
                return;
           
            string oldName = c.getName();
            if (!oldName.Equals(newName))
            {
                int temp = c.BoardPosition;
                minions[id] = DrawableCard.makeDrawable(CardCreatorFactory.createFromName(newName).setId(int.Parse(id)));
                minions[id].BoardPosition = temp;
                if (board.p1Board.Contains(c))
                {
                    board.p1Board.Remove(c);
                    board.p1Board.Add(minions[id]);
                }
                Console.WriteLine("Minion changed: "+oldName + " to " + minions[id].getReadableName());
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
                /*    if (powerData[powerPointer].Contains("TAG_CHANGE Entity=[") && powerData[powerPointer].Contains("tag=ATK value="))
                    {

                        string[] a = getNameAndId(powerData[powerPointer]);

                        if (a[0].Length < 2|| !minions.ContainsKey(a[1]))
                            continue;
                        Card c = minions[a[1]];


                        string newvalue = getStringBetween(powerData[powerPointer], "tag=ATK value=", " ");

                        c.setAtk(int.Parse(newvalue));

                        if (board.containsCard(minions[a[1]]))
                            Console.WriteLine("atk change on " + nameIDString(powerData[powerPointer]) +" to "+newvalue);
                    }*/

                if (powerData[powerPointer].Contains("tag=DIVINE_SHIELD value=1") && powerData[powerPointer].Contains("entityName="))
                {
                    string[] a = getNameAndId(powerData[powerPointer]);
                    if (minions.ContainsKey(a[1]) && !minions[a[1]].divineShield)
                    {
                        minions[a[1]].setDivineShield(true);
                        Console.WriteLine("Recieved Divine Shield on " + nameIDString(powerData[powerPointer]));
                    }
                }
                if (powerData[powerPointer].Contains("tag=REBORN value=1") && powerData[powerPointer].Contains("entityName="))
                {
                    string[] a = getNameAndId(powerData[powerPointer]);
                    if (minions.ContainsKey(a[1]))
                    {
                        Effect e = new Reborn(minions[a[1]].golden);
                        if (!minions[a[1]].hasEffect(e))
                        {
                            minions[a[1]].addEffect(e);
                            minions[a[1]].tempReborn = true;
                            Console.WriteLine("Recieved temporary Reborn on " + nameIDString(powerData[powerPointer]));
                        }
                      
                    }
                }



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
                //Look for reborn on enemy minions. Should only be true for lich king hero power
                if (powerData[powerPointer].Contains("tag=REBORN value=1") && powerData[powerPointer].Contains("entityName"))
                {
                    string id = getNameAndId(powerData[powerPointer])[1];
                    Effect e = new Reborn(minions[id].golden);
                    if (!minions[id].hasEffect(e))
                    {
                        minions[id].addEffect(e);
                        Console.WriteLine("reborn found on " + nameIDString(powerData[powerPointer]));
                    }
                }
                //Look for nefarian hero power. TODO check if this works for friendly hero
                if (powerData[powerPointer].Contains("BlockType=TRIGGER Entity=[entityName=Nefarious Fire id="))
                {
                   
                        Console.WriteLine("Nefarian hero power detected");
                    if (playerHeroPwr.Equals(HeroPower.Nefarian))
                        board.NefarianPlayer = 1;
                    else
                        board.NefarianPlayer = 2;
                }
                
                  powerPointer++;
            }
            powerPointer = -1;
            lookForEffects();
   
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
                if (powerData[powerPointer].Contains("tag=ATTACKING value=1") && powerData[powerPointer].Contains("cardId=TB_BaconShop_HERO") && powerData[powerPointer].Contains("GameState"))
                {
                    string[] name = getNameAndId(powerData[powerPointer]);
                    Console.WriteLine("Attacking hero: "+name[0]);
                    if (name[1].Equals(heroId))
                        if (expectedLoss)
                        {
                            Console.WriteLine("Lucky win ;)");
                            luckywins++;
                        }
                        else
                            Console.WriteLine("WIN!!");
                    else
                    {
                        while (!(powerData[powerPointer].Contains("entityName=") && powerData[powerPointer].Contains("tag=DAMAGE value=")))
                            powerPointer++;
                        string dmgtaken = getStringBetween(powerData[powerPointer], "tag=DAMAGE value=", " ");
                        Console.WriteLine("TAKEN DAMAGE: " +dmgtaken );
                        actualHealth = maxHealth - int.Parse(dmgtaken);
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
        public void startGUI()
        {
            initialized = true;
            Thread thr2 = new Thread(new ThreadStart(FormRun));
            thr2.SetApartmentState(ApartmentState.STA);
            thr2.Start();
        }
        public void startRound(string newvalue, string opponent, string opponentHeroPWR)
        {
      
            if (waitingForStart)
            {
                round++;
                Console.WriteLine("Finished building enemy board. (tavern tier: " + newvalue + ")");
                //Looking for effects in power file. Like reborn buffs
                lookForEffects();

                //Set up board using the correct positions
                Card[] cards = new Card[board.p1Board.Count];
                foreach (Card c in board.p1Board)
                    cards[c.BoardPosition - 1] = c;
                board.p1Board = new BoardSide();
                for (int i = 0; i < cards.Length; i++)
                    if (cards[i] != null)
                        board.p1Board.Add(cards[i]);

                board.p1Board.tavernTier = taverntier;
                board.p2Board.heroOwner = opponentHeroPWR;
                board.p2Board.tavernTier = int.Parse(newvalue);
                
                //Extra hack for special hero abilities
                if (playerIsIllidan)
                    board.illidanPlayer = 1;
                if (opponentHeroPWR.Equals(HeroPower.Illidan))
                    board.illidanPlayer = 2;
                if (playerIsDW)
                    board.DeathwingPlayer = 1;
                if (opponentHeroPWR.Equals(HeroPower.Deathwing))
                    board.DeathwingPlayer = 2;

                board.printState();
                board.recievedRandomValues = new List<int>();
                board.makeUpForReaderError();

                //Save opponent boardstate to disk
                if (saveBoards && !opponent.Equals("Kel'Thuzad"))
                     BoardEditor.autoSave(board.p2Board, round);
                List<HearthstoneBoard> res = new List<HearthstoneBoard>();
                double ranking = -1;
                try
                {
                    Console.Write("Simulating board results ");
                    Thread t = new Thread(() => res = board.simulateResults(simulationCount));
                    t.Start();
                    while (board.currentProgression < board.currentProgressionMax)
                    {
                        if (board.currentProgressionMax != simulationCount)
                            board.currentProgressionMax = simulationCount;
                        Console.SetCursorPosition(25, Console.CursorTop);
                        Console.Write(board.currentProgression+"/"+board.currentProgressionMax+ " ("+Math.Round(((double)board.currentProgression*100/(double)board.currentProgressionMax),0)+"%)                 ");
                        Thread.Sleep(100);
                      
                    }
                    Console.SetCursorPosition(25, Console.CursorTop);
                    Console.Write(board.currentProgression + "/" + board.currentProgressionMax + " (100%)                ");
                    Console.WriteLine();

                    var roundBoards = BoardEditor.loadAllRoundSides(round);

                    if (!(roundBoards.Count == 0))
                    {
                        int totalSims = strengthSimuls;
                        int eachSim = strengthSimuls / roundBoards.Count;
                        int totalfinished = 0;
                        var sims = new List<HearthstoneBoard>();
                        int score = roundBoards.Count;
                        int[] scoreChanges = new int[] { 0, 1, -1 };
                        Console.Write("Simulating ranking results ");
                        foreach (BoardSide b in roundBoards)
                        {
                            HearthstoneBoard h = new HearthstoneBoard();
                            h.p1Board = board.p1Board.copy();
                            h.p2Board = b.copy();
                            switch (b.heroOwner)
                            {
                                case HeroNames.Nefarian: case HeroPower.Nefarian:
                                    h.NefarianPlayer = 2; break;
                                case HeroNames.Illidan: case HeroPower.Illidan:
                                    h.illidanPlayer = 2; break;
                                case HeroNames.Deathwing: case HeroPower.Deathwing:
                                    h.DeathwingPlayer = 2; break;
                            }
                            if (playerIsIllidan)
                                h.illidanPlayer = 1;
                            if (playerIsDW)
                                h.DeathwingPlayer = 1;
                            Thread t2 = new Thread(() => sims = h.simulateResults(eachSim));
                            t2.Start();
                            while (h.currentProgression < h.currentProgressionMax)
                            {
                                if (totalSims != strengthSimuls)
                                {
                                    totalSims = strengthSimuls;
                                    eachSim = strengthSimuls / roundBoards.Count;
                                    h.currentProgressionMax = eachSim;
                                }
                                Console.SetCursorPosition(27, Console.CursorTop);
                                Console.Write(totalfinished+h.currentProgression + "/" + totalSims + " (" + Math.Round(((double)(totalfinished + h.currentProgression) * 100 / (double)totalSims), 0) + "%)                 ");
                                Thread.Sleep(100);
                            }
                            score += scoreChanges[StatisticsManager.expectedResult(StatisticsManager.calculateAverageWins(sims))];
                            totalfinished += eachSim;
                        }
                        ranking = (double)score / (double)(roundBoards.Count * 2);
                        ranking = Math.Round(ranking * 100, 1);
                        Console.WriteLine();

                    }
                 


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

                //If gui has not been initilized then wait
                while (gui == null)
                    Thread.Sleep(100);                //Find worst and best boards
                var wAndB = StatisticsManager.findWorstAndBest(res);
                
                //Calculate most likely game damage outcome and set expected health
                int likelydmg = StatisticsManager.mostLikelyOutcome(dmgdist);
                if (likelydmg < 0)
                {
                    expectedHealth += likelydmg;
                }

                //Set expected outcome, used for lucky win/unlucky loss
                if (dmgdist2[2] > 0.5)
                    expectedLoss = true;
                else
                    expectedLoss = false;
                StatisticsManager.printExpectedResult(dmgdist2);
                waitingForStart = false;

                //Calculate the chance to die
                double deathchance = StatisticsManager.chanceForDamageOrMore(-actualHealth, dmgdist);

                //Update the GUI to the new boardstate
                GUIstate currentState = new GUIstate(dmgdist, dmgdist2, board, wAndB[1].recievedRandomValues, wAndB[0].recievedRandomValues,expectedHealth,opponent,deathchance,ranking);
                gui.addGUIstate(currentState);

                //Remove temporary reborn buffs (Lich King)
                foreach (Card c in board.p1Board)
                    if (c.tempReborn)
                        c.removeReborn();
            }
        }

        public string nameIDString(string line)
        {
            string[] a = getNameAndId(line);
            return a[0] + " id: " + a[1];
        }

        /*
         * Extract name and id from a line in the log file [0] is name [1] is id
         * */
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
            Directory.CreateDirectory("Boardsides");
            var sets = new Dictionary<string, string>();

            //parse the settings file and save them in a list
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
            obj.run();
        }

        //Method used in thread that starts up gui
        public void FormRun()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            gui = new GUI(this);
            Application.Run(gui);
        }


    }
}
