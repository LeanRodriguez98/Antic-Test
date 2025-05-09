using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;

namespace AnticTest.Data.Gameplay
{
	public class MobileEntityGameplayData<TEntity, TCell, TCoordinate> : EntityGameplayData<TEntity, TCell, TCoordinate>
		where TEntity : MobileEntity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
	}
}
