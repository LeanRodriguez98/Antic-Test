using UnityEngine;

namespace AnticTest.Data.Architecture
{
	public abstract class ArchitectureData<T> : ArchitectureData
    {
        protected abstract T Get(params object[] parameters);
    }

    public abstract class ArchitectureData : ScriptableObject {}
}
