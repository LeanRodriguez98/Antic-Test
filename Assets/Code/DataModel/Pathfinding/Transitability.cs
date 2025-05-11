using AnticTest.DataModel.Grid;
using System;
using System.Collections.Generic;

namespace AnticTest.DataModel.Pathfinding
{
	[Serializable]
	public struct Transitability
	{
		[Serializable]
		public struct CellValue
		{
			public CellType cellType;
			public int value;
		}

		public List<CellValue> moveToCellTypeCost;
		public List<CellType> intransitableCellTypes;
		public int increaseHeightCost;
		public int decreaseHeightCost;

		public Transitability(List<CellValue> moveToCellTypeCost,
							  List<CellType> intransitableCellTypes,
							  int increaseHeightCost,
							  int decreaseHeightCost)
		{
			this.moveToCellTypeCost = moveToCellTypeCost;
			this.intransitableCellTypes = intransitableCellTypes;
			this.increaseHeightCost = increaseHeightCost;
			this.decreaseHeightCost = decreaseHeightCost;
		}

		public Dictionary<CellType, int> GetMoveToCellTypeCostAsDictionary()
		{
			Dictionary<CellType, int> output = new Dictionary<CellType, int>();
			foreach (CellValue cellValue in moveToCellTypeCost)
			{
				if (!output.ContainsKey(cellValue.cellType))
					output.Add(cellValue.cellType, cellValue.value);
			}
			return output;
		}
	}
}
