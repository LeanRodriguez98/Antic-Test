using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnticTest.DataModel.Entities
{
	public interface IEntity
	{
		public uint GetID();
		public (int x, int y) GetCoordinate();
	}
}
