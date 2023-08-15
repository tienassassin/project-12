using UnityEngine;

namespace System.DB
{
    public abstract class ScriptableDatabase : ScriptableObject
    {
        internal abstract void Import(params string[] data);
        internal abstract void DeleteAll();
    }
}
