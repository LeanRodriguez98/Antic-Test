﻿using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using UnityEngine;

namespace AnticTest.Data.Architecture
{
	public abstract class MobileEntityArchitectureData<TEntity, TCell, TCoordinate> : EntityArchitectureData<TEntity, TCell, TCoordinate>
		where TEntity : MobileEntity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		[SerializeField] private int speed;
		[SerializeField] private int health;
		[SerializeField] private int damage;
		[SerializeField] private float timeBetweenAtacks;
		[SerializeField] private Transitability transitability;

		public override object[] GetParameters()
		{
			return new object[] { speed, health, damage, timeBetweenAtacks, transitability };
		}
	}
}
