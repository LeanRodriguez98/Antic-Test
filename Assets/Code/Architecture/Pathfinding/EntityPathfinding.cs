using AnticTest.Architecture.GameLogic;
using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using AnticTest.Systems.Provider;
using System.Collections.Generic;

namespace AnticTest.Architecture.Pathfinding
{
	public class EntityPathfinding<TCell, TCoordinate> : AStarPathfinding<TCell>, IPathfinder<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private ITransitabilityProvider transitabilityProvider;

		public EntityPathfinding(ITransitabilityProvider transitabilityProvider)
		{
			this.transitabilityProvider = transitabilityProvider;
		}

		public IEnumerable<TCell> FindPath(TCell origin, TCell destity, IEnumerable<TCell> graph)
		{
			return Find(origin, destity, graph);
		}

		protected override bool CoordinatesEquals(TCell A, TCell B)
		{
			return A == B;
		}

		protected override int Distance(TCell A, TCell B)
		{
			return A.GetCoordinate().Distance(B.GetCoordinate());
		}

		protected override IEnumerable<TCell> GetNeighbors(TCell coordinate)
		{
			List<TCell> neighbors = new List<TCell>();
			foreach (TCoordinate neighborCoordinate in coordinate.GetNeighbors())
			{
				TCell cell = Map[neighborCoordinate];
				if (cell != null)
					neighbors.Add(cell);
			}

			return neighbors;
		}

		protected override bool IsBloqued(TCell coordinate)
		{
			return transitabilityProvider.GetTransitability().intransitableCellTypes.Contains(coordinate.GetCellType());
		}

		protected override int MoveToNeighborCost(TCell A, TCell B)
		{
			Transitability transitability = transitabilityProvider.GetTransitability();
			int heightDifferentCost = A.GetHeight() == B.GetHeight() ? 0 : 
				A.GetHeight() > B.GetHeight() ? transitability.increaseHeightCost : transitability.decreaseHeightCost;
			return transitability.GetMoveToCellTypeCostAsDictionary()[B.GetCellType()] + heightDifferentCost;
		}
	}
}
