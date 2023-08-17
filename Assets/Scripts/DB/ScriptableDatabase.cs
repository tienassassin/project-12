using UnityEngine;

namespace System.DB
{
    public abstract class ScriptableDatabase : ScriptableObject
    {
        public abstract void Import(params string[] data);
        public abstract void DeleteAll();
    }
}
