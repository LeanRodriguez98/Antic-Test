using AnticTest.Architecture.Events;
using AnticTest.DataModel.Entities;
using AnticTest.Systems.FSM;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	public abstract class CombatBaseState : State
	{
		private float attackCouldown;

		public override void EnterBehaviours(params object[] parameters)
		{
			attackCouldown = 0.0f;
		}

		public override void TickBehaviours(float deltaTime, params object[] parameters)
		{
			Func<List<ICombatant>> targetOponents = (Func<List<ICombatant>>)parameters[0];
			int damage = (int)parameters[1];
			float timeBetweenAtacks = (float)parameters[2];

			int targetDesapearsFlag = (int)parameters[3];
			int targetDeadFlag = (int)parameters[4];

			if (targetOponents.Invoke().Count == 0)
			{
				FSMTrigger.Invoke(targetDesapearsFlag);
				return;
			}

			if (targetOponents.Invoke()[0].IsDead)
			{
				EventBus.Raise(new EntityDestroyEvent((IEntity)targetOponents.Invoke()[0]));
				FSMTrigger.Invoke(targetDeadFlag);
				return;
			}

			attackCouldown += deltaTime;
			if (attackCouldown >= timeBetweenAtacks)
			{
				targetOponents.Invoke()[0].SetDamage(damage);
				attackCouldown = 0.0f;
			}
		}

		public override void ExitBehaviours(params object[] parameters)
		{
		}
	}
}
