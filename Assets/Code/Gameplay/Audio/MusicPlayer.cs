using UnityEngine;

namespace AnticTest.Gameplay.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField] [Range(0.0f, 1.0f)] private float volume = 1.0f;
		private AudioSource audioSource;

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
		
			if (audioSource.clip == null)
				return;

			audioSource.volume = volume;
			audioSource.loop = true;
			audioSource.Play();
		}
	}
}