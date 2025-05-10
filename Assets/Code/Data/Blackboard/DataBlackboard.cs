using AnticTest.Data.Architecture;
using AnticTest.Data.Gameplay;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AnticTest.Data.Blackboard
{
	public class DataBlackboard : IService
	{
		private const string RESOURCES_DATA_FOLDER = "Data";

		private const string ENTITY_ARCHITECTURE_DATA_FIELD_NAME = "architectureData";
		private const string ENTITY_PREFAB_FIELD_NAME = "prefab";

		private Dictionary<Type, List<ArchitectureData>> architectureDatas;
		private Dictionary<Type, List<GameplayData>> gameplayDatas;

		private Dictionary<ArchitectureData, GameObject> prefabsFromArchitectureData;

		public DataBlackboard()
		{
			architectureDatas = new Dictionary<Type, List<ArchitectureData>>();
			gameplayDatas = new Dictionary<Type, List<GameplayData>>();
			LoadData(ref architectureDatas);
			LoadData(ref gameplayDatas);
			LoadEntityPrefabsFromData();
		}

		private void LoadData<DataType>(ref Dictionary<Type, List<DataType>> datas) where DataType : ScriptableObject
		{
			IEnumerable<DataType> allDatas = Resources.LoadAll<DataType>(RESOURCES_DATA_FOLDER);

			foreach (DataType data in allDatas)
			{
				Type dataType = data.GetType();

				if (!datas.ContainsKey(dataType))
					datas.Add(dataType, new List<DataType>());
				datas[dataType].Add(data);
			}
		}

		private void LoadEntityPrefabsFromData()
		{
			prefabsFromArchitectureData = new Dictionary<ArchitectureData, GameObject>();

			foreach (List<GameplayData> gameplayDatasList in gameplayDatas.Values)
			{
				foreach (GameplayData gameplayData in gameplayDatasList)
				{
					if (gameplayData.GetType().InheritsFromRawGeneric(typeof(EntityGameplayData<,,>)))
					{
						Type dataType = gameplayData.GetType();

						FieldInfo architectureProp = dataType.GetField(ENTITY_ARCHITECTURE_DATA_FIELD_NAME, BindingFlags.Instance | BindingFlags.Public);
						FieldInfo prefabProp = dataType.GetField(ENTITY_PREFAB_FIELD_NAME, BindingFlags.Instance | BindingFlags.Public);

						if (architectureProp != null && prefabProp != null)
						{
							object architectureData = architectureProp.GetValue(gameplayData);
							object prefab = prefabProp.GetValue(gameplayData);

							if (architectureData is ArchitectureData && prefab is GameObject)
							{
								if (!prefabsFromArchitectureData.ContainsKey(architectureData as ArchitectureData))
									prefabsFromArchitectureData.Add(architectureData as ArchitectureData, prefab as GameObject);
							}
						}
					}
				}
			}
		}


		public DataType GetArchitectureData<DataType>(string name) where DataType : ArchitectureData
		{
			if (architectureDatas.ContainsKey(typeof(DataType)))
			{
				foreach (ArchitectureData data in architectureDatas[typeof(DataType)])
				{
					if (data.name == name)
					{
						return data as DataType;
					}
				}
			}
			return null;
		}

		public IEnumerable<DataType> GetArchitectureDatas<DataType>() where DataType : ArchitectureData
		{
			if (architectureDatas.ContainsKey(typeof(DataType)))
				return architectureDatas[typeof(DataType)].OfType<DataType>();
			return null;
		}

		public DataType GetGameplayData<DataType>(string name) where DataType : GameplayData
		{
			if (gameplayDatas.ContainsKey(typeof(DataType)))
			{
				foreach (GameplayData data in gameplayDatas[typeof(DataType)])
				{
					if (data.name == name)
					{
						return data as DataType;
					}
				}
			}
			return null;
		}

		public IEnumerable<DataType> GetGameplayDatas<DataType>() where DataType : GameplayData
		{
			if (gameplayDatas.ContainsKey(typeof(DataType)))
				return gameplayDatas[typeof(DataType)].OfType<DataType>();
			return null;
		}

		public GameObject GetPrefabFromData(ArchitectureData architectureData)
		{
			return prefabsFromArchitectureData[architectureData];
		}
	}
}
