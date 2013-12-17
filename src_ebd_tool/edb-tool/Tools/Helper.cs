using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.DirectoryServices.AccountManagement;

namespace edb_tool
{
    public static class Helper
    {
        public static int LocateColumnInGrid(string name, DataGridView gridview)
        {
            int idcolumn = -1;//column localization required
            for (int i = 0; i < gridview.Columns.Count; i++)
            {
                if (gridview.Columns[i].HeaderText.ToLower() == name.ToLower())
                {
                    idcolumn = i;
                    break;
                }
            }

            return idcolumn;
        }

        public static string GetFileShortName(string path)
        {
            string shortname="";

            int lastslash = path.LastIndexOf("\\");

            if (lastslash != -1)
            {
                shortname = path.Substring(lastslash + 1);
            }

            return shortname;
        }

        public static GUser[] Scan(string url)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string webData = wc.DownloadString(url);

            string[] entries = Regex.Split(webData, "<div class=\"bloc_infobulle\">");

            //var qq = from string entry in entries
            //         where (Regex.Match(entry, "^\n\\s+<a\\s+href=\"http://www.leboncoin.fr/",
            //             RegexOptions.IgnoreCase)).Success
            //         select entry;

            string[] justAprarts = entries;//qq.ToArray();

            string regName = "<span class=\"contenu_titre_article\">([^\\>]*)</span>";
            string regMail = "<a href=\"mailto:([^\\>]*)\">envoyer mail</a>";
            //string regArea = "\\s+(\\d+)\\s*m[2|²]\n";
            //string regCity = "<div class=\"placement\">\n\\s*\n\\s*\n\\s*\n\\s*\n\\s*\n\\s*([^\\>]*)</div>";
            //string regLink = "<a href=\"([^\"]*)\" title";

            var qq2 = from string entry in justAprarts
                      where
                      (Regex.Match(entry, regName, RegexOptions.IgnoreCase)).Success
                      && (Regex.Match(entry, regMail, RegexOptions.IgnoreCase)).Success
                      //&& (Regex.Match(entry, regArea, RegexOptions.IgnoreCase)).Success
                      //&& (Regex.Match(entry, regCity, RegexOptions.IgnoreCase)).Success
                      //&& (Regex.Match(entry, regLink, RegexOptions.IgnoreCase)).Success
                      let mail = (Regex.Match(entry, regMail, RegexOptions.IgnoreCase)).Groups[1].Value.Replace("\"","")
                      where mail!=""
                      let Name = (Regex.Match(entry, regName, RegexOptions.IgnoreCase)).Groups[1].Value
                      let pos = Name.IndexOf(".")
                      select new GUser
                      {
                         FirstName = Name.Substring(pos + 1),
                         LastName = Name.Substring(0, pos),
                         EMail = mail,
                         Username = mail.Substring(0,mail.IndexOf("@")),
                      };

            var extracted = qq2.ToArray();

            //toolStripStatusLabel1.Text = "Processed!";

            //GUser[] badusers = (from u in extracted
            //                    where u.Username == "nouser"
            //                    select u).ToArray();

            return extracted;
        }

        public static void ImportUsers(GUser[] users)
        {
            MySql db = new MySql();

            foreach (var user in users)
            {
              db.AddUser(user, user.Username);
            }
        }

        public static string Get(string url)
        {
            //http://localhost/edb-json/experiment.php?function=AddExperiment&name=orel&comment=&description&iduser1999

            string responseText = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();

            using (var streamReader = new StreamReader(resStream))
            {
                responseText = streamReader.ReadToEnd();
                //Now you have your response.
                //or false depending on information in the response
                //return true;
            }

            return responseText;

        }

        public static bool VerifyPassword(string username,string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "gipsa-lab.local"))
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(username, password);
                return isValid;
            }
        }
    }
}
