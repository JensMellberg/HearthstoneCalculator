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
        GUIstate currentState;
        GUIstate lastAddedState;
        Dictionary<string, GUIstate> allStates = new Dictionary<string, GUIstate>();
        BoardStateReader parent;
        bool loaded = false;
        private void GUI_Load(object sender, EventArgs e)
        {
            loaded = true;
            chart1.Series["Series1"].Name = "Damage distribution";
            if (parent != null)
            setChartType(int.Parse(parent.settings[BoardStateReader.ChartType]));
        }
        public void setChartType(int type)
        {
            this.chart1.Invoke((MethodInvoker)delegate {
                switch (type)
                {
                    case 0: chart1.Series["Damage distribution"].ChartType = SeriesChartType.Line; break;
                    case 1: chart1.Series["Damage distribution"].ChartType = SeriesChartType.Area; break;
                    case 2: chart1.Series["Damage distribution"].ChartType = SeriesChartType.Column; break;
                } 
            });
        }

        public void addGUIstate(GUIstate newstate)
        {
            string stateName = newstate.opponent;
            int suffix = 1;
            while (allStates.ContainsKey(stateName))
            {
                suffix++;
                stateName = newstate.opponent + suffix;
            }
            allStates.Add(stateName, newstate);
            this.comboBox1.Invoke((MethodInvoker)delegate {
                comboBox1.Items.Add(stateName);
                if (currentState == lastAddedState)
                    comboBox1.SelectedIndex = comboBox1.Items.Count-1;
            });
            lastAddedState = newstate;
        }

        public void drawState(GUIstate state)
        {
            currentState = state;
            drawChart(state.dmgdist);
            setExpected(state.expectedHealth);
            setLabels(state.dmgdist2, state.opponent);
            setWorstAndBest(state.board, state.worstValues, state.bestValues);
            setChancetodie(state.deathodds);
            setRanking(state.ranking);
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
        public void resetState()
        {
            this.button1.Invoke((MethodInvoker)delegate {
                button1.Enabled = false;
            });
            this.button2.Invoke((MethodInvoker)delegate {
                button2.Enabled = false;
            });
            this.button3.Invoke((MethodInvoker)delegate {
                button3.Enabled = false;
            });
            this.label6.Invoke((MethodInvoker)delegate {
                label6.Text = "Expected hp: 40";
            });
            this.label7.Invoke((MethodInvoker)delegate {
                label7.Text = "Opponent: ";
            });
            this.label8.Invoke((MethodInvoker)delegate {
                label8.Text = "Expected: ";
            });
            this.label2.Invoke((MethodInvoker)delegate {
                label2.Text = "Draw chance: ";
            });
            this.label1.Invoke((MethodInvoker)delegate {
                label1.Text = "Win chance: ";
            });
            this.label3.Invoke((MethodInvoker)delegate {
                label3.Text = "Loss chance: ";
            });
            this.label10.Invoke((MethodInvoker)delegate {
                label10.Text = "Chance to die: ";
            });
            this.label4.Invoke((MethodInvoker)delegate {
                label4.Text = "Lucky wins: 0";
            });
            this.label5.Invoke((MethodInvoker)delegate {
                label5.Text = "Unlucky losses: 0";
            });
            this.comboBox1.Invoke((MethodInvoker)delegate {
            comboBox1.Items.Clear();
            allStates = new Dictionary<string, GUIstate>();
            });
            this.label11.Invoke((MethodInvoker)delegate {
                label11.Text = " Board strength: ?th percentile";
            });
            this.chart1.Invoke((MethodInvoker)delegate {
                chart1.Series["Damage distribution"].Points.Clear();
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
        public void setChancetodie(double percentage)
        {
            this.label10.Invoke((MethodInvoker)delegate {
                label10.Text = "Chance to die: "+ percentage + "%";
            });
        }

        public void setRanking(double ranking)
        {
            this.label11.Invoke((MethodInvoker)delegate {
                label11.Text = "Board strength: "+ranking+"th percentile";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GUIstate state = allStates[comboBox1.SelectedItem.ToString()];
            drawState(state);
        }
    }
}
