﻿using System;
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
            ////RootUrl = "http://localhost/edb-json/";
            //RootUrl = "http://si-devel.gipsa-lab.grenoble-inp.fr/edm/";
            RootUrl = ProviderFactory.GetWebProvider();
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
            var json = JsonHelper.DownloadJson(RootUrl + "subject.php?function=" + "ListSubjectsByExperimentId" 
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
            string link = RootUrl + "experiment.php?function=" + "ListExperimentsByExperimentIdUserId" + "&iduser=" + iduser.ToString() + "&idexperiment=" + idexperiment.ToString();
            var json = JsonHelper.DownloadJson(link);

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


        public void AddModality(GModality m)
        {
            string link =
            RootUrl + "modality.php?function=" + "AddModality" +
                                                "&name=" + System.Web.HttpUtility.UrlEncode(m.name) +
                                                "&comment=" + System.Web.HttpUtility.UrlEncode(m.comment) +
                                                "&description=" + System.Web.HttpUtility.UrlEncode(m.description);

            Helper.Get(link);
        }

        public List<GModality> ListModalities()
        {
            var json = JsonHelper.DownloadJson(RootUrl + "modality.php?function=" + "ListModalities");

            List<GModality> modalities = JsonConvert.DeserializeObject<List<GModality>>(json);

            return modalities;
        }

        public void DeleteModality(int id)
        {
            string link =
            RootUrl + "modality.php?function=" + "DeleteModality" +
                                                "&idmodality=" + id.ToString();

            Helper.Get(link);
        }

        public void UpdateModality(GModality m)
        {
            string link =
            RootUrl + "modality.php?function=" + "UpdateModality" +
                                                "&idmodality=" + m.idmodality +
                                                "&name=" + System.Web.HttpUtility.UrlEncode(m.name) +
                                                "&comment=" + System.Web.HttpUtility.UrlEncode(m.comment) +
                                                "&description=" + System.Web.HttpUtility.UrlEncode(m.description);

            Helper.Get(link);
        }

        public void AddTag(GTag tag)
        {
            string link =
            RootUrl + "tag.php?function=" + "AddTag" +
                                                "&name=" + System.Web.HttpUtility.UrlEncode(tag.name);

            Helper.Get(link);
        }

        public List<GTag> ListTags()
        {
            var json = JsonHelper.DownloadJson(RootUrl + "tag.php?function=" + "ListTags");

            List<GTag> tags = JsonConvert.DeserializeObject<List<GTag>>(json);

            return tags;
        }

        public void DeleteTag(int id)
        {
            string link =
            RootUrl + "tag.php?function=" + "DeleteTag" +
                                                "&idtag=" + id.ToString();

            Helper.Get(link);
        }

        public void UpdateTag(GTag tag)
        {
            string link =
            RootUrl + "tag.php?function=" + "UpdateTag" +
                                                "&idtag=" + tag.idtag +
                                                "&name=" + System.Web.HttpUtility.UrlEncode(tag.name);

            Helper.Get(link);
        }

        public long AssociateTags(int[] idtag, int idfile, int idexperiment)
        {
            return -1;
        }

        public void AddModalityToExperiment(int idmodality, int idexperiment)
        {
            string link = RootUrl + "modality.php?function=" + "AddModalityToExperiment"
                                                             + "&idmodality=" + idmodality.ToString()
                                                             + "&idexperiment=" + idexperiment.ToString();
            Helper.Get(link);
        }

        public List<GModality> ListModalitiesByExperimentID(int idexperiment)
        {
            string link = RootUrl + "modality.php?function=" + "ListModalitiesByExperimentID"
                                                              + "&idexperiment=" + idexperiment.ToString();
            var json = JsonHelper.DownloadJson(link);

            List<GModality> modalities = JsonConvert.DeserializeObject<List<GModality>>(json);

            return modalities;
        }

        public long AddFile(GFile f)
        {
           string link =
           RootUrl + "file.php?function=" + "AddFile" +
                                               "&filename=" + System.Web.HttpUtility.UrlEncode(f.filename) +
                                               "&pathname=" + System.Web.HttpUtility.UrlEncode(f.pathname);

           var json = JsonHelper.DownloadJson(link);
           long idfile = JsonConvert.DeserializeObject<int>(json);

           return idfile;
        }

        public void AddFiles(List<GFile> files)
        {
            //return -1;
        }

        public void AssociateFile(int idexperiment, int idsubject, int idmodality, long idfile)
        {
        }

        public List<GFile> ListFilesByExperimentSubjectModalityID(int idexperiment, int idsubject, int idmodality)
        {
            var json = JsonHelper.DownloadJson(RootUrl + "file.php?function=" + "ListFilesByExperimentSubjectModalityID"
                                                + "&idexperiment=" + idexperiment.ToString()
                                                + "&idsubject=" + idsubject.ToString()
                                                + "&idmodality=" + idmodality.ToString());

            List<GFile> files = JsonConvert.DeserializeObject<List<GFile>>(json);

            return files;
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
