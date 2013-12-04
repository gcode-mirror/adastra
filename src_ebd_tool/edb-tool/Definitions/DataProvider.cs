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
        //used in Manage Subjects when you want ot see all subjects from all experiments (does not include shared ones)
        List<GSubject> ListSubjects(int iduser);
        List<GSubject> ListSubjectsByExperimentId(int idexperiment, int iduser);
        void DeleteSubject(int id);
        void UpdateSubject(GSubject s);

        //experiment
        void AddExperiment(GExperiment exp);
        List<GExperiment> ListExperiments(int iduser);
        List<GExperiment> ListExperimentsByExperimentIdUserId(int idexperiment, int iduser);
        void DeleteExperiment(int id);
        void UpdateExperiment(GExperiment exp);

        //share experiment
        void AddSharedExperiment(int idexperiment, int owneruserid, int targetuserid);
        List<GUser> ListTagetUsers(int idexperiment, int owner_userid); //List the users to which the selcted experiment has been shared
        List<GExperiment> ListExperimentsSharedToTheUserByOthers(int iduser);
        void DeleteSharedExperiment(int idexperiment, int owner_userid); //deletes to whom it is shared

        void AddModality(GModality m);
        List<GModality> ListModalities();
        void DeleteModality(int id);
        void UpdateModality(GModality m);

        void AddTag(GTag tag);
        List<GTag> ListTags();
        void DeleteTag(int id);
        void UpdateTag(GTag tag);
        void AssociateTags(List<GTag> tags, int idfile, int idexperiment);
        void RemoveTags(int idfile);

        void AddModalityToExperiment(int idmodality, int idexperiment);
        List<GModality> ListModalitiesByExperimentID(int idexperiment);

        long AddFile(GFile file);
        void AddFiles(List<GFile> files);
        
        //connect a file with experiment, subject
        void AssociateFile(int idexperiment, int idsubject, int idmodality, long idfile);
        List<GFile> ListFilesByExperimentSubjectModalityID(int idexperiment, int idsubject, int idmodality);
        
        //used when the entire modality is deleted which involves removing all the associated files
        void DeleteFilesByExperimentIdSubjectIdModalityId(int idexperiment, int idsubject, int idmodality);
        
        #region Removing a signle file requires 3 operations: 
        void DeleteFilesByFileId(int idfile);
        void DeleteFilesByFileIdFromListFile(int idfile);
        //remove tags
        #endregion

        #region users

        void AddUser(GUser u,string password);//by the web spider 
        bool VerifyUserPassword(string username, string password, out int userid);
        List<GUser> ListUsers();

        #endregion

        void UpdateFileTags(int[] idfiles, string tagLine);
    }
}
