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
    public class ModelStorage
    {
        DB db;

        public ModelStorage()
        {
            db = new DB(DbSettings.ConnectionString);
        }

        public void SaveModel(AMLearning model)
        {
            bool justCreated = false;
            if (!File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.CreateDatabase(DbSettings.fullpath);
                justCreated = true;
            }

            db.OpenDatabase(DbSettings.fullpath);
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

            db.Store(model);

            db.Close();
        }

        public List<AMLearning> LoadModels()
        {
            List<AMLearning> result=new List<AMLearning>();

            if (File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.OpenDatabase(DbSettings.fullpath);

                db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

                var query = from AMLearning sample in db select sample;

                result = query.ToList();

                db.Close();
            }
            return result;
        }
    }
}
