using AnticTest.Architecture.GameLogic.Strategies;
using AnticTest.Systems.Events;

namespace AnticTest.Architecture.Events
{
	public struct SwapAntsStrategyEvent : IEvent
	{
		public AntStrategies strategyToSwap;

		public SwapAntsStrategyEvent(AntStrategies strategyToSwap)
		{
			this.strategyToSwap = strategyToSwap;
		}
	}
}
