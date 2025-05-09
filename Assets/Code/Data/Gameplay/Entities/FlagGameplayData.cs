using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	[CreateAssetMenu(fileName = "New Flag Gameplay Data", menuName = "AnticTest/Data/Gameplay/Entities/Flag")]
	public class FlagGameplayData : EntityGameplayData<Flag<Cell<Coordinate>, Coordinate>, Cell<Coordinate>, Coordinate>
	{
	}
}
