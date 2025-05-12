using AnticTest.DataModel.Grid;
using AnticTest.Systems.FSM;

namespace AnticTest.Architecture.States
{
	public class PatrolState<TCell, TCoordinate> : State
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		public PatrolState() { }

		public override void EnterBehaviours(params object[] parameters)
		{
		}

		public override void TickBehaviours(float deltaTime, params object[] parameters)
		{
			bool hasOponents = (bool)parameters[0];
			bool hasMovementOrder = (bool)parameters[1];

			if (hasOponents)
				FSMTrigger.Invoke((int)AntFlags.OnEnemyApproach);

			if (hasMovementOrder)
				FSMTrigger.Invoke((int)AntFlags.OnMovementOrder);
		}

		public override void ExitBehaviours(params object[] parameters)
		{
		}
	}
}
