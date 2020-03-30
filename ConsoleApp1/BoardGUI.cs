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
    public partial class BoardGUI : Form
    {
        public BoardGUI()
        {
            InitializeComponent();
        }

        private void BoardGUI_Load(object sender, EventArgs e)
        {

        }

        public void redrawBoard(DrawableHearthstoneBoard board)
        {
            this.Invoke((MethodInvoker)delegate {
                board.drawBoardState(new Point(50, 50), new Point(50, 300), this);
            });
        }
        DrawableHearthstoneBoard lastBoard;
        List<int> worstValues = new List<int>();
        List<int> bestValues = new List<int>();
        public void setWorstAndBest(DrawableHearthstoneBoard board, List<int> worst, List<int> best)
        {

            worstValues = worst;
            lastBoard = board.copy() ;
            bestValues = best;
            this.button1.Invoke((MethodInvoker)delegate {
                button1.Enabled = true;
            });
            this.button2.Invoke((MethodInvoker)delegate {
                button2.Enabled = true;
            });
        }

        public void disableButtons()
        {
            this.button1.Invoke((MethodInvoker)delegate {
                button1.Enabled = false;
            });
            this.button2.Invoke((MethodInvoker)delegate {
                button2.Enabled = false;
            });
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(FormRun));
            t.Start();
        }
        public void FormRun()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (int i in worstValues)
            lastBoard.stockedRandomValues.Add(i);
            foreach (int i in lastBoard.stockedRandomValues)
                Console.WriteLine(i);
            var board = new VisualBoard(lastBoard);
            Application.Run(board);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(FormRunBest));
            t.Start();

        }
        public void FormRunBest()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (int i in bestValues)
                lastBoard.stockedRandomValues.Add(i);
            foreach (int i in lastBoard.stockedRandomValues)
                Console.WriteLine(i);
            var board = new VisualBoard(lastBoard);
            Application.Run(board);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BoardEditor.saveBoard(lastBoard);

        }

    }
}


