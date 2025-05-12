namespace AnticTest.Architecture.States
{
	public enum EnemyStates
	{
		Movement = 0,
		Fight = 1,
	}

	public enum EnemyFlags
	{
		OnAntReach = 0,
		OnAntDead = 1,
		OnAntDesapears = 2
	}

	public enum AntStates
	{
		Movement = 0,
		Defend = 1,
		Patrol = 2
	}

	public enum AntFlags
	{
		OnEnemyApproach = 0,
		OnEnemyDead = 1,
		OnEnemyDesapears = 2,
		OnMovementOrder = 3,
		OnDestinationReach = 4
	}
}
