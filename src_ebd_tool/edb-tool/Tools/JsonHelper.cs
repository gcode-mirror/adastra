using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace edb_tool
{
    public static class JsonHelper
    {
        public static string DownloadJson(string url)
        {
            string json = "";
            try
            {
                WebClient wc = new WebClient();
                json = wc.DownloadString(url);
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to connect to:" + url);
            }
            return json;
        }

        public static bool SendJson(string encode_json,string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = encode_json;// Need to put data here to pass to the API.**

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                //Now you have your response.
                //or false depending on information in the response
                return true;
            }
        }
    }
}
