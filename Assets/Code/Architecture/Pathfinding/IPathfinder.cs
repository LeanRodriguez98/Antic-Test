using AnticTest.DataModel.Grid;
using System.Collections.Generic;

namespace AnticTest.Architecture.Pathfinding
{
	public interface IPathfinder<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public IEnumerable<TCell> FindPath(TCell origin, TCell destity, IEnumerable<TCell> graph);
	}
}
