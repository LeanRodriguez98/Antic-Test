using System.Collections.Generic;

namespace AnticTest.DataModel.Entities
{
	public interface ICombatant
	{
		public int Health { get; }
		public bool IsDead { get; }
		public int Damage { get; }
		public List<ICombatant> CurrentOponents { get; }
		public bool HasOponents { get; }
		public float TimeBetweenAtacks { get; }
		public void SetDamage(int damage);
		public void ClearCurrentOponents();
		public void AddOponent(ICombatant oponent);
	}
}
