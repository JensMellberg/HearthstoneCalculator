using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public partial class Settings : Form
    {
        GUI parent;
        BoardStateReader reader;
        public Settings(GUI gui, BoardStateReader reader)
        {
            parent = gui;
            this.reader = reader;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.FileName = "Folder";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                  
                textBox1.Text = folderPath;
                }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = reader.path.Substring(0,reader.path.Length-6);
            textBox2.Text = reader.simulationCount.ToString();
            textBox3.Text = reader.strengthSimuls.ToString();
            comboBox1.SelectedIndex = int.Parse(reader.settings[BoardStateReader.ChartType] );
            checkBox1.Checked = reader.saveBoards;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                reader.path = textBox1.Text + @"\Logs\";
                reader.settings[BoardStateReader.PathID] = reader.path;
                reader.settings[BoardStateReader.SimulationID] = int.Parse(textBox2.Text).ToString();
                reader.settings[BoardStateReader.StrengthID] = int.Parse(textBox3.Text).ToString();
                reader.settings[BoardStateReader.ChartType] = comboBox1.SelectedIndex.ToString();
                reader.settings[BoardStateReader.SaveBoards] = checkBox1.Checked ? "1" : "0";
                reader.saveSettings();
            } catch(Exception)
            {
                MessageBox.Show("Invalid values");
                return;
            }
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
