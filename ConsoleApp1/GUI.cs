using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ConsoleApp1
{
    public partial class GUI : Form
    {
        public GUI(BoardStateReader parent)
        {

            InitializeComponent();
            this.parent = parent;
        }
        BoardStateReader parent;
        bool loaded = false;
        private void GUI_Load(object sender, EventArgs e)
        {
            loaded = true;
            chart1.Series["Series1"].Name = "Damage distribution";
        }

        public void waitForWindowLoad()
        {
            while (!loaded)
                Thread.Sleep(100);
        }
        List<StatisticsManager.DmgDistEntry> points;

        public void drawChart(List<StatisticsManager.DmgDistEntry> points)
        {
            this.points = points;
            waitForWindowLoad();
            this.chart1.Invoke((MethodInvoker)delegate {
                chart1.Series["Damage distribution"].Points.Clear();
                foreach (var p in points)
                {
                    chart1.Series["Damage distribution"].Points.AddXY(p.damage, p.percentage);
                }
            });
        }
        public void setLucks(int win, int loss)
        {
            this.label4.Invoke((MethodInvoker)delegate {
                label4.Text = "Lucky wins: " + win;
            });

            this.label5.Invoke((MethodInvoker)delegate {
                label5.Text = "Unlucky losses: " + loss;
            });
        }
        public void setExpected(int health)
        {
            this.label6.Invoke((MethodInvoker)delegate {
                label6.Text = "Expected hp: " + health;
                if (health <= 0)
                    label6.Text = "Expected hp: DEAD";
            });
        }

        public void hideRealTimeLabels()
        {
            label4.Hide();
            label5.Hide();
            label6.Hide();
            button1.Hide();
            button2.Hide();
            button3.Hide();
            label7.Hide();
            label8.Hide();
            pictureBox1.Hide();
        }
        DrawableHearthstoneBoard lastBoard;
        List<int> worstValues = new List<int>();
        List<int> bestValues = new List<int>();
        public void setWorstAndBest(DrawableHearthstoneBoard board, List<int> worst, List<int> best)
        {

            worstValues = worst;
            lastBoard = board.copy();
            bestValues = best;
            this.button1.Invoke((MethodInvoker)delegate {
                button1.Enabled = true;
            });
            this.button2.Invoke((MethodInvoker)delegate {
                button2.Enabled = true;
            });
            this.button3.Invoke((MethodInvoker)delegate {
                button3.Enabled = true;
            });
        }


        public void setLabels(double[] avrgs, string opponent)
        {
            this.label2.Invoke((MethodInvoker)delegate {
                label2.Text = "Draw Chance: " + Math.Round(avrgs[0] * 100, 1) + "%";
            });
            this.label1.Invoke((MethodInvoker)delegate {
                label1.Text = "Win Chance: " + Math.Round(avrgs[1] * 100, 1) + "%";
            });
            this.label3.Invoke((MethodInvoker)delegate {
                label3.Text = "Loss Chance: " + Math.Round(avrgs[2] * 100, 1) + "%";
            });
            this.label7.Invoke((MethodInvoker)delegate {
                label7.Text = "Opponent: " + opponent;
            });
            this.label8.Invoke((MethodInvoker)delegate {
                var expect = StatisticsManager.expectedResultAndColor(avrgs);
                label8.Text = "Expected: " + expect.result;
                label8.ForeColor = expect.color;
            });
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread myNewThread = new Thread(() => simulate(bestValues));
            myNewThread.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread myNewThread = new Thread(() => simulate(worstValues));
            myNewThread.Start();
        }
        void simulate(List<int> inputList)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DrawableHearthstoneBoard board = lastBoard.copy();
            board.stockedRandomValues = new List<int>();
            foreach (int i in inputList)
                board.stockedRandomValues.Add(i);
            var visualBoard = new VisualBoard(board);
            Application.Run(visualBoard);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BoardEditor.saveBoard(lastBoard);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            var settings = new Settings(this, parent);
            settings.Show();
        }
    }
}
