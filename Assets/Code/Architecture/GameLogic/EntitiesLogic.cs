using AnticTest.Architecture.Pathfinding;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Provider;

namespace AnticTest.Architecture.GameLogic
{
	public class EntitiesLogic<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();

		public EntitiesLogic()
		{
		}

		public void Init()
		{
			foreach (MobileEntity<TCell, TCoordinate> mobileEntity in EntityRegistry.MobileEntites)
			{
				mobileEntity.SetPathfinderSystem(() => Map.Grid, new EntityPathfinding<TCell, TCoordinate>(mobileEntity));
				mobileEntity.SetLogicalDistanceBetweenCells(() => Map.DistanceBetweenCells);
			}

			foreach (EnemyBug<TCell, TCoordinate> enemyBug in EntityRegistry.Enemies)
			{
				enemyBug.GoTo(Map.Grid[(TCoordinate)EntityRegistry.Flag.GetCoordinate()]);
			}
		}

		public void Update(float deltaTime) 
		{
			foreach (IEntity entity in EntityRegistry.Entities)
			{
				entity.Update(deltaTime);
			}
		}
	}
}
