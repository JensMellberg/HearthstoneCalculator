using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace ConsoleApp1
{
    [Serializable]
   public class DrawableHearthstoneBoard : HearthstoneBoard
    {
        public const int minionSpaceBetween = 150;
        public DrawableHearthstoneBoard()
        {

        }
        public override void addNewMinionToBoard(BoardSide current, Card c, int position, int overAllow)
        {
            DrawableCard d = DrawableCard.makeDrawable(c);
            switch (d.getName())
            {
                case "Guard Bot": d.pictureID = "BOT_218t"; break;
                case "Rat": d.pictureID = "CFM_316t"; break;
                case "Hyena": d.pictureID = "ULD_154t"; break;
                case "Microbot": d.pictureID = "BOT_312t"; break;
                case "Spider": d.pictureID = "OG_216a"; break;
                case "Voidwalker": d.pictureID = "CS2_065"; break;
                case "Robosaur": d.pictureID = "BOT_537t"; break;
                case "Imp": d.pictureID = "TB_BaconUps_030t"; break;
                case "Finkle Einhorn": d.pictureID = "EX1_finkle"; break;
                case "Big Bad Wolf": d.pictureID = "KAR_005a"; break;
                case "Ironhide Runt": d.pictureID = "TRL_232t"; break;
                case "Damaged Golem": d.pictureID = "skele21"; break;
                case "Bronze Warden": d.pictureID = "BGS_034"; break;
                case "Jo-E Bot": d.pictureID = "BOT_445t"; break;
                default: d.pictureID = "CFM_316t"; break;
            }
            
            base.addNewMinionToBoard(current, d, position, overAllow); 
        }
        public void drawBoardState(Point p1startPoint, Point p2startPoint, Form gui)
        {
            for (int i = gui.Controls.Count-1; i>= 0; i--)
            {
                if ((string)gui.Controls[i].Tag == DrawableCard.removeTag)
                    gui.Controls.RemoveAt(i);
            }
            
            foreach (Card c in p2Board)
            {
                DrawableCard d = ((DrawableCard)c);
                d.draw(p1startPoint, gui, this);
                p1startPoint.X += minionSpaceBetween;
            }
            foreach (Card c in p1Board)
            {
                DrawableCard d = ((DrawableCard)c);
                d.draw(p2startPoint, gui, this);
                p2startPoint.X += minionSpaceBetween;
            }
        }
        public new DrawableHearthstoneBoard copy()
        {
            DrawableHearthstoneBoard board = new DrawableHearthstoneBoard();
            board.printPriority = printPriority;
            board.turnbyturn = turnbyturn;
            board.recievedRandomValues = recievedRandomValues;
            board.stockedRandomValues = stockedRandomValues;
            board.p1Board = p1Board.copy();
            board.p2Board = p2Board.copy();
            return board;
        }
    }
}
