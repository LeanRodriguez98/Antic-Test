using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;

namespace AnticTest.Architecture.Events
{
	public struct CellSelectedEvent<TCoordinate> : IEvent
		where TCoordinate : struct, ICoordinate
	{
		public TCoordinate selectedCoordinate;

		public CellSelectedEvent(TCoordinate selectedCoordinate)
		{
			this.selectedCoordinate = selectedCoordinate;
		}
	}
}
