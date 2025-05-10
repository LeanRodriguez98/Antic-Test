using AnticTest.Architecture.GameLogic;
using AnticTest.DataModel.Grid;
using AnticTest.Gameplay.Components;
using AnticTest.Systems.Provider;
using UnityEngine;

namespace AnticTest.Gameplay.Utils
{
	public static class GridUtils
	{
		public static Coordinate ToCoordinate(Vector2Int vector2Int)
		{
			return new Coordinate(vector2Int.x, vector2Int.y);
		}

		public static Coordinate ToCoordinate(Vector3Int vector3Int)
		{
			return new Coordinate(vector3Int.x, vector3Int.y);
		}

		public static Vector2Int ToVector2Int(ICoordinate coordinate)
		{
			return new Vector2Int(coordinate.X, coordinate.Y);
		}

		public static Vector3Int ToVector3Int(ICoordinate coordinate)
		{
			return new Vector3Int(coordinate.X, coordinate.Y);
		}

		public static Vector3Int ToVector3Int(Vector2Int vector2Int)
		{
			return new Vector3Int(vector2Int.x, vector2Int.y);
		}

		public static Vector3 CoordinateToWorld(ICoordinate coordinate)
		{
			GameMap gameMap = ServiceProvider.Instance.GetService<GameMap>();
			Map<Cell<Coordinate>, Coordinate> logicalMap = ServiceProvider.Instance.GetService<Map<Cell<Coordinate>, Coordinate>>();
			return gameMap.GetGrid().CellToWorld(ToVector3Int(coordinate)) +
				Vector3.up * gameMap.GetCellHeight() * (float)logicalMap.GetCell(coordinate).GetHeight();
		}
	}
}
