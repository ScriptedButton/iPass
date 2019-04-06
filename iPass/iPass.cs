using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;

namespace iPass
{
    class iPass
    {
        private String username;
        private String password;
        private WebClient wc;

        public iPass (String username, String password)
        {
            this.username = username;
            this.password = password;
            this.wc = new CookieClient();
        }

        public void login()
        {
            wc.Headers.Add("Content-Type: application/x-www-form-urlencoded");

            String login = wc.UploadString("https://ipassweb.harrisschool.solutions/school/nsboro/syslogin.html", String.Format("userid={0}&password={1}", username, password));
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
