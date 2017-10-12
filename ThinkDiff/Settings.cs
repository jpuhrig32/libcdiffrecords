using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;

namespace ThinkDiff
{
    public static class Settings
    {
        public static string DatabaseFile { get; set; }
        public static int AdmissionWindow { get; set; }
        public static int FieldCharWidth { get; set; }
        public static SQLiteInterface DataInterface { get; set; }
        
        


        private static void SetDefaults()
        {
            DatabaseFile = "./Cdiffdata.db";
            FieldCharWidth = 50;
            AdmissionWindow = 3;
        }

        public async static Task LoadSettings()
        {
            await Task.Run(async() =>
            { 
            SetDefaults();
            bool buildTables = false;
            if (!File.Exists(DatabaseFile))
            {
                buildTables = true;
            }
            DataInterface = new SQLiteInterface(DatabaseFile, buildTables);

            if (buildTables)
               await SaveSettings();
            else
            {
                string query = "SELECT * FROM settings";

                Dictionary<string, string> settingsResults = DataInterface.GetSettingsDataFromDatabase(query);



                DatabaseFile = GetSetting("DatabaseFile", DatabaseFile, settingsResults);
                AdmissionWindow = int.Parse(GetSetting("AdmissionWindow", AdmissionWindow.ToString(), settingsResults));
                FieldCharWidth = int.Parse(GetSetting("FieldCharWidth", FieldCharWidth.ToString(), settingsResults));
            }
        });

        }

        private static string GetSetting(String settingName, String defaultVal, Dictionary<string, string> results)
        {
            if (results.ContainsKey(settingName))
                return results[settingName]; ;
            return defaultVal;
        }
        public async static Task SaveSettings()
        {
            await Task.Run(async () =>
            {
                List<string> queries = new List<string>();
                queries.Add(BuildSaveSettingQuery("DatabaseFile", DatabaseFile));
                queries.Add(BuildSaveSettingQuery("AdmissionWindow", AdmissionWindow.ToString()));
                queries.Add(BuildSaveSettingQuery("FieldCharWidth", FieldCharWidth.ToString()));

                await DataInterface.ExecuteMultipleNonQueriesAsync(queries.ToArray());
            });
        }

        private static string BuildSaveSettingQuery(string queryName, string queryValue)
        {
            return "INSERT OR UPDATE INTO settings VALUES('" + queryName + "', '" + queryValue + "');";
        }





    }
}
