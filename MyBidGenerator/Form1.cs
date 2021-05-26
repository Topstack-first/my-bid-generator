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

namespace MyBidGenerator
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string>  bidDictionary = new Dictionary<string, string>();
        private String bidDir = "bids";
        
        public Form1()
        {
            InitializeComponent();
            initializeBidDictionary();
        }
        private void initializeBidDictionary()
        {
            bidDictionary.Clear();
            loadFromDir(bidDir);
        }
        private void loadFromDir(String bidDir)
        {
            if(!Directory.Exists(bidDir))
            {
                Directory.CreateDirectory(bidDir);
                return;
            }
            string[] filePaths = Directory.GetFileSystemEntries(bidDir);
            for(int i=0;i<filePaths.Length;i++)
            {
                string text = File.ReadAllText(filePaths[i]);
                if(text != "")
                {
                    bidDictionary.Add(Path.GetFileName(filePaths[i]), text);
                }
            }
        }
        private void OnButtonClick(object sender, EventArgs e)
        {
            
            string senderName = ((Button)sender).Text;
            string senderColor = ((Button)sender).BackColor.Name;
            senderName = senderColor + senderName;
            string text = "";

            if (this.checkBox1.Checked)
            {
                text = this.rich_bid.Text;
                if (text == "")
                    return;
                File.WriteAllText("bids/" + senderName, text);

                this.checkBox1.Checked = false;
                initializeBidDictionary();
                this.rich_bid.Clear();
                return;
            }

            if (bidDictionary.TryGetValue(senderName,out text))
            {
                string randomStr = this.generateRandomString("-");
                text = text.Replace("**line**", randomStr);

                randomStr = this.generateRandomString("★");
                text = text.Replace("**star**", randomStr);

                randomStr = this.generateRandomString("*");
                text = text.Replace("**multi**", randomStr);

                randomStr = this.generateRandomString(".");
                text = text.Replace("**dot**", randomStr);

                this.rich_bid.Text = text;
                Clipboard.SetText(text);
            }
            else
            {
                this.rich_bid.Clear();
            }
        }
        private string generateRandomString(string one)
        {
            Random rand = new Random();
            int num = rand.Next(10, 100);
            string randomDots = "";
            for (int i = 0; i < num; i++)
            {
                randomDots += one;
            }
            return randomDots;
        }


    }
}
