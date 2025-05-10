using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Map Architecture Data", menuName = "AnticTest/Data/Architecture/Map")]
	public class MapArchitectureData : ArchitectureData
	{
		[SerializeField] private Vector2Int mapSize;
		[SerializeField] [Range(1.0f, 10.0f)] private float logicalDistanceBetweenCells = 1.0f;
		[SerializeField] private List<CellArchitectureDataEditorRepresentation> cellsInMap;
		[SerializeField] private List<EntityArchitectureDataEditorRepresentation> entitiesInMap;

		[SerializeField, HideInInspector] private GridMatrix<CellData> map;

#if UNITY_EDITOR
		public Vector2Int MapSize
		{
			get => mapSize;
			set => mapSize = value;
		}

		public List<CellArchitectureDataEditorRepresentation> CellsInMap => cellsInMap;

		public List<EntityArchitectureDataEditorRepresentation> EntitiesInMap => entitiesInMap;
#endif

		public GridMatrix<CellData> Map
		{
			get => map;
#if UNITY_EDITOR
			set => map = value;
#endif
		}

		public float LogicalDistanceBetweenCells => logicalDistanceBetweenCells;

		private void OnValidate()
		{
			if (mapSize.x <= 0 || mapSize.y <= 0)
			{
				mapSize = new Vector2Int(mapSize.x <= 0 ? 1 : mapSize.x,
										 mapSize.y <= 0 ? 1 : mapSize.y);
			}
		}

		[Serializable]
		public struct CellData
		{
			public CellArchitectureData cellArchitectureData;
			public ArchitectureData entityArchitectureData;
		}

		[Serializable]
		public struct CellArchitectureDataEditorRepresentation
		{
			[SerializeField] public CellArchitectureData cellArchitectureData;
			[SerializeField] public Color representativeColor;
		}

		[Serializable]
		public struct EntityArchitectureDataEditorRepresentation
		{
			[SerializeField] public ArchitectureData entityArchitectureData;
			[SerializeField] public char representativeChar;
		}
	}
}
