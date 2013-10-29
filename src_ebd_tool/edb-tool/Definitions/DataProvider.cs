using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace edb_tool
{
    public interface DataProvider
    {
        void AddSubject(GSubject subject);
        List<GSubject> ListSubjects(int iduser);
        List<GSubject> ListSubjectsByExperimentId(int idexperiment, int iduser);
        void DeleteSubject(int id);
        void UpdateSubject(GSubject s);

        void AddExperiment(GExperiment exp);
        List<GExperiment> ListExperiments(int iduser);
        List<GExperiment> ListExperimentsByExperimentIdUserId(int idexperiment, int iduser);
        void DeleteExperiment(int id);
        void UpdateExperiment(GExperiment exp);


        void AddModality(GModality m);
        List<GModality> ListModalities();
        void DeleteModality(int id);
        void UpdateModality(GModality m);

        void AddTag(string name);
        DataTable ListTags();
        void DeleteTag(int id);
        void UpdateTag(int id, string name);
        long AssociateTags(int[] idtag, int idfile, int idexperiment);

        void AddModalityToExperiment(int idmodality, int idexperiment);
        List<GModality> ListModalitiesByExperimentID(int idexperiment);

        long AddFile(string name, string path);
        long AddFiles(string[] name, string[] path);
        void AssociateFile(int idexperiment, int idsubject, int idmodality, long idfile);
        DataTable ListFilesByExperimentSubjectModalityID(int idexperiment, int idsubject, int idmodality);
        void DeleteFilesByExperimentIdSubjectIdModalityId(int idexperiment, int idsubject, int idmodality);
        void DeleteFilesByFileId(int idfile);
        void DeleteFilesByFileIdFromListFile(int idfile);


        void AddUser(string firstname, string lastname, string username, string password, string email);//by the web spider 
        bool VerifyUserPassword(string username, string password, out int userid);

        void UpdateFileTags(int[] idfiles, string tagLine);
    }
}
