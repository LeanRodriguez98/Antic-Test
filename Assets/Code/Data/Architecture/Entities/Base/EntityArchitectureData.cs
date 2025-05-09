using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;

namespace AnticTest.Data.Architecture
{
	public abstract class EntityArchitectureData<TEntity, TCell, TCoordinate> : ArchitectureData<TEntity>
		where TEntity : Entity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public abstract TEntity Get(TCoordinate coord);
		public abstract object[] GetParameters();
	}
}
