using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using AnticTest.Data.Architecture;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace AnticTest.Gameplay.Map
{
	[RequireComponent(typeof(Grid))]
	public class GameMap : MonoBehaviour, IService
	{
		private Grid<Cell> LogicalGrid => ServiceProvider.Instance.GetService<Grid<Cell>>();
		private Grid gameGrid;

		private Dictionary<Coordinate, GameObject> cellGameObjects;

		[SerializeField] private float cellHeight;

		private void Start()
		{
			gameGrid = GetComponent<Grid>();
			CreateGameMap();
			ServiceProvider.Instance.AddService<GameMap>(this);
		}

		public float GetCellHeight()
		{
			return cellHeight;
		}

		public Grid GetGrid()
		{
			return gameGrid;
		}

		private void CreateGameMap()
		{
			cellGameObjects = new Dictionary<Coordinate, GameObject>();
			Coordinate mapSize = LogicalGrid.GetSize();
			for (int x = 0; x < mapSize.x; x++)
			{
				for (int y = 0; y < mapSize.y; y++)
				{
					Cell cell = LogicalGrid[new Coordinate(x, y)];
					KeyValuePair<GameObject, int> prefab = GetPrefabFor(cell);
					GameObject newCell = Instantiate(prefab.Key, gameGrid.CellToWorld(new Vector3Int(x, y, 0)),
						prefab.Key.transform.rotation * Quaternion.Euler(0, 60.0f * prefab.Value, 0.0f), transform);
					newCell.isStatic = true;
				}
			}

			KeyValuePair<GameObject, int> GetPrefabFor(Cell cell)
			{
				IEnumerable<CellGameplayData> cellGameplayDatas = ServiceProvider.Instance.GetService<DataBlackboard>().GetGameplayDatas<CellGameplayData>();

				if (cell.GetHeight() == CellHeight.Zero)
					return new KeyValuePair<GameObject, int>(GetPrefabsFor(cell.GetCellType(), cell.GetHeight())[0], 0);

				for (int i = 0; i < CellGameplayData.Passability.Length; i++)
				{
					for (int j = 0; j < CellGameplayData.Passability[i].Length; j++)
					{
						bool[] current = new bool[]
						{
								cell.GetHeight() <= (LogicalGrid[cell.GetNeighbors()[j % 6]]        != null ? LogicalGrid[cell.GetNeighbors()[j % 6]].GetHeight()       : cell.GetHeight() - 1),
								cell.GetHeight() <= (LogicalGrid[cell.GetNeighbors()[(j+1) % 6]]    != null ? LogicalGrid[cell.GetNeighbors()[(j+1) % 6]].GetHeight()   : cell.GetHeight() - 1),
								cell.GetHeight() <= (LogicalGrid[cell.GetNeighbors()[(j+2) % 6]]    != null ? LogicalGrid[cell.GetNeighbors()[(j+2) % 6]].GetHeight()   : cell.GetHeight() - 1),
								cell.GetHeight() <= (LogicalGrid[cell.GetNeighbors()[(j+3) % 6]]    != null ? LogicalGrid[cell.GetNeighbors()[(j+3) % 6]].GetHeight()   : cell.GetHeight() - 1),
								cell.GetHeight() <= (LogicalGrid[cell.GetNeighbors()[(j+4) % 6]]    != null ? LogicalGrid[cell.GetNeighbors()[(j+4) % 6]].GetHeight()   : cell.GetHeight() - 1),
								cell.GetHeight() <= (LogicalGrid[cell.GetNeighbors()[(j+5) % 6]]    != null ? LogicalGrid[cell.GetNeighbors()[(j+5) % 6]].GetHeight()   : cell.GetHeight() - 1)
						};

						if (current.SequenceEqual(CellGameplayData.Passability[i]))
						{
							return new KeyValuePair<GameObject, int>(GetPrefabsFor(cell.GetCellType(), cell.GetHeight())[i], j);
						}
					}
				}
				return default;

				GameObject[] GetPrefabsFor(CellType cellType, CellHeight cellHeight)
				{
					foreach (CellGameplayData cellData in cellGameplayDatas)
					{
						CellArchitectureData ArchitectureData = cellData.architectureData as CellArchitectureData;
						if (ArchitectureData.cellType == cellType && ArchitectureData.cellHeight == cellHeight)
						{
							return cellData.prefafs;
						}
					}
					return null;
				}
			}
		}
	}
}
