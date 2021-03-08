using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
namespace Utils
{
	public static class UnityExtension
	{
		public static void DestroyAllChildren(this Transform transform)
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Object.Destroy(transform.GetChild(i).gameObject);
			}
		}

		public static void TraversalChildren(this Transform transform, UnityAction<Transform> action)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				action.Invoke(transform.GetChild(i));
			}
		}
		
		#region AudioSource extension
		/// <summary>
		/// A sugar for audioSource play by check audioSource and volume state
		/// </summary>
		public static bool Play(this AudioSource audioSource, AudioClip audioClip = null, float volume = 1f)
		{
			if (audioSource == null || audioClip == null || volume < 0f)
			{
				return false;
			}

			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.Play();

			return true;
		}

	


		private static void OnUpdateVolume(AudioSource source, float value)
		{
			source.volume = Mathf.Clamp01(value);
		}
		
		public static bool IsPlayingClip(this AudioSource source, string clipName)
		{
			return source.IsCurrentClip(clipName) && source.isPlaying;
		}

		public static bool IsCurrentClip(this AudioSource source, string clipName)
		{
			return source.clip != null && source.clip.name == clipName;
		}
		#endregion
	}
}