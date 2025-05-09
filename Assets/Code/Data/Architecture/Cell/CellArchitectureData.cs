using AnticTest.DataModel.Grid;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
    [CreateAssetMenu(fileName = "New Cell Architecture Data", menuName = "AnticTest/Data/Architecture/Cell")]
    public sealed class CellArchitectureData : ArchitectureData<Cell<Coordinate>>
    {
        public CellType cellType;
        public CellHeight cellHeight;

        public Cell<Coordinate> Get(Coordinate position)
        {
            return Get((object)position);
        }

        protected override Cell<Coordinate> Get(params object[] parameters)
        {
            Cell<Coordinate> newCell = new Cell<Coordinate>();
            newCell.Init((Coordinate)parameters[0], cellType, cellHeight);
            return newCell;
        }
    }
}
