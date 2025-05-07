using AnticTest.DataModel.Map;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
    [CreateAssetMenu(fileName = "New Cell Architecture Data", menuName = "AnticTest/Data/Architecture/Cell")]
    public sealed class CellArchitectureData : ArchitectureData<Cell>
    {
        public CellType cellType;
        public CellHeight cellHeight;

        public Cell Get(Coordinate position)
        {
            return Get((object)position);
        }

        protected override Cell Get(params object[] parameters)
        {
            Cell newCell = new Cell();
            newCell.Init((Coordinate)parameters[0], cellType, cellHeight);
            return newCell;
        }
    }
}
