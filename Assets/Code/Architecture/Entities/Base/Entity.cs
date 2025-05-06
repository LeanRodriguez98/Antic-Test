using AnticTest.Architecture.Map;
using System;

namespace AnticTest.Architecture.Entities
{
	public delegate void EntityEvent(Entity entity);

	public abstract class Entity
	{
		public static EntityEvent OnEntityCreated;
		public static EntityEvent OnEntityDestroyed;

		public Action OnThisEntityCreated;
		public Action OnThisEntityDestroyed;

		private static uint lastEntityID;

		protected uint ID;
		protected Coordinate coordinate;

		protected Entity(Coordinate coordinate)
		{
			GenerateID();
			this.coordinate = coordinate;
			OnEntityCreated?.Invoke(this);
			OnThisEntityCreated?.Invoke();
		}

		protected void GenerateID()
		{
			ID = lastEntityID;
			lastEntityID++;
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