using AnticTest.Architecture.Events;
using AnticTest.DataModel.Entities;
using AnticTest.Systems.FSM;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	public class FightState : State
	{
		private float attackCouldown;

		public override void EnterBehaviours(params object[] parameters)
		{
			attackCouldown = 0.0f;
		}

		public override void TickBehaviours(float deltaTime, params object[] parameters)
		{
			List<ICombatant> targetOponents = (List<ICombatant>)parameters[0];
			int damage = (int)parameters[1];
			float timeBetweenAtacks = (float)parameters[2];

			//if targetoponents.count == 0

			if (targetOponents[0].IsDead)
			{
				EventBus.Raise(new EntityDestroyEvent((IEntity)targetOponents[0]));
				FSMTrigger.Invoke((int)EntityFlags.OnEnemyDead);
			}

			attackCouldown += deltaTime;
			if (attackCouldown >= timeBetweenAtacks)
			{
				targetOponents[0].SetDamage(damage);
				attackCouldown = 0.0f;
			}
		}
		
		public override void ExitBehaviours(params object[] parameters)
		{
		}
	}
}
