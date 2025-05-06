using AnticTest.Architecture.Map;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.Utils
{
	public static class CoordinateUtils
	{
		public static List<Coordinate> GetCoordinatesInRadius(Coordinate center, uint radius, bool includeCenter = true)
		{
			List<Coordinate> tilesInRadius = new List<Coordinate>();
			tilesInRadius.Add(center);

			if (radius == 0)
				return tilesInRadius;

			Coordinate Last() => tilesInRadius[tilesInRadius.Count - 1];
			Coordinate startRing = Last();

			for (int i = 1; i <= radius; i++)
			{
				tilesInRadius.Add(new Coordinate(startRing.y % 2 == 0 ? startRing.x - 1 : startRing.x, startRing.y + 1));
				startRing = Last();
				for (int j = 1; j <= i; j++)
					tilesInRadius.Add(new Coordinate(Last().x + 1, Last().y));
				for (int j = 1; j <= i; j++)
					tilesInRadius.Add(new Coordinate(Last().y % 2 == 0 ? Last().x : Last().x + 1, Last().y - 1));
				for (int j = 1; j <= i; j++)
					tilesInRadius.Add(new Coordinate(Last().y % 2 == 0 ? Last().x - 1 : Last().x, Last().y - 1));
				for (int j = 1; j <= i; j++)
					tilesInRadius.Add(new Coordinate(Last().x - 1, Last().y));
				for (int j = 1; j <= i; j++)
					tilesInRadius.Add(new Coordinate(Last().y % 2 == 0 ? Last().x - 1 : Last().x, Last().y + 1));
				for (int j = 1; j < i; j++)
					tilesInRadius.Add(new Coordinate(Last().y % 2 == 0 ? Last().x : Last().x + 1, Last().y + 1));
			}

			if (!includeCenter)
				tilesInRadius.RemoveAt(0);

			return tilesInRadius;
		}

		public static uint Distance(Coordinate a, Coordinate b)
		{
			int diffX = Math.Abs(b.x - a.x);
			int diffY = Math.Abs(b.y - a.y);
			return (uint)((((b.x - a.x < 0) ^ ((a.y % 2) != 0)) ?
				Math.Max(0, diffX - (diffY + 1) / 2) : Math.Max(0, diffX - (diffY) / 2)) + diffY);
		}
	}
}
