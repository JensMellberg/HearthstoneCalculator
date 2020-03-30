using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public partial class VisualBoard : Form
    {
        public VisualBoard(DrawableHearthstoneBoard parentBoard)
        {
            visualizeBoard = parentBoard.copy();
            InitializeComponent();
        }
        DrawableHearthstoneBoard visualizeBoard;
        private void VisualBoard_Load(object sender, EventArgs e)
        {
            visualizeBoard.finishedWorkFlag = false;
            Thread t = new Thread(new ThreadStart(visualizeBoard.visualizeSimulation));
            t.Start();
            while (!visualizeBoard.finishedWorkFlag)
                Thread.Sleep(100);

            drawBoardState();

    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            visualizeBoard.stopFlag = false;
            visualizeBoard.finishedWorkFlag = false;
            while (!visualizeBoard.finishedWorkFlag)
                Thread.Sleep(100);
            drawBoardState();
        }
        public void drawBoardState()
        {
            visualizeBoard.drawBoardState(new Point(50, 50), new Point(50, 300), this);
            if (visualizeBoard.finished)
            {
                pictureBox1.Hide();
                pictureBox2.Hide();
                button1.Hide();
                return;
            }
            if (visualizeBoard.getPlayerFromMinion(visualizeBoard.attacker) == 2)
                pictureBox1.Location = new Point(55 + visualizeBoard.p2Board.IndexOf(visualizeBoard.attacker) * DrawableHearthstoneBoard.minionSpaceBetween, 50 + DrawableCard.portraitHeight + 30);
            else
                pictureBox1.Location = new Point(55 + visualizeBoard.p1Board.IndexOf(visualizeBoard.attacker) * DrawableHearthstoneBoard.minionSpaceBetween, 230);

            if (visualizeBoard.getPlayerFromMinion(visualizeBoard.defender) == 2)
                pictureBox2.Location = new Point(55 + visualizeBoard.p2Board.IndexOf(visualizeBoard.defender) * DrawableHearthstoneBoard.minionSpaceBetween, 50 + DrawableCard.portraitHeight + 30);
            else
                pictureBox2.Location = new Point(55 + visualizeBoard.p1Board.IndexOf(visualizeBoard.defender) * DrawableHearthstoneBoard.minionSpaceBetween, 230);
        }
    }
}
