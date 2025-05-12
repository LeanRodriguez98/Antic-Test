using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	public class DefendState : CombatBaseState
	{
		public override void TickBehaviours(float deltaTime, params object[] parameters)
		{
			base.TickBehaviours(deltaTime, parameters.AddRange(
				new object[] { AntFlags.OnEnemyDesapears, AntFlags.OnEnemyDead }));
		}
	}
}
