using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace iPass
{
    class iPass
    {
        private String username;
        private String password;
        private WebClient wc;
        private HtmlNode studentBio;
        private Dictionary<String, String> xpathValues;

        public iPass (String username, String password)
        {
            this.username = username;
            this.password = password;
            this.wc = new CookieClient();
            this.xpathValues = new Dictionary<String, String>();
        }

        public void login()
        {
            wc.Headers.Add("Content-Type: application/x-www-form-urlencoded");

            String login = wc.UploadString("https://ipassweb.harrisschool.solutions/school/nsboro/syslogin.html", String.Format("userid={0}&password={1}", username, password));
        }

        public void loadXpaths(String filePath)
        {
            var lines = File.ReadLines(filePath);
            foreach (var line in lines)
            {
                string[] contents = line.Split(':');

                xpathValues.Add(contents[0], contents[1]);
            }
        }

        public void loadBio()
        {

            String viewBio = wc.DownloadString("https://ipassweb.harrisschool.solutions/school/nsboro/istudentbio.htm");

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(viewBio);
            studentBio = doc.DocumentNode;
        }

        private String getXpath(String key)
        {
            return xpathValues[key];
        }

        public String getText(String name)
        {
            String xpath = getXpath(name);

            HtmlNode value = studentBio.SelectSingleNode(xpath);
            return WebUtility.HtmlDecode(value.InnerText);
        }


        public Image getPhoto()
        {
            HtmlNode photo = studentBio.SelectSingleNode("//*[@id=\"kidphoto\"]");
            using (Stream stream = wc.OpenRead("https://ipassweb.harrisschool.solutions/school/nsboro/" + photo.GetAttributeValue("src", "")))
            {
            return Image.FromStream(stream);
            }
        }

        public Dictionary<String, String> getXpathValues()
        {
            return xpathValues;
        }

        public Dictionary<String,String> getBio()
        {
            Dictionary<String, String> data = new Dictionary<String, String>();

            String viewBio = wc.DownloadString("https://ipassweb.harrisschool.solutions/school/nsboro/istudentbio.htm");

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(viewBio);
            HtmlNode documentNode = doc.DocumentNode;

            var test = documentNode.Descendants("td").Where(n => n.HasClass("Labelr"));

            foreach (HtmlNode element in test)
            {
                HtmlNode label = element;
                HtmlNode value = element.NextSibling.NextSibling;

                if (!data.ContainsKey(WebUtility.HtmlDecode(label.InnerText.Trim(':', ' ')).Trim()) && value != null && !string.IsNullOrEmpty(label.InnerText) && !string.IsNullOrEmpty(value.InnerText))
                {
                    data.Add(WebUtility.HtmlDecode(label.InnerText.Trim(':', ' ')).Trim(), WebUtility.HtmlDecode(value.InnerText.Trim()));
                }
                
            }

            return data;
        }
    }
}
