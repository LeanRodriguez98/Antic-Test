using AnticTest.Gameplay.Events;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using UnityEngine;

namespace AnticTest.Gameplay.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicPlayer : MonoBehaviour
	{
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		[SerializeField] [Range(0.0f, 1.0f)] private float volume = 1.0f;
		private AudioSource audioSource;

		private void Start()
		{
			EventBus.Subscribe<ToggleMusicEvent>(ToggleMusic);

			audioSource = GetComponent<AudioSource>();

			if (audioSource.clip == null)
				return;

			audioSource.volume = volume;
			audioSource.loop = true;
			audioSource.Play();
		}

		private void OnDisable()
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