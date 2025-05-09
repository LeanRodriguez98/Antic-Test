using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	[CreateAssetMenu(fileName = "New Enemy Bug Gameplay Data", menuName = "AnticTest/Data/Gameplay/Entities/Enemy Bug")]
	public class EnemyBugGameplayData : MobileEntityGameplayData<EnemyBug<Cell<Coordinate>, Coordinate>, Cell<Coordinate>, Coordinate>
	{
	}
}
