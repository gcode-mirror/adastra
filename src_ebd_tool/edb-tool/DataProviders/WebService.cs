using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Newtonsoft.Json;

namespace edb_tool
{
    public class WebService : DataProvider
    {
        string RootUrl;

        public WebService()
        {
            RootUrl = "http://localhost/edb-json/";
            //RootUrl = "http://si-devel.gipsa-lab.grenoble-inp.fr/edm/";
        }

        public void AddSubject(GSubject s)
        {
          string link =
          RootUrl + "subject.php?function=" + "AddSubject" +
                                              "&name=" + System.Web.HttpUtility.UrlEncode(s.name) +
                                              "&age=" + s.age +
                                              "&sex=" + s.sex +
                                              "&idexperiment=" + s.idexperiment +
                                              "&iduser=" + s.iduser;
          Helper.Get(link);
        }

        public List<GSubject> ListSubjects(int iduser)
        {
            var json = JsonHelper.DownloadJson(RootUrl + "subject.php?function=" + "ListSubjects" 
                                                                 + "&iduser=" + iduser.ToString());

            List<GSubject> subjects = JsonConvert.DeserializeObject<List<GSubject>>(json);

            return subjects;
        }

        public List<GSubject> ListSubjectsByExperimentId(int idexperiment, int iduser)
        {
            var json = JsonHelper.DownloadJson(RootUrl + "subject.php?function=" + "ListSubjects" 
                                                                       + "&iduser=" + iduser.ToString()
                                                                       + "&idexperiment=" + idexperiment.ToString()
                                                                       );

            List<GSubject> subjects = JsonConvert.DeserializeObject<List<GSubject>>(json);

            return subjects;
        }

        public void DeleteSubject(int id)
        {
            string link =
            RootUrl + "subject.php?function=" + "DeleteSubject" +
                                                "&idsubject=" + id;

            Helper.Get(link);
        }

        public void UpdateSubject(GSubject s)
        {
          string link =
          RootUrl + "subject.php?function=" + "UpdateSubject" +
                                              "&idsubject=" + s.idsubject +
                                              "&name=" + System.Web.HttpUtility.UrlEncode(s.name) +
                                              "&age=" + s.age +
                                              "&sex=" + s.sex +
                                              "&idexperiment=" + s.idexperiment +
                                              "&iduser=" + s.iduser;

            Helper.Get(link);
        }

        public void AddExperiment(GExperiment exp)
        {
            string link = 
            RootUrl + "experiment.php?function=" + "AddExperiment" + 
                                                "&name=" + System.Web.HttpUtility.UrlEncode(exp.name) +
                                                "&comment=" + System.Web.HttpUtility.UrlEncode(exp.comment) + 
                                                "&description=" + System.Web.HttpUtility.UrlEncode(exp.description) +
                                                "&iduser=" + exp.iduser;

            Helper.Get(link);
        }

        public List<GExperiment> ListExperiments(int iduser)
        {
            var json = JsonHelper.DownloadJson(RootUrl + "experiment.php?function=" + "ListExperiments" + "&iduser=" + iduser.ToString());

            List<GExperiment> experiments = JsonConvert.DeserializeObject<List<GExperiment>>(json);

            return experiments;
        }

        public List<GExperiment> ListExperimentsByExperimentIdUserId(int idexperiment, int iduser)
        {
            var json = JsonHelper.DownloadJson(RootUrl + "experiment.php?function=" + "ListExperimentsByExperimentIdUserId" + "&iduser=" + iduser.ToString() + "&idexperiment" + idexperiment.ToString());

            List<GExperiment> experiments = JsonConvert.DeserializeObject<List<GExperiment>>(json);

            return experiments;
        }

        public void DeleteExperiment(int id)
        {
            string link =
            RootUrl + "experiment.php?function=" + "DeleteExperiment" +
                                                "&idexperiment=" + id.ToString();

            Helper.Get(link);
        }

        public void UpdateExperiment(GExperiment exp)
        {
           string link =
           RootUrl + "experiment.php?function=" + "UpdateExperiment" +
                                               "&idexperiment=" + exp.idexperiment +
                                               "&name=" + System.Web.HttpUtility.UrlEncode(exp.name) +
                                               "&comment=" + System.Web.HttpUtility.UrlEncode(exp.comment) +
                                               "&description=" + System.Web.HttpUtility.UrlEncode(exp.description) +
                                               "&iduser=" + exp.iduser;

            Helper.Get(link);
        }


        public void AddModality(string name, string comment, string description)
        {
        }

        public DataTable ListModalities()
        {
            return null;
        }

        public void DeleteModality(int id)
        {
        }

        public void UpdateModality(int id, string name, string comment, string description)
        {
        }

        public void AddTag(string name)
        {
        }

        public DataTable ListTags()
        {
            return null;
        }

        public void DeleteTag(int id)
        {
        }

        public void UpdateTag(int id, string name)
        {
        }

        public long AssociateTags(int[] idtag, int idfile, int idexperiment)
        {
            return -1;
        }

        public void AddModalityToExperiment(int idmodality, int idexperiment)
        {
        }

        public DataTable ListModalitiesByExperimentSubjectID(int idexperiment)
        {
            return null;
        }

        public long AddFile(string name, string path)
        {
            return -1;
        }

        public long AddFiles(string[] name, string[] path)
        {
            return -1;
        }

        public void AssociateFile(int idexperiment, int idsubject, int idmodality, long idfile)
        {
        }

        public DataTable ListFilesByExperimentSubjectModalityID(int idexperiment, int idsubject, int idmodality)
        {
            return null;
        }

        public void DeleteFilesByExperimentIdSubjectIdModalityId(int idexperiment, int idsubject, int idmodality)
        {
        }

        public void DeleteFilesByFileId(int idfile)
        {
        }

        public void DeleteFilesByFileIdFromListFile(int idfile)
        {
        }

        public void AddUser(string firstname, string lastname, string username, string password, string email)
        {
        }

        public bool VerifyUserPassword(string username, string password, out int userid)
        {
            userid = -1;

            string link = RootUrl + "user.php?function=" + "VerifyUserPassword"
                                                           + "&username=" + System.Web.HttpUtility.UrlEncode(username)
                                                           + "&password=" + System.Web.HttpUtility.UrlEncode(password);
            var json = JsonHelper.DownloadJson(link);

            userid = JsonConvert.DeserializeObject<int>(json);


            return userid > 0;
        }

        public void UpdateFileTags(int[] idfiles, string tagLine)
        {
        }
    }
}
