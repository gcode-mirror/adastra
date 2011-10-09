using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Accord.Statistics.Analysis;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Eloquera.Client;
using Adastra.Algorithms;

namespace Adastra
{
    public static class EEGRecordStorage
    {
        static string fullpath
        {
            get
            {
                return Database.fullpath;
            }
        }

        static DB db
        {
            get
            {
                return Database.getDB;
            }
        }


        public static void SaveRecord(EEGRecord record)
        {
            bool justCreated = false;
            if (!File.Exists(fullpath + ".eq"))
            {
                db.CreateDatabase(fullpath);
                justCreated = true;
            }

            db.OpenDatabase(fullpath);
            db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

            if (justCreated)
            {
                db.RegisterType(typeof(AMLearning));
                db.RegisterType(typeof(LinearDiscriminantAnalysis));
                db.RegisterType(typeof(ActivationNetwork));
                db.RegisterType(typeof(Accord.MachineLearning.VectorMachines.MulticlassSupportVectorMachine));
                db.RegisterType(typeof(Accord.MachineLearning.VectorMachines.Learning.MulticlassSupportVectorLearning));
                db.RegisterType(typeof(Accord.MachineLearning.VectorMachines.Learning.SequentialMinimalOptimization));
                db.RegisterType(typeof(EEGRecord));
            }

            db.Store(record);

            db.Close();
        }

        public static List<EEGRecord> LoadModels()
        {
            List<EEGRecord> result = new List<EEGRecord>();

            if (File.Exists(fullpath + ".eq"))
            {
                db.OpenDatabase(fullpath);

                db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

                var query = from EEGRecord sample in db select sample;

                result = query.ToList();

                db.Close();
            }
            return result;
        }
    }
}
