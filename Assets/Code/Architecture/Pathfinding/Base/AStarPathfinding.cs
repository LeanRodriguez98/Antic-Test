using System.Collections.Generic;

namespace AnticTest.Architecture.Pathfinding
{
	public abstract class AStarPathfinding<TCoordinate>
	{
		protected List<TCoordinate> Find(TCoordinate startCoordinate, TCoordinate goalCoordinate, IEnumerable<TCoordinate> graph)
		{
			Dictionary<TCoordinate, (TCoordinate Parent, int AcumulativeCost, int Heuristic)> coordinates = new Dictionary<TCoordinate, (TCoordinate, int, int)>();
			foreach (TCoordinate coordinate in graph)
			{
				coordinates.Add(coordinate, (default, 0, 0));
			}

			List<TCoordinate> openList = new List<TCoordinate>();
			List<TCoordinate> closedList = new List<TCoordinate>();

			openList.Add(startCoordinate);

			while (openList.Count > 0)
			{
				TCoordinate currentCoordinate = openList[0];
				int currentIndex = 0;

				for (int i = 1; i < openList.Count; i++)
				{
					if (coordinates[openList[i]].AcumulativeCost + coordinates[openList[i]].Heuristic <
					coordinates[currentCoordinate].AcumulativeCost + coordinates[currentCoordinate].Heuristic)
					{
						currentCoordinate = openList[i];
						currentIndex = i;
					}
				}

				openList.RemoveAt(currentIndex);
				closedList.Add(currentCoordinate);

				if (CoordinatesEquals(currentCoordinate, goalCoordinate))
				{
					return GeneratePath(startCoordinate, goalCoordinate);
				}

				foreach (TCoordinate neighbor in GetNeighbors(currentCoordinate))
				{
					if (!coordinates.ContainsKey(neighbor) ||
					IsBloqued(neighbor) ||
					closedList.Contains(neighbor))
					{
						continue;
					}

					int tentativeNewAcumulatedCost = 0;
					tentativeNewAcumulatedCost += coordinates[currentCoordinate].AcumulativeCost;
					tentativeNewAcumulatedCost += MoveToNeighborCost(currentCoordinate, neighbor);

					if (!openList.Contains(neighbor) || tentativeNewAcumulatedCost < coordinates[currentCoordinate].AcumulativeCost)
					{
						coordinates[neighbor] = (currentCoordinate, tentativeNewAcumulatedCost, Distance(neighbor, goalCoordinate));

						if (!openList.Contains(neighbor))
						{
							openList.Add(neighbor);
						}
					}
				}
			}

			return null;

			List<TCoordinate> GeneratePath(TCoordinate startCoordinate, TCoordinate goalCoordinate)
			{
				List<TCoordinate> path = new List<TCoordinate>();
				TCoordinate currentCoordinate = goalCoordinate;

				while (!CoordinatesEquals(currentCoordinate, startCoordinate))
				{
					path.Add(currentCoordinate);
					currentCoordinate = coordinates[currentCoordinate].Parent;
				}

				path.Add(startCoordinate);
				path.Reverse();
				return path;
			}
		}

		protected abstract IEnumerable<TCoordinate> GetNeighbors(TCoordinate coordinate);

		protected abstract int Distance(TCoordinate A, TCoordinate B);

		protected abstract bool CoordinatesEquals(TCoordinate A, TCoordinate B);

		protected abstract int MoveToNeighborCost(TCoordinate A, TCoordinate b);

		protected abstract bool IsBloqued(TCoordinate coordinate);
	}
}