using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

[Serializable]
public class DrawableCard : Card
{
    public const string removeTag = "Temp";
    [NonSerialized]
    PictureBox portrait;
    [NonSerialized]
    Label atkLabel;
    [NonSerialized]
    Label hpLabel;
    [NonSerialized]
    Label nameLabel;
    public const int portraitWidth = 80;
    public const int portraitHeight = 80;

    int characters = 0;

    public string pictureID;

    public DrawableCard(int ID, string name, int attack, int hp, List<Effect> effects, bool poisonous, bool divineShield, bool taunt, Type type,int tier, string cardID)
        : base(ID, name, attack, hp, effects, poisonous,divineShield,taunt,type,tier,cardID)
	{
	}
    [NonSerialized]
    List<Label> statustexts = new List<Label>();

    public void draw(Point position, Form gui, HearthstoneBoard board)
    {
        statustexts = new List<Label>();
        portrait = new PictureBox();
        portrait.Location = position;
        portrait.Size = new Size(portraitWidth, portraitHeight);
        portrait.Image = setUpImage();
        portrait.Tag = removeTag;
        gui.Controls.Add(portrait);
        nameLabel = new Label();
        nameLabel.Text = name;
        nameLabel.Location = new Point(position.X, position.Y + portraitHeight);
        nameLabel.Size = new Size(portraitWidth + 40, 20);
        nameLabel.Tag = removeTag;
        gui.Controls.Add(nameLabel);
        atkLabel = new Label();
        atkLabel.Text = getAttack(board).ToString();
        atkLabel.ForeColor = Color.Red;
        atkLabel.Location = new Point(position.X-20, position.Y + portraitHeight - 20);
        atkLabel.Tag = removeTag;
        gui.Controls.Add(atkLabel);
        hpLabel = new Label();
        hpLabel.Text = getHp(board).ToString();
        hpLabel.ForeColor = Color.Red;
        hpLabel.Size = new Size(40, 20);
        hpLabel.Location = new Point(position.X + portraitWidth+ 10, position.Y + portraitHeight - 20);
        hpLabel.Tag = removeTag;
        gui.Controls.Add(hpLabel);
        characters = 0;
        if (taunt)
            addStatusText(position,"T ", Color.Gray,gui);
        if (divineShield)
            addStatusText(position, "D ", Color.Gold, gui);
        if (poisonous)
            addStatusText(position, "P ", Color.Green, gui);
        if (windfury)
            addStatusText(position, "WF ", Color.White, gui);
        if (golden)
            addStatusText(position, "G ", Color.Gold, gui);

    }
    public void addStatusText(Point startpos, string text, Color color, Form gui)
    {
        Label l = new Label();
        l.Text = text;
        l.ForeColor = color;
        l.AutoSize = true;
        int width = characters*12;
        int height = 20;
        while (width > portraitWidth)
        {
            width -= portraitWidth;
            height += 13;
        }
       l.Location = new Point(startpos.X+width, startpos.Y +portraitHeight+ height);

        gui.Controls.Add(l);
        characters += text.Length;
        l.Tag = removeTag;

    }




    public override Card copy()
    {
        List<Effect> newEffs = new List<Effect>();
        foreach (Effect e in effects)
            newEffs.Add(e);
        DrawableCard c = new DrawableCard(ID, name, attack, hp, newEffs, poisonous, divineShield, taunt, type, tavernTier, cardID);
        c.golden = golden;
        c.windfury = windfury;
        c.pictureID = pictureID;
        c.attackPriority = attackPriority;
        return c;
    }
    private byte[] DownloadRemoteImageFile(string url)
    {
        byte[] content;
        var request = (HttpWebRequest)WebRequest.Create(url);

        using (var response = request.GetResponse())
        using (var reader = new BinaryReader(response.GetResponseStream()))
        {
            content = reader.ReadBytes(10000000);
        }

        return content;
    }

    private Image setUpImage()
    {
        if (pictureID == null)
            pictureID = "CFM_316t";
        var img = DownloadRemoteImageFile("https://art.hearthstonejson.com/v1/orig/"+pictureID+".png");
        Image original;
        using (var ms = new MemoryStream(img))
        {
            original = Image.FromStream(ms);
        }
        var newPic = new Bitmap(portraitWidth, portraitHeight);
        var gr = Graphics.FromImage(newPic);
        gr.DrawImage(original, 0, 0, portraitWidth, portraitHeight);
        return newPic;
    }

    public static DrawableCard makeDrawable(Card c)
    {
      
        List<Effect> newEffs = new List<Effect>();
        foreach (Effect e in c.effects)
            newEffs.Add(e);
        DrawableCard ret = new DrawableCard(c.ID, c.name, c.attack, c.hp, newEffs, c.poisonous, c.divineShield, c.taunt, c.type, c.tavernTier, c.cardID);
        ret.golden = c.golden;
        ret.attackPriority = c.attackPriority;

        ret.windfury = c.windfury;
        return ret;
    }

  


}
