using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using AnticTest.Systems.FSM;
using System;
using System.Collections.Generic;

namespace AnticTest.DataModel.Entities
{
	public abstract class MobileEntity<TCell, TCoordinate> : Entity<TCell, TCoordinate>,
		ITransitabilityProvider, ICombatant
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected int speed;
		protected int health;
		protected int damage;
		protected float timeBetweenAtacks;
		protected List<ICombatant> currentOponents;
		protected Transitability transitability;
		protected FSM fsm;
		protected TCoordinate destiny;

		public MobileEntity(TCoordinate coordinate, uint ID) : base(coordinate, ID)
		{
			currentOponents = new List<ICombatant>();
		}

		public override void SetCoordinate(TCoordinate coordinate)
		{
			base.SetCoordinate(coordinate);
		}

		protected override void SetParameters(params object[] parameters)
		{
			speed = (int)parameters[0];
			health = (int)parameters[1];
			damage = (int)parameters[2];
			timeBetweenAtacks = (float)parameters[3];
			transitability = (Transitability)parameters[4];
		}

		public override void Update(float deltatime)
		{
			fsm?.Tick(deltatime);
		}

		public void AddState<TState>(int stateIndex, TState state, Func<object[]> onEnterParameters = null,
			Func<object[]> onTickParameters = null, Func<object[]> onExitParameters = null) where TState : State
		{
			fsm.AddBehaviour(stateIndex, state, onEnterParameters, onTickParameters, onExitParameters);
		}

		public void InitFSM(int statesAmount, int flagsAmount)
		{
			fsm = new FSM(statesAmount, flagsAmount);
		}

		public void SetFSMTransition(int originState, int flag, int destinationState, Action onTransition = null)
		{
			fsm.SetTransition(originState, flag, destinationState, onTransition);
		}

		public void StartFSM(int startState)
		{
			fsm.ForceState(startState);
		}

		public int Speed => speed;
		public TCoordinate Destiny { get => destiny; set => destiny = value; }

		public Transitability GetTransitability()
		{
			return transitability;
		}

		public int Health => health;
		public bool IsDead => health <= 0;
		public int Damage => damage;
		public float TimeBetweenAtacks => timeBetweenAtacks;
		public List<ICombatant> CurrentOponents => currentOponents;
		public bool HasOponents => CurrentOponents != null && CurrentOponents.Count > 0;

		public void SetDamage(int damage)
		{
			health -= damage > health ? damage : health;
		}

		public void ClearCurrentOponents()
		{
			currentOponents.Clear();
		}

		public void AddOponent(ICombatant oponent)
		{
			currentOponents.Add(oponent);
		}
	}
}
