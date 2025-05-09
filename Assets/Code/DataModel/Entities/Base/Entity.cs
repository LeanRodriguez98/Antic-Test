using AnticTest.DataModel.Grid;
using System;

namespace AnticTest.DataModel.Entities
{
	public abstract class Entity<TCell, TCoordinate> : IEntity
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public delegate void EntityEvent(Entity<TCell, TCoordinate> entity);
		
		public Action OnThisEntityCreated;
		public Action OnThisEntityDestroyed;

		protected uint ID;
		protected TCoordinate coordinate;

		protected Entity(TCoordinate coordinate, uint ID)
		{
			this.coordinate = coordinate;
			this.ID = ID;
			OnThisEntityCreated?.Invoke();
		}

		protected abstract void SetParameters(params object[] parameters);

		public uint GetID()
		{
			return ID;
		}

		public (int x, int y) GetCoordinate()
		{
			return coordinate.GetCoordinate();
		}

		public virtual void SetCoordinate(TCoordinate coordinate)
		{
			this.coordinate = coordinate;
		}
	}
}