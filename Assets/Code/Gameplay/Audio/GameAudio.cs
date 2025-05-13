using AnticTest.Gameplay.Components;
using UnityEngine;

namespace AnticTest.Gameplay.Audio
{
	[RequireComponent(typeof(MusicPlayer))]
	public class GameAudio : GameComponent
	{
		private MusicPlayer musicPlayer;

		public override void Init()
		{
			musicPlayer = GetComponent<MusicPlayer>();
			musicPlayer.Init();
		}

		public override void Dispose()
		{
			musicPlayer.Dispose();
		}
	}
}
