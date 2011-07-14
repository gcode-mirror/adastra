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
    public static class ModelStorage
    {
        const string dbName = "AdastraDB";

        static string fullpath = Environment.CurrentDirectory + "\\" + dbName;

        //var db = new DB("server=(local);options=none;");
        static DB db = new DB("server=(local);password=;options=inmemory,persist;");//in-memory save on exit

        public static void SaveModel(IMLearning model)
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
                db.RegisterType(typeof(IMLearning));
                db.RegisterType(typeof(LinearDiscriminantAnalysis));
                db.RegisterType(typeof(ActivationNetwork));
                db.RegisterType(typeof(Accord.MachineLearning.VectorMachines.MulticlassSupportVectorMachine));
                db.RegisterType(typeof(Accord.MachineLearning.VectorMachines.Learning.MulticlassSupportVectorLearning));
                db.RegisterType(typeof(Accord.MachineLearning.VectorMachines.Learning.SequentialMinimalOptimization));
            }

            db.Store(model);

            db.Close();
        }

        public static List<IMLearning> LoadModels()
        {
            List<IMLearning> result=new List<IMLearning>();

            if (File.Exists(fullpath + ".eq"))
            {
                db.OpenDatabase(fullpath);

                db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

                var query = from IMLearning sample in db select sample;

                result = query.ToList();
            }
            return result;
        }
    }
}
