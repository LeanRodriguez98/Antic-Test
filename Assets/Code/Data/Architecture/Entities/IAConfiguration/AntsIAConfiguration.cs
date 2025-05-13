using UnityEngine;

namespace AnticTest.Data.Architecture
{
	[CreateAssetMenu(fileName = "New Ant IA Configuration", menuName = "AnticTest/Data/Architecture/AntsIAConfiguration")]
	public class AntsIAConfiguration : ScriptableObject
	{
		[SerializeField] private bool useIAByDefault;
		[SerializeField] private uint rangeToDefend;

		[SerializeField] private float enemyDistaneWeightMultiplier;
		[SerializeField] private float enemyHealthWeightMultiplier;
		[SerializeField] private float enemyDamageWeightMultiplier;
		[SerializeField] private float enemySpeedWeightMultiplier;

		public bool UseIAByDefault => useIAByDefault;
		public uint RangeToDefend => rangeToDefend;

		public float EnemyDistanceWeightMultiplier => enemyDistaneWeightMultiplier;
		public float EnemyHealthWeightMultiplier => enemyHealthWeightMultiplier;
		public float EnemyDamageWeightMultiplier => enemyDamageWeightMultiplier;
		public float EnemySpeedWeightMultiplier => enemySpeedWeightMultiplier;
	}
}
