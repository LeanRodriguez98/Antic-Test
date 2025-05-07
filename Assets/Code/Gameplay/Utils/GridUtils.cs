using AnticTest.DataModel.Map;
using AnticTest.Services.Provider;
using AnticTest.Gameplay.Map;
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

		public static Vector2Int ToVector2Int(Coordinate coordinate)
		{
			return new Vector2Int(coordinate.x, coordinate.y);
		}

		public static Vector3Int ToVector3Int(Coordinate coordinate)
		{
			return new Vector3Int(coordinate.x, coordinate.y);
		}

		public static Vector3Int ToVector3Int(Vector2Int vector2Int)
		{
			return new Vector3Int(vector2Int.x, vector2Int.y);
		}

		public static Vector3 CoordinateToWorld(Coordinate coordinate)
		{
			GameMap gameMap = ServiceProvider.Instance.GetService<GameMap>();
			return gameMap.GetGrid().CellToWorld(ToVector3Int(coordinate)) + Vector3.up * gameMap.GetCellHeight();
		}
	}
}
