using AnticTest.DataModel.Grid;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.Utils
{
	public static class CoordinateUtils
	{
		public static List<Coordinate> GetCoordinatesInRadius(Coordinate center, uint radius, bool includeCenter = true)
		{
			List<Coordinate> cellInRadius = new List<Coordinate>();
			cellInRadius.Add(center);

			if (radius == 0)
				return cellInRadius;

			Coordinate Last() => cellInRadius[cellInRadius.Count - 1];
			Coordinate startRing = Last();

			for (int i = 1; i <= radius; i++)
			{
				cellInRadius.Add(new Coordinate(startRing.Y % 2 == 0 ? startRing.X - 1 : startRing.X, startRing.Y + 1));
				startRing = Last();
				for (int j = 1; j <= i; j++)
					cellInRadius.Add(new Coordinate(Last().X + 1, Last().Y));
				for (int j = 1; j <= i; j++)
					cellInRadius.Add(new Coordinate(Last().Y % 2 == 0 ? Last().X : Last().X + 1, Last().Y - 1));
				for (int j = 1; j <= i; j++)
					cellInRadius.Add(new Coordinate(Last().Y % 2 == 0 ? Last().X - 1 : Last().X, Last().Y - 1));
				for (int j = 1; j <= i; j++)
					cellInRadius.Add(new Coordinate(Last().X - 1, Last().Y));
				for (int j = 1; j <= i; j++)
					cellInRadius.Add(new Coordinate(Last().Y % 2 == 0 ? Last().X - 1 : Last().X, Last().Y + 1));
				for (int j = 1; j < i; j++)
					cellInRadius.Add(new Coordinate(Last().Y % 2 == 0 ? Last().X : Last().X + 1, Last().Y + 1));
			}

			if (!includeCenter)
				cellInRadius.RemoveAt(0);

			return cellInRadius;
		}
	}
}
