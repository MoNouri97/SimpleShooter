using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
	public SoundGroup[] soundGroups;
	public Dictionary<string, AudioClip[]> library = new Dictionary<string, AudioClip[]>();

	private void Awake()
	{
		foreach (var item in soundGroups)
		{
			library.Add(item.id, item.sounds);
		}
	}
	public AudioClip GetClipFromName(string clip)
	{
		AudioClip[] sounds = library[clip];
		if (sounds == null) return null;
		if (sounds.Length == 1) return sounds[0];

		return sounds[Random.Range(0, sounds.Length - 1)];
	}

	[System.Serializable]
	public struct SoundGroup
	{
		public string id;
		public AudioClip[] sounds;
	}
}
