using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	public class EntityGameplayData<TEntity, TCell, TCoordinate> : GameplayData<TEntity>
		where TEntity : Entity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public GameObject prefab;
	}
}
