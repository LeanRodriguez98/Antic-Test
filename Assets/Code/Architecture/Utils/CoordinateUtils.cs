using AnticTest.DataModel.Grid;
using System.Collections.Generic;

namespace AnticTest.Architecture.Utils
{
	public static class CoordinateUtils<TCoordinate>
		where TCoordinate : struct , ICoordinate
	{
		public static List<TCoordinate> GetCoordinatesInRing(TCoordinate center, uint radius)
		{
			const int HEX_SIDES = 6;
			const int SOUTH_WEST_DIRECTION = 4;
			(int dx, int dy)[] DIRECTIONS_EVEN = new (int, int)[]
			{
				(1, 0), (0, -1), (-1, -1),
				(-1, 0), (-1, 1), (0, 1)
			};

			(int dx, int dy)[] DIRECTIONS_ODD = new (int, int)[]
			{
				(1, 0), (1, -1), (0, -1),
				(-1, 0), (0, 1), (1, 1)
			};

			List<TCoordinate> ring = new List<TCoordinate>();
			if (radius == 0)
			{
				ring.Add(center);
				return ring;
			}

			TCoordinate current = center;

			for (int i = 0; i < radius; i++)
			{
				(int dx, int dy) direction = current.Y % 2 == 0 ? DIRECTIONS_EVEN[SOUTH_WEST_DIRECTION] : DIRECTIONS_ODD[SOUTH_WEST_DIRECTION];
				current.Set(current.X + direction.dx, current.Y + direction.dy);
			}

			for (int i = 0; i < HEX_SIDES; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					ring.Add(current);
					(int dx, int dy) direction = current.Y % 2 == 0 ? DIRECTIONS_EVEN[i] : DIRECTIONS_ODD[i];
					current.Set(current.X + direction.dx, current.Y + direction.dy);
				}
			}

			return ring;
		}

		public static List<TCoordinate> GetCoordinatesInRadius(TCoordinate center, uint radius)
		{
			List<TCoordinate> coordinates = new List<TCoordinate>();
			coordinates.Add(center);

			for (uint i = 1; i <= radius; i++)
			{
				coordinates.AddRange(GetCoordinatesInRing(center, i));
			}

			return coordinates;
		}
	}
}
