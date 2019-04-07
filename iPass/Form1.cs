using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Web;
using System.Configuration;


namespace iPass
{
    public partial class Form1 : Form
    {
        private WebClient wc;
        private Settings settings;
        private iPass ipass;

        private void addData(String key, String value)
        {
            richTextBox1.SelectionBullet = true;
            richTextBox1.Text += key + ":" + value.TrimStart().TrimEnd() + "\n";
        }

        public Form1()
        {
            wc = new CookieClient();
            settings = new Settings();

            if (!settings.hasUsername() && !settings.hasPassword())
            {
                Login login = new Login();
                login.ShowDialog();
            }

            ipass = new iPass(settings.getUsername(), settings.getPassword());
            ipass.login();

            InitializeComponent();

            ipass.loadBio();
            ipass.loadXpaths(Environment.CurrentDirectory + @"/xpaths.txt");

            /*
            addData("Name", ipass.getText("name"));
            addData("Email", ipass.getText("email"));
            addData("DOB", ipass.getText("dob"));
            addData("Age", ipass.getText("age"));
            addData("Locker", ipass.getText("locker"));
            */

            foreach (var xpath in ipass.getXpathValues())
            {
                if (xpath.Key == "studentid")
                {
                    string[] contents1 = ipass.getText(xpath.Key).Split(':');
                    String shortName = contents1[1].Replace(" ID", "");
                    String id = contents1[2];
                    addData("STUDENT ID", WebUtility.HtmlDecode(id));
                }
                else
                {
                    addData(xpath.Key.ToUpper(), ipass.getText(xpath.Key));
                }
            }
            richTextBox1.Text = richTextBox1.Text.TrimEnd('\n');
            pictureBox1.Image = ipass.getPhoto();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ipass.getText("name"));
            Dictionary<String, String> bio = ipass.getBio();
        }
    }
}
