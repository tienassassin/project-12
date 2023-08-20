using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DB.System
{
    public abstract class Database : DuztineBehaviour
    {
        public bool readOnlyMode = true;
        
        [DisableIf("readOnlyMode")] 
        public string[] sheets;

        protected void Awake()
        {
            Import();
        }

        [Button]
        protected abstract void Import();
        protected abstract void DeleteAll();
    }

    public static partial class DatabaseExtensions
    {
        private static string path = "Databases/";

        private static string GetDatabasePath(string sheet)
        {
            return $"{path}{sheet}_DB";
        }
        public static string FetchFromLocal(this Database db, int index)
        {
            return Resources.Load<TextAsset>(GetDatabasePath(db.sheets[index]))?.text ?? null;
        }
        
        public static string FetchFromLocal(this Database db, string sheet)
        {
            return Resources.Load<TextAsset>(GetDatabasePath(sheet))?.text ?? null;
        }

        public static List<string> FetchFromLocal(this Database db)
        {
            var data = new List<string>();
            foreach (var sheet in db.sheets)
            {
                data.Add(db.FetchFromLocal(sheet));
            }

            return data;
        }

        public static string FetchFromCloud(this Database db, int index)
        {
            return null;
        }
        
        public static string FetchFromCloud(this Database db, string sheet)
        {
            return null;
        }
        
        public static List<string> FetchFromCloud(this Database db)
        {
            return null;
        }
    }
}

