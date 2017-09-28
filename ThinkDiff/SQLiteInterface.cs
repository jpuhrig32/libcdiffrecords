using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using libcdiffrecords.Data;
using libcdiffrecords;

namespace ThinkDiff
{
    public class SQLiteInterface
    {
        SQLiteConnection conn;
        public SQLiteInterface(string dbFile, bool flushTable)
        {
            bool rebuildTables = flushTable;
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
                rebuildTables = true;
            }

            conn = new SQLiteConnection("Data Source=" + dbFile + ";Version=3;");

            if (rebuildTables)
            {
                FlushTables();
                BuildStandardTables();
            }
        }

        private void FlushTables()
        {
            string survTable = "DROP TABLE IF EXISTS surveillance_data;";
            string admTable = "DROP TABLE IF EXISTS admission_data;";
            string storTable = "DROP TABLE IF EXISTS sample_storage;";
            string settingsTable = "DROP TABLE IF EXISTS settings";

            SQLiteCommand survCmd = new SQLiteCommand(survTable, conn);
            SQLiteCommand admCmd = new SQLiteCommand(admTable, conn);
            SQLiteCommand storCmd = new SQLiteCommand(storTable, conn);
            SQLiteCommand setCmd = new SQLiteCommand(settingsTable, conn);

            conn.Open();
            survCmd.ExecuteNonQuery();
            admCmd.ExecuteNonQuery();
            storCmd.ExecuteNonQuery();

            conn.Close();
        }
        private async void BuildStandardTables()
        {
            await Task.Run(async () =>
            {
                string survTable = "CREATE TABLE IF NOT EXISTS surveillance_data ( sample_id VARCHAR(10) PRIMARY KEY NOT NULL," +
                                                                                   "admission_id VARCHAR(10)," +
                                                                                   "patient_name VARCHAR(30)," +
                                                                                   "mrn VARCHAR(8) NOT NULL," +
                                                                                   "sex char(1)," +
                                                                                   "dob DATETIME," +
                                                                                   "adm_date DATETIME," +
                                                                                   "sample_date DATETIME," +
                                                                                   "c_diff_test_result VARCHAR(3)," +
                                                                                   "toxin_result VARCHAR(3)," +
                                                                                   "test_type VARCHAR(30)," +
                                                                                   "unit VARCHAR(10)," +
                                                                                   "room VARCHAR(5)," +
                                                                                   "legacy_id VARCHAR(5)," +
                                                                                   "notes VARCHAR(255));";

                string admissionTable = "CREATE TABLE IF NOT EXISTS admission_data (admission_id VARCHAR(10) PRIMARY KEY NOT NULL," +
                                                                                    "mrn VARCHAR(8) NOT NULL," +
                                                                                    "patient_age INT," +
                                                                                    "unit VARCHAR(5)," +
                                                                                    "admission_date DATETIME," +
                                                                                    "sample_count INT," +
                                                                                    "status_on_first_sample VARCHAR(3)," +
                                                                                    "admission_result VARCHAR(50));";

                string storageTable = "CREATE TABLE IF NOT EXISTS sample_storage (container_id VARCHAR(9)," +
                                                                                 "box_position INT," +
                                                                                 "tube_accession VARCHAR(13) PRIMARY KEY NOT NULL," +
                                                                                 "tube_label VARCHAR(30)," +
                                                                                 "sample_id VARCHAR(10) NOT NULL," +
                                                                                 "additives VARCHAR(30)," +
                                                                                 "notes VARCHAR(255));";

                string settingsTable = "CREATE TABLE IF NOT EXISTS settings (property VARCHAR(50), value VARCHAR(50));";

                await ExecuteMultipleNonQueriesAsync(new String[4] { survTable, admissionTable, storageTable, settingsTable });
            });
        }

        public async Task<int> ExecuteMultipleNonQueriesAsync(String[] queries)
        {
            return await Task.Run(async () =>
            {
                int count = 0;
                if(conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();
                for (int i = 0; i < queries.Length; i++)
                {
                    using(SQLiteCommand cmd = new SQLiteCommand(queries[i], conn))
                        count += await cmd.ExecuteNonQueryAsync();
                }
             
                return count;
            });
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            return await Task.Run(async () => 
                {
                    int count = 0;

                    if(conn.State == ConnectionState.Closed)
                         await conn.OpenAsync();
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    count = await cmd.ExecuteNonQueryAsync();
                   

                    return count;
                });
        }

        public void CloseConnection()
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }
        public async Task<DbDataReader> ExecuteDataReaderQueryAsync(string query)
        {
           return await Task.Run(async () =>
            {
                if(conn.State == ConnectionState.Closed)
                    await conn.OpenAsync();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);

                DbDataReader dr =  await cmd.ExecuteReaderAsync();
            
                return dr;

            });
        }

        /// <summary>
        /// Returns a dictionary of the table column names, as well as their datatypes.
        /// for a given table
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <returns>Dictionary of column names and datatypes</returns>
        public async Task<Dictionary<string, string>> GetTableFields(string tableName)
        {
            return await Task.Run(async () =>
                {
                    Dictionary<string, string> tablesAndTypes = new Dictionary<string, string>();
                    string query = "PRAGMA table_info(" + tableName + " );";
                    DbDataReader dr = await ExecuteDataReaderQueryAsync(query);

                    while(await dr.ReadAsync())
                    {
                        if (!tablesAndTypes.ContainsKey(dr.GetString(1)))
                            tablesAndTypes.Add(dr.GetString(1), dr.GetString(2));
                    }
                    return tablesAndTypes;
                });
        }

        public async Task<Dictionary<string,string>> GetSettingsDataFromDatabaseAsync(string query)
        {

            return await Task.Run(async () =>
            {
                Dictionary<string, string> settingsResults = new Dictionary<string, string>();
                if(conn.State == ConnectionState.Closed)
                     await conn.OpenAsync();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string key = reader.GetString(0);
                    if (!settingsResults.ContainsKey(key))
                        settingsResults.Add(key, reader.GetString(1));
                }
                return settingsResults;
            });
        }

        public Dictionary<string, string> GetSettingsDataFromDatabase(string query)
        {
            Dictionary<string, string> settingsResults = new Dictionary<string, string>();
            if (conn.State == ConnectionState.Closed)
               conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            DbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string key = reader.GetString(0);
                if (!settingsResults.ContainsKey(key))
                    settingsResults.Add(key, reader.GetString(1));
            }
            return settingsResults;
        }

        public async Task<Bin> PullDataFromDatabase(string query)
        {
            return await Task.Run(async () =>
            {
                Bin retBin = new Bin("NewData");
                DbDataReader dr = await ExecuteDataReaderQueryAsync(query);
                Dictionary<string, bool> stdFields = AppData.ProduceStandardFieldTable();

                while (await dr.ReadAsync())
                {
                    retBin.Add(PullDataPointFromDataReader(dr));

                }

                return retBin;
            });
        }

        private DataPoint PullDataPointFromDataReader(DbDataReader dr)
        {
            DataPoint dp = new DataPoint();
            dp.Initalize();
            dp.SampleID = dr.GetString(0);
            dp.AdmissionID = dr.GetString(1);
            dp.PatientName = dr.GetString(2);
            dp.MRN = dr.GetString(3);
            dp.PatientSex = Utilities.ParseSexFromString(dr.GetString(4));
            dp.DateOfBirth = DateTime.Parse(dr.GetString(5));
            dp.AdmissionDate = DateTime.Parse(dr.GetString(6));
            dp.SampleDate = DateTime.Parse(dr.GetString(7));
            dp.CdiffResult = Utilities.ParseTestResult(dr.GetString(8));
            dp.ToxinResult = Utilities.ParseTestResult(dr.GetString(9));
            dp.Test = Utilities.ParseTestTypeFromString(dr.GetString(10));
            dp.Unit = dr.GetString(11);
            dp.Room = dr.GetString(12);
            dp.LegacyID = dr.GetString(13);
            dp.Notes = dr.GetString(14);

            for(int i = 15; i < dr.FieldCount; i++)
            {
                dp.Fields.Add(dr.GetName(i), dr.GetString(i));
            }


            return dp;
        }
    }
}
