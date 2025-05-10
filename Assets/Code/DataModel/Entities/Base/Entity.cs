using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;

namespace AnticTest.DataModel.Entities
{
	public abstract class Entity<TCell, TCoordinate> : IEntity
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		protected uint ID;
		protected TCoordinate coordinate;

		protected Entity(TCoordinate coordinate, uint ID)
		{
			this.coordinate = coordinate;
			this.ID = ID;
		}

		protected abstract void SetParameters(params object[] parameters);

		public uint GetID()
		{
			return ID;
		}

		public ICoordinate GetCoordinate()
		{
			return coordinate.GetCoordinate();
		}

		public virtual void SetCoordinate(TCoordinate coordinate)
		{
			this.coordinate = coordinate;
		}
	}
}