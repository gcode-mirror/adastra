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
using Encog.MathUtil.RBF;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.RBF;
using Encog.Util.Simple;
using Encog.Neural.Networks.Training.Propagation.Resilient;

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
