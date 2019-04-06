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
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> bio = ipass.getBio();

            foreach(var entry in bio){
                richTextBox1.Text += entry.Key.Trim() + entry.Value.Trim() + "\n";
            }
        }
    }
}
