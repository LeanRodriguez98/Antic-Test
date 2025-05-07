using UnityEngine;

namespace AnticTest.Data.Architecture
{
    public abstract class ArchitectureData<T> : ScriptableObject
    {
        protected abstract T Get(params object[] parameters);
    }
}
