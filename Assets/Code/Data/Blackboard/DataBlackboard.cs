using AnticTest.Services.Provider;
using AnticTest.Data.Architecture;
using AnticTest.Data.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnticTest.Data.Blackboard
{
	public class DataBlackboard : IService
	{
		private Dictionary<Type, List<ScriptableObject>> architectureDatas;
		private Dictionary<Type, List<ScriptableObject>> gameplayDatas;

		public DataBlackboard()
		{
			architectureDatas = new Dictionary<Type, List<ScriptableObject>>();
			gameplayDatas = new Dictionary<Type, List<ScriptableObject>>();
			LoadData();
			ServiceProvider.Instance.AddService<DataBlackboard>(this);
		}

		private void LoadData()
		{
			IEnumerable<ScriptableObject> allDatas = Resources.LoadAll<ScriptableObject>("Data");

			foreach (ScriptableObject data in allDatas)
			{
				Type dataType = data.GetType();

				if (dataType.InheritsFromRawGeneric(typeof(ArchitectureData<>)))
				{
					if (!architectureDatas.ContainsKey(dataType))
						architectureDatas.Add(dataType, new List<ScriptableObject>());
					architectureDatas[dataType].Add(data);
				}
				else if (data.GetType().InheritsFromRawGeneric(typeof(GameplayData<>)))
				{
					if (!gameplayDatas.ContainsKey(dataType))
						gameplayDatas.Add(dataType, new List<ScriptableObject>());
					gameplayDatas[dataType].Add(data);
				}
				else
				{
					Debug.LogWarning("The data object '" + data.name + "' is neither an ArchitectureData nor a GameplayData, but it is located in the Data folder!");
				}
			}
		}

		public IEnumerable<DataType> GetArchitectureDatas<DataType>() where DataType : ScriptableObject 
		{
			if (architectureDatas.ContainsKey(typeof(DataType)))
				return architectureDatas[typeof(DataType)].OfType<DataType>();
			return null;
		}

		public IEnumerable<DataType> GetGameplayDatas<DataType>() where DataType : ScriptableObject
		{
			if (gameplayDatas.ContainsKey(typeof(DataType)))
				return gameplayDatas[typeof(DataType)].OfType<DataType>();
			return null;
		}
	}
}
