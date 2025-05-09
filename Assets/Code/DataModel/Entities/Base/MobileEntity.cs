using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using System;

namespace AnticTest.DataModel.Entities
{
	public abstract class MobileEntity<TCell, TCoordinate> : Entity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public static EntityEvent OnEntityMoved;
		public CoordinateEvent OnThisEntityMoved;

		protected int speed;
		protected int health;

		public MobileEntity(TCoordinate coordinate, uint ID) : base(coordinate, ID) { }

		public override void SetCoordinate(TCoordinate coordinate)
		{
			base.SetCoordinate(coordinate);
			OnEntityMoved?.Invoke(this);
			OnThisEntityMoved?.Invoke(this.coordinate);
		}

		protected override void SetParameters(params object[] parameters)
		{
			speed = (int)parameters[0];
			health = (int)parameters[1];
		}
	}
}
