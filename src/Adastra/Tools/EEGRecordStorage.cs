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
    public class EEGRecordStorage
    {
        DB db;

        public EEGRecordStorage()
        {
            db = new DB(DbSettings.ConnectionString);
        }

        public void SaveRecord(EEGRecord record)
        {
            if (!File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.CreateDatabase(DbSettings.fullpath);
            }

            db.OpenDatabase(DbSettings.fullpath);
            db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

            db.Store(record);

            db.Close();
        }

        public List<EEGRecord> LoadModels()
        {
            List<EEGRecord> result = new List<EEGRecord>();

            if (File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.OpenDatabase(DbSettings.fullpath);

                db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

                var query = from EEGRecord sample in db select sample;

                result = query.ToList();

                db.Close();
            }
            return result;
        }

        public EEGRecord LoadModel(string name)
        {
            EEGRecord result=null;

            if (File.Exists(DbSettings.fullpath + ".eq"))
            {
                db.OpenDatabase(DbSettings.fullpath);

                db.RefreshMode = ObjectRefreshMode.AlwaysReturnUpdatedValues;

                result = (from EEGRecord sample in db
                            where sample.Name==name
                            select sample).FirstOrDefault();

                db.Close();
            }

            return result;
        }
    }
}
