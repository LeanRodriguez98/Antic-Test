using AnticTest.Data.Architecture;
using System;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	public abstract class GameplayData<T> : ScriptableObject
	{
		public ArchitectureData<T> architectureData;
		public Type GetDataType() => typeof(T);
	}
}