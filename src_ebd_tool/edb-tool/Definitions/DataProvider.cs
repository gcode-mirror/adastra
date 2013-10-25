using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace edb_tool
{
    public interface DataProvider
    {
        void AddSubject(string name, int sex, int age, int idexperiment, int iduser);
        DataTable ListSubjects(int iduser);
        DataTable ListSubjectsByExperimentId(int idexperiment, int iduser);
        void DeleteSubject(int id);
        void UpdateSubject(int id, string name, int sex, int? age, int idexperiment);

        void AddExperiment(string name, string comment, string description, int iduser);
        List<GExperiment> ListExperiments(int iduser);
        DataTable ListExperimentsByExperimentIdUserId(int idexperiment, int iduser);
        void DeleteExperiment(int id);
        void UpdateExperiment(int id, string name, string comment, string description);


        void AddModality(string name, string comment, string description);
        DataTable ListModalities();
        void DeleteModality(int id);
        void UpdateModality(int id, string name, string comment, string description);

        void AddTag(string name);
        DataTable ListTags();
        void DeleteTag(int id);
        void UpdateTag(int id, string name);
        long AssociateTags(int[] idtag, int idfile, int idexperiment);

        void AddModalityToExperiment(int idmodality, int idexperiment);
        DataTable ListModalitiesByExperimentSubjectID(int idexperiment);

        long AddFile(string name, string path);
        long AddFiles(string[] name, string[] path);
        void AssociateFile(int idexperiment, int idsubject, int idmodality, long idfile);
        DataTable ListFilesByExperimentSubjectModalityID(int idexperiment, int idsubject, int idmodality);
        void DeleteFilesByExperimentIdSubjectIdModalityId(int idexperiment, int idsubject, int idmodality);
        void DeleteFilesByFileId(int idfile);
        void DeleteFilesByFileIdFromListFile(int idfile);


        void AddUser(string firstname, string lastname, string username, string password, string email);
        bool VerifyUserPassword(string username, string password, out int userid);

        void UpdateFileTags(int[] idfiles, string tagLine);
    }
}
