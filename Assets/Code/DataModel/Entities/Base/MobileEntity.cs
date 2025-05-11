using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using AnticTest.Systems.FSM;
using System;

namespace AnticTest.DataModel.Entities
{
	public abstract class MobileEntity<TCell, TCoordinate> : Entity<TCell, TCoordinate>, ITransitabilityProvider
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected int speed;
		protected int health;
		protected Transitability transitability;
		protected FSM fsm;
		protected TCoordinate destiny;

		public MobileEntity(TCoordinate coordinate, uint ID) : base(coordinate, ID) { }

		public override void SetCoordinate(TCoordinate coordinate)
		{
			base.SetCoordinate(coordinate);
		}

		protected override void SetParameters(params object[] parameters)
		{
			speed = (int)parameters[0];
			health = (int)parameters[1];
			transitability = (Transitability)parameters[2];
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

		public void StartFSM(int startState)
		{
			fsm.ForceState(startState);
		}

		public int Speed => speed;
		public int Health => health;
		public TCoordinate Destiny { get => destiny; set => destiny = value; }

		public Transitability GetTransitability()
		{
			return transitability;
		}
	}
}
