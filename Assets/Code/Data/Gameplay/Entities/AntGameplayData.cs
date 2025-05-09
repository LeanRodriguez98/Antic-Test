using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	[CreateAssetMenu(fileName = "New Ant Gameplay Data", menuName = "AnticTest/Data/Gameplay/Entities/Ant")]
	public class AntGameplayData : MobileEntityGameplayData<Ant<Cell<Coordinate>, Coordinate>, Cell<Coordinate>, Coordinate>
	{

	}
}
