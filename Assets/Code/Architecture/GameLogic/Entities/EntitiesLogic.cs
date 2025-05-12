using AnticTest.Architecture.Events;
using AnticTest.Architecture.Pathfinding;
using AnticTest.Architecture.States;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class EntitiesLogic<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		public EntitiesLogic() { }

		public void Init()
		{
			EventBus.Subscribe<EntityChangeCoordinateEvent>(UpdateCurrentOponents);
			EventBus.Subscribe<EntityDestroyEvent>(UpdateCurrentOponents);

			foreach (Ant<TCell, TCoordinate> ant in EntityRegistry.Ants)
			{
				ant.InitFSM(Enum.GetValues(typeof(AntStates)).Length, Enum.GetValues(typeof(AntFlags)).Length);
				AddPatrolState(ant);
				AddDefendState(ant);
				AddAntMovementState(ant);
				RegisterAntTransitions(ant);
				ant.StartFSM((int)AntStates.Patrol);
			}

			foreach (EnemyBug<TCell, TCoordinate> enemyBug in EntityRegistry.Enemies)
			{
				enemyBug.InitFSM(Enum.GetValues(typeof(EnemyStates)).Length, Enum.GetValues(typeof(EnemyFlags)).Length);
				AddEnemyMovementState(enemyBug);
				AddFightState(enemyBug);
				enemyBug.Destiny = (TCoordinate)EntityRegistry.Flag.GetCoordinate();
				RegisterEnemyTransitions(enemyBug);
				enemyBug.StartFSM((int)EnemyStates.Movement);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (IEntity entity in EntityRegistry.Entities)
			{
				entity.Update(deltaTime);
			}
		}

		private void AddPatrolState(Ant<TCell, TCoordinate> ant)
		{
			ant.AddState
				(
				stateIndex: (int)AntStates.Patrol,
				state: new PatrolState<TCell, TCoordinate>(),
				onTickParameters: () => new object[]
					{
						ant.HasOponents,
						false
					}
				);
		}

		private void AddDefendState(Ant<TCell, TCoordinate> ant)
		{
			AddCombatState(ant, new DefendState(), (int)AntStates.Defend);
		}

		private void AddAntMovementState(Ant<TCell, TCoordinate> ant)
		{
			AddMovementState(ant,
							 new AntMovementState<TCell, TCoordinate>(() => Map.Grid,
							 new EntityPathfinding<TCell, TCoordinate>(ant),
							 () => Map.DistanceBetweenCells),
							 (int)AntStates.Movement);
		}

		private void AddEnemyMovementState(EnemyBug<TCell, TCoordinate> enemyBug)
		{
			AddMovementState(enemyBug,
							 new EnemyMovementState<TCell, TCoordinate>(() => Map.Grid,
							 new EntityPathfinding<TCell, TCoordinate>(enemyBug),
							 () => Map.DistanceBetweenCells), 
							 (int)EnemyStates.Movement);
		}

		private void AddFightState(EnemyBug<TCell, TCoordinate> enemyBug)
		{
			AddCombatState(enemyBug, new FightState(), (int)EnemyStates.Fight);
		}

		private void AddMovementState<TMovementState>(MobileEntity<TCell, TCoordinate> mobileEntity,
													  TMovementState movementState, int stateIndex)
													  where TMovementState : MovementBaseState<TCell, TCoordinate>
		{
			Action<TCoordinate> SetCoordinate = (coordinate) => { mobileEntity.SetCoordinate(coordinate); };
			Func<TCell> GetDestiny = () => Map.Grid[mobileEntity.Destiny];
			Func<TCoordinate> GetCoordinate = () => (TCoordinate)mobileEntity.GetCoordinate();
			Func<bool> HasOponents = () => mobileEntity.HasOponents;

			mobileEntity.AddState
				(
				stateIndex: stateIndex,
				state: movementState,
				onEnterParameters: () => new object[]
					{
							Map.Grid[(TCoordinate)mobileEntity.GetCoordinate()],
							GetDestiny
					},
				onTickParameters: () => new object[]
					{
							mobileEntity.GetID(),
							mobileEntity.Speed,
							SetCoordinate,
							GetCoordinate,
							HasOponents
					}
				);
		}

		private void AddCombatState<TCombatState>(MobileEntity<TCell, TCoordinate> mobileEntity,
												  TCombatState combatState, int stateIndex)
												  where TCombatState : CombatBaseState
		{
			Func<List<ICombatant>> GetCurrentOponents = () => mobileEntity.CurrentOponents;

			mobileEntity.AddState
				(
				stateIndex: stateIndex,
				state: combatState,
				onTickParameters: () => new object[]
					{
						GetCurrentOponents,
						mobileEntity.Damage,
						mobileEntity.TimeBetweenAtacks
					}
				);
		}

		private void RegisterEnemyTransitions(EnemyBug<TCell, TCoordinate> enemyBug)
		{
			enemyBug.SetFSMTransition((int)EnemyStates.Movement,
									  (int)EnemyFlags.OnAntReach,
									  (int)EnemyStates.Fight, () =>
									  {

									  });

			enemyBug.SetFSMTransition((int)EnemyStates.Fight,
									  (int)EnemyFlags.OnAntDead,
									  (int)EnemyStates.Movement, () =>
									  {

									  });

			enemyBug.SetFSMTransition((int)EnemyStates.Fight,
									  (int)EnemyFlags.OnAntDesapears,
									  (int)EnemyStates.Movement, () =>
									  {

									  });
		}

		private void RegisterAntTransitions(Ant<TCell, TCoordinate> ant) 
		{
			ant.SetFSMTransition((int)AntStates.Movement,
								 (int)AntFlags.OnEnemyApproach,
								 (int)AntStates.Defend, () =>
								 {
								 
								 });

			ant.SetFSMTransition((int)AntStates.Movement,
								 (int)AntFlags.OnDestinationReach,
								 (int)AntStates.Patrol, () =>
								 {

								 });

			ant.SetFSMTransition((int)AntStates.Patrol,
								 (int)AntFlags.OnEnemyApproach,
								 (int)AntStates.Defend, () =>
								 {

								 });

			ant.SetFSMTransition((int)AntStates.Patrol,
								 (int)AntFlags.OnMovementOrder,
								 (int)AntStates.Movement, () =>
								 {
									 // set destination
								 });

			ant.SetFSMTransition((int)AntStates.Defend,
								 (int)AntFlags.OnEnemyDead,
								 (int)AntStates.Patrol, () =>
								 {

								 });

			ant.SetFSMTransition((int)AntStates.Defend,
								 (int)AntFlags.OnEnemyDesapears,
								 (int)AntStates.Patrol, () =>
								 {

								 });

			ant.SetFSMTransition((int)AntStates.Defend,
								 (int)AntFlags.OnMovementOrder,
								 (int)AntStates.Movement, () =>
								 {
									 // set destination
								 });
		}

		private void UpdateCurrentOponents(EntityChangeCoordinateEvent entityChangeCoordinateEvent)
		{
			UpdateOponentsIn((TCoordinate)entityChangeCoordinateEvent.oldCoordinate);
			UpdateOponentsIn((TCoordinate)entityChangeCoordinateEvent.newCoordinate);
		}

		private void UpdateCurrentOponents(EntityDestroyEvent entityDestroyEvent)
		{
			UpdateOponentsIn((TCoordinate)entityDestroyEvent.entity.GetCoordinate());
		}

		private void UpdateOponentsIn(TCoordinate coordinate)
		{
			foreach (KeyValuePair<Type, List<uint>> allEntiiesInCoordinate in Map.GetAllEntitiesIn(coordinate))
			{
				foreach (uint entityID in allEntiiesInCoordinate.Value)
				{
					SetCurrentOponentsOf(EntityRegistry[entityID]);
				}
			}
		}

		private void SetCurrentOponentsOf(IEntity entity)
		{
			if (!(entity is ICombatant))
				return;

			ICombatant combatant = (ICombatant)entity;
			combatant.ClearCurrentOponents();

			foreach (KeyValuePair<Type, List<uint>> allEntiiesInCoordinate in Map.GetAllEntitiesIn((TCoordinate)entity.GetCoordinate()))
			{
				foreach (uint otherEntityID in allEntiiesInCoordinate.Value)
				{
					if (!(EntityRegistry[otherEntityID] is ICombatant))
						continue;

					ICombatant otherCombatant = (ICombatant)EntityRegistry[otherEntityID];
					if (AreOponents(combatant, otherCombatant))
						combatant.AddOponent(otherCombatant);
				}
			}
		}

		private bool AreOponents(ICombatant combatantA, ICombatant combatantB)
		{
			return (combatantA is Ant<TCell, TCoordinate> && combatantB is EnemyBug<TCell, TCoordinate>) ||
				   (combatantB is Ant<TCell, TCoordinate> && combatantA is EnemyBug<TCell, TCoordinate>);
		}
	}
}
