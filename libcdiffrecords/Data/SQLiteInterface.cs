using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace libcdiffrecords.Data
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

            SQLiteCommand survCmd = new SQLiteCommand(survTable, conn);
            SQLiteCommand admCmd = new SQLiteCommand(admTable, conn);
            SQLiteCommand storCmd = new SQLiteCommand(storTable, conn);

            conn.Open();
            survCmd.ExecuteNonQuery();
            admCmd.ExecuteNonQuery();
            storCmd.ExecuteNonQuery();

            conn.Close();
        }
        private void BuildStandardTables()
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
                                                                                "patient_age INT,"+
                                                                                "unit VARCHAR(5),"+
                                                                                "admission_date DATETIME," +
                                                                                "sample_count INT," +
                                                                                "status_on_first_sample VARCHAR(3)," +
                                                                                "admission_result VARCHAR(50));";

            string storageTable = "CREATE TABLE IF NOT EXISTS sample_storage (tube_id VARCHAR(13) PRIMARY KEY NOT NULL," +
                                                                             "sammple_id VARCHAR(10) NOT NULL," +
                                                                             "container_id VARCHAR(9)," +
                                                                             "additives VARCHAR(30)," +
                                                                             "notes VARCHAR(255));";

            conn.Open();
            SQLiteCommand survCmd = new SQLiteCommand(survTable, conn);
            SQLiteCommand admCmd = new SQLiteCommand(admissionTable, conn);
            SQLiteCommand storCmd = new SQLiteCommand(storageTable, conn);

            survCmd.ExecuteNonQuery();
            admCmd.ExecuteNonQuery();
            storCmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
