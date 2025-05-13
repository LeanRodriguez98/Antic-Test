using AnticTest.Architecture.Events;
using AnticTest.Architecture.GameLogic.Strategies;
using AnticTest.Architecture.Pathfinding;
using AnticTest.Architecture.States;
using AnticTest.Data.Blackboard;
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
		private DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();


		private CombatFinder<TCell, TCoordinate> combatFinder;

		private Dictionary<AntStrategies, IAntsStrategy> strategies;
		private AntStrategies antsCurrentStrategy;

		public EntitiesLogic()
		{
			if (DataBlackboard.AntsIAConfiguration.UseIAByDefault)
				antsCurrentStrategy = AntStrategies.IA;
			else
				antsCurrentStrategy = AntStrategies.Manual;
		}

		public void Init()
		{
			EventBus.Subscribe<SwapAntsStrategyEvent>(SwapAntsStrategy);

			combatFinder = new CombatFinder<TCell, TCoordinate>();

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

			strategies = new Dictionary<AntStrategies, IAntsStrategy>();
			strategies.Add(AntStrategies.IA, new AntsIAStrategy<TCell, TCoordinate>());
			strategies.Add(AntStrategies.Manual, new AntsManualStrategy<TCell, TCoordinate>());

			strategies[antsCurrentStrategy].Enable();
		}

		public void Update(float deltaTime)
		{
			strategies[antsCurrentStrategy].Update();

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
						ant.HasDestiny
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
									 ant.RemoveDestiny();
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
		}

		private void SwapAntsStrategy(SwapAntsStrategyEvent swapAntsStrategyEvent)
		{
			strategies[antsCurrentStrategy].Disable();
			antsCurrentStrategy = swapAntsStrategyEvent.strategyToSwap;
			strategies[antsCurrentStrategy].Enable();
		}

		public Dictionary<AntStrategies, IAntsStrategy> Strategies => strategies;
		public AntStrategies AntsCurrentStrategy => antsCurrentStrategy;
	}
}
