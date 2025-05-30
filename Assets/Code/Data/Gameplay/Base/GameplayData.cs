using AnticTest.Data.Architecture;
using System;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	public abstract class GameplayData<T> : GameplayData
	{
		public ArchitectureData<T> architectureData;
		public Type GetDataType() => typeof(T);
	}

	public abstract class GameplayData : ScriptableObject { }
}