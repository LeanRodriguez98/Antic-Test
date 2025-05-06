using AnticTest.Architecture.Map;
using System;

namespace AnticTest.Architecture.Entities
{
	public delegate void EntityEvent(Entity entity);

	public abstract class Entity
	{
		public Action OnThisEntityCreated;
		public Action OnThisEntityDestroyed;

		protected uint ID;
		protected Coordinate coordinate;

		protected Entity(Coordinate coordinate, uint ID)
		{
			this.coordinate = coordinate;
			this.ID = ID;
			OnThisEntityCreated?.Invoke();
		}

		public uint GetID()
		{
			return ID;
		}

		public Coordinate GetCoordinate()
		{
			return coordinate;
		}

		public virtual void SetCoordinate(Coordinate coordinate)
		{
			this.coordinate = coordinate;
		}
	}
}