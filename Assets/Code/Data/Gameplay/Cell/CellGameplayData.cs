using AnticTest.DataModel.Map;
using UnityEngine;

namespace AnticTest.Data.Gameplay
{
	[CreateAssetMenu(fileName = "New Cell Gameplay Data", menuName = "AnticTest/Data/Gameplay/Cell")]
	public sealed class CellGameplayData : GameplayData<Cell>
	{
		public GameObject[] prefafs = new GameObject[14];

		public static readonly bool[][] Passability = new bool[][]
		{
			new bool[] {true,  true,   true,   true,   true,   true },
			new bool[] {false, false,  false,  false,  false,  false},
			new bool[] {true,  false,  false,  false,  false,  false},
			new bool[] {true,  true,   false,  false,  false,  false},
			new bool[] {true,  true,   true,   false,  false,  false},
			new bool[] {true,  true,   true,   true,   false,  false},
			new bool[] {true,  true,   true,   true,   true,   false},
			new bool[] {true,  false,  false,  true,   false,  false},
			new bool[] {true,  true,   false,  true,   true,   false},
			new bool[] {true,  true,   false,  true,   false,  false},
			new bool[] {true,  true,   false,  false,  true,   false},
			new bool[] {true,  false,  true,   false,  true,   false},
			new bool[] {true,  false,  true,   true,   true,   false},
			new bool[] {true,  false,  true,   false,  false,  false},
		};
	}
}