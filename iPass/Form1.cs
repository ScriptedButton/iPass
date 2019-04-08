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

namespace iPass
{
    public partial class Form1 : Form
    {
        private WebClient wc;
        private Settings settings;
        private iPass ipass;
        private String lastKey;

        private void addData(String key, String value)
        {
            richTextBox1.SelectionBullet = true;
            richTextBox1.Text += key + ":" + value.TrimStart().TrimEnd() + "\n";
        }

        private void addData2(String key, String value)
        {
            richTextBox2.SelectionBullet = true;
            richTextBox2.Text += key + ":" + value.TrimStart().TrimEnd() + "\n";
        }

        private void addData3(String key, String value)
        {
            richTextBox3.SelectionBullet = true;
            richTextBox3.Text += key + ":" + value.TrimStart().TrimEnd() + "\n";
        }

        private void init()
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
            ipass.loadBio();
            ipass.loadSchedule();
            webBrowser1.DocumentText = ipass.getSchedule().Replace("window.top.location.replace('/school/nsboro/syslogin.html')", "");
            webBrowser2.Url = new Uri("https://ipassweb.harrisschool.solutions/school/nsboro/samgrades.html");
            webBrowser2.DocumentText = ipass.getGrades().Replace("window.top.location.replace('/school/nsboro/syslogin.html')", "");
            ipass.loadXpaths(Environment.CurrentDirectory + @"/xpaths.txt");

            string[][] phoneNumbers = ipass.getPhoneNumbers();
            string[][] guardians = ipass.getGuardians();
            string[][] busRoutes = ipass.getBusRoute();
            foreach (string[] phoneNumber in phoneNumbers)
            {
                dataGridView1.Rows.Add(phoneNumber[0], phoneNumber[1], phoneNumber[2]);
            }
            foreach (string[] guardian in guardians)
            {
                dataGridView2.Rows.Add(guardian[0], guardian[1], guardian[2]);
            }
            foreach (string[] route in busRoutes)
            {
                dataGridView3.Rows.Add(route[0], route[1], route[2]);
            }


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
                    lastKey = xpath.Key;
                }
                else if (xpath.Key == "zipcode")
                {
                    lastKey = xpath.Key;
                }
                else if(xpath.Key == "mailingzip")
                {
                    lastKey = xpath.Key;
                }
                else if (lastKey == "studentid")
                {
                    addData2(xpath.Key.ToUpper(), ipass.getText(xpath.Key));
                }
                else if (lastKey == "zipcode")
                {
                    addData3(xpath.Key.ToUpper(), ipass.getText(xpath.Key));
                }
                else if(lastKey == "mailingzip")
                {
                    break;
                }
                else
                {
                    addData(xpath.Key.ToUpper(), ipass.getText(xpath.Key));
                }
            }
            richTextBox1.Text = richTextBox1.Text.TrimEnd('\n');
            richTextBox2.Text = richTextBox2.Text.TrimEnd('\n');
            richTextBox3.Text = richTextBox3.Text.TrimEnd('\n');
            pictureBox1.Image = ipass.getPhoto();

            textBox1.Text = ipass.getText("lockertype").TrimEnd();
            textBox2.Text = ipass.getText("lockernumber").TrimEnd();
            textBox3.Text = ipass.getText("lockercombo").TrimEnd();
        }

        public Form1()
        {
            InitializeComponent();
            init();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ipass.getText("name"));
            Dictionary<String, String> bio = ipass.getBio();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
                if (webBrowser2.Document != null)
                {
                webBrowser2.Document.GetElementById("academicYear").OuterHtml = "";
                    foreach (HtmlElement imgElemt in webBrowser2.Document.Images)
                    {
                        imgElemt.SetAttribute("src", "");
                        imgElemt.SetAttribute("alt", "");
                    }
                }
        }
    }
}
