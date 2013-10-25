using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

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
                      select new GUser
                      {
                          Name = (Regex.Match(entry, regName, RegexOptions.IgnoreCase)).Groups[1].Value,
                          Mail = mail,
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
              int pos = user.Username.IndexOf(".");
              string lastname = user.Username.Substring(pos + 1);
              string firstname = user.Username.Substring(0, pos);
              string password = user.Username;
              db.AddUser(firstname, lastname, user.Username, password, user.Mail);
            }
        }
    }
}
