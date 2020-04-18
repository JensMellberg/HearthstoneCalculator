using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public partial class BoardEditor : Form
    {
        public BoardEditor()
        {
            InitializeComponent();
        }

        private void BoardEditor_Load(object sender, EventArgs e)
        {

        }

        DrawableHearthstoneBoard board;
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BoardEditor());
        }
        public static void saveBoard(DrawableHearthstoneBoard board)
        {
            DrawableHearthstoneBoard save = board.copy();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Board State|*.boardstate";
            saveFileDialog1.Title = "Save a Board state";
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs =
                    (FileStream)saveFileDialog1.OpenFile();


                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(fs, save);
                fs.Close();
            }
        }
        public static void autoSave(BoardSide b, int round)
        {
            BoardSide save = b.copy();
            IFormatter formatter = new BinaryFormatter();
            string directory = @"BoardSides\Round" + round;
            Directory.CreateDirectory(directory);
            string filename = directory + @"\board.boardside";
            int suffix = 1;
            while (File.Exists(filename))
            {
                suffix++;
                filename = directory + @"\board" + suffix + ".boardside";
            }

            //Dont save board if its already saved
            BoardSide lastBoard = null;
            if (suffix == 2)
                lastBoard = loadBoardSide(directory + @"\board.boardside");
            else if (suffix != 1)
                lastBoard = loadBoardSide(directory + @"\board" + (suffix - 1) + ".boardside");
            if (lastBoard != null && lastBoard.Compare(b, null, null))
                return;
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, b);
            stream.Close();

        }
        public static List<BoardSide> loadAllRoundSides(int round)
        {
            List<BoardSide> sides = new List<BoardSide>();
            string directory = @"BoardSides\Round" + round;
            Directory.CreateDirectory(directory);
            string filename = directory + @"\board.boardside";
            int suffix = 1;
            while (File.Exists(filename))
            {
                sides.Add(loadBoardSide(filename));
                suffix++;
                filename = directory + @"\board" + suffix + ".boardside";
            }
            return sides;



        }
        public static BoardSide loadBoardSide(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            IFormatter formatter = new BinaryFormatter();
            BoardSide objnew = (BoardSide)formatter.Deserialize(stream);
            return objnew;
        }
        public static DrawableHearthstoneBoard loadBoard(string path)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = path;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Load a Board state";
            openFileDialog1.Filter = "Board State|*.boardstate";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.ShowDialog();


            if (openFileDialog1.FileName != "")
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                IFormatter formatter = new BinaryFormatter();
                DrawableHearthstoneBoard ret = (DrawableHearthstoneBoard)formatter.Deserialize(fs);

                ret.printState();

                return ret;
            }
            return null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawableHearthstoneBoard temp = BoardEditor.loadBoard(Application.StartupPath);
            if (temp == null)
                return;
            board = temp;
            board.drawBoardState(new Point(50, 50), new Point(50, 300), this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BoardEditor.saveBoard(board);
        }
        List<int> worstValues = new List<int>();
        List<int> bestValues = new List<int>();
        private void button2_Click(object sender, EventArgs e)
        {
            List<HearthstoneBoard> res = new List<HearthstoneBoard>();
            int sims = 0;
            try
            {
                sims = int.Parse(textBox1.Text);
            } catch (Exception) { MessageBox.Show("Simulations must be a number"); return; }
            try
            {
                res = board.simulateResults(sims);
            }
            catch (ExceptionWithMessageWhyDoesntCSharpHaveItDeafaultComeOne ex)
            {
                MessageBox.Show("Exception encountered on board simulation: " + ex.Message);
                return;
            }

            var dmgdist = StatisticsManager.calculateDmgDistributions(res);
            StatisticsManager.printReadableResult(dmgdist);

            var dmgdist2 = StatisticsManager.calculateAverageWins(res);
            StatisticsManager.printReadableResult(dmgdist2);
            GUI gui = new GUI(null);
            gui.hideRealTimeLabels();
            gui.Show();
            
            gui.drawChart(dmgdist);
            gui.setLabels(dmgdist2, "");
            var wAndB = StatisticsManager.findWorstAndBest(res);
            worstValues = wAndB[1].recievedRandomValues;
            bestValues = wAndB[0].recievedRandomValues;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(formRun));
            t.Start();
        }
        void formRun()
        {
            DrawableHearthstoneBoard runBoard = board.copy();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (int i in bestValues)
                runBoard.stockedRandomValues.Add(i);
            var boardV = new VisualBoard(runBoard);
            Application.Run(boardV);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(formRun2));
            t.Start();
        }
        void formRun2()
        {
            DrawableHearthstoneBoard runBoard = board.copy();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (int i in worstValues)
                runBoard.stockedRandomValues.Add(i);
            var boardV = new VisualBoard(runBoard);
            Application.Run(boardV);
        }

        void formRun3()
        {
            DrawableHearthstoneBoard runBoard = board.copy();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var boardV = new VisualBoard(runBoard);
            Application.Run(boardV);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(formRun3));
            t.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(typeof(CardCreatorFactory.Cards));
            List<string> values = new List<string>();
            foreach (var attr in typeof(CardCreatorFactory.Cards).GetFields())
            {
                values.Add((string)attr.GetValue(""));
            }
            values.Sort();
            foreach (string s in values)
                Console.WriteLine(s);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            board = new DrawableHearthstoneBoard();
            board.p1Board.Add(DrawableCard.makeDrawable(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DragonspawnLieutenant)));
            board.p1Board.Add(DrawableCard.makeDrawable(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DragonspawnLieutenant)));
     //       board.p1Board.Add(DrawableCard.makeDrawable(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.DragonspawnLieutenant)));
            board.p2Board.Add(DrawableCard.makeDrawable(CardCreatorFactory.createFromName(CardCreatorFactory.Cards.MalGanis)));
            board.illidanPlayer = 1;
            board.drawBoardState(new Point(50, 50), new Point(50, 300), this);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int from = 0;
            int to = 0;
            try
            {
                from = int.Parse(textBox3.Text)-1;
                to = int.Parse(textBox4.Text)-1;
                if (from > 7 || from < 1 || to > 7 || to < 1)
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Please only use numbers between 1 and 7");
                return;
            }
            DrawableCard c1 = (DrawableCard)board.p1Board[from];
            DrawableCard c2 = (DrawableCard)board.p1Board[to];
            board.p1Board[from] = c2;
            board.p1Board[to] = c1;
            board.drawBoardState(new Point(50, 50), new Point(50, 300), this);
        }
    }
}
