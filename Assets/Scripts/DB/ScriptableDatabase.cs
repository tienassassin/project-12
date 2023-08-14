using UnityEngine;

namespace System.DB
{
    internal abstract class ScriptableDatabase : ScriptableObject
    {
        internal abstract void Import(params string[] data);
        internal abstract void DeleteAll();
    }
}
