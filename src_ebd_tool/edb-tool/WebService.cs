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
            //RootUrl = "http://localhost/edb-json/";
            RootUrl = "http://si-devel.gipsa-lab.grenoble-inp.fr/edm/";
        }

        public void AddSubject(string name, int sex, int age, int idexperiment, int iduser)
        {

        }

        public DataTable ListSubjects(int iduser)
        {
            
            return null;
        }

        public DataTable ListSubjectsByExperimentId(int idexperiment, int iduser)
        {
            return null;
        }

        public void DeleteSubject(int id)
        {
        }

        public void UpdateSubject(int id, string name, int sex, int? age, int idexperiment)
        {
        }

        public void AddExperiment(string name, string comment, string description, int iduser)
        {
        }

        public List<GExperiment> ListExperiments(int iduser)
        {
            var json = JsonHelper.DownloadJson(RootUrl + "experiment/list.php?" + "iduser=" + iduser.ToString());

            List<GExperiment> experiments = JsonConvert.DeserializeObject<List<GExperiment>>(json);

            return experiments;
        }

        public DataTable ListExperimentsByExperimentIdUserId(int idexperiment, int iduser)
        {
            return null;
        }

        public void DeleteExperiment(int id)
        {
        }

        public void UpdateExperiment(int id, string name, string comment, string description)
        {

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
            DataProvider mysql = new MySql();

            return mysql.VerifyUserPassword(username, password, out userid);
        }

        public void UpdateFileTags(int[] idfiles, string tagLine)
        {
        }
    }
}
