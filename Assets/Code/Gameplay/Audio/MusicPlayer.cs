using AnticTest.Gameplay.Components;
using AnticTest.Gameplay.Events;
using AnticTest.Systems.Events;
using System;
using UnityEngine;

namespace AnticTest.Gameplay.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicPlayer : GameComponent
	{
		[SerializeField] [Range(0.0f, 1.0f)] private float volume = 1.0f;
		private AudioSource audioSource;

		public override void Init()
		{
			EventBus.Subscribe<ToggleMusicEvent>(ToggleMusic);

			audioSource = GetComponent<AudioSource>();

			if (audioSource.clip == null)
				return;

			audioSource.volume = volume;
			audioSource.loop = true;
			audioSource.Play();
		}

		public override void Dispose()
		{
			EventBus.Unsubscribe<ToggleMusicEvent>(ToggleMusic);
		}

		private void ToggleMusic(ToggleMusicEvent _)
		{
			if (audioSource == null)
				return;

			if (audioSource.isPlaying)
				audioSource.Pause();
			else
				audioSource.Play();
		}
	}
}