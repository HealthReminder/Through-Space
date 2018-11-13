using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {

	public static SoundtrackManager instance;

	public int currentSet;
	public int currentSource = 1;
	public AudioSource source1,source2;
	[SerializeField]
	public List<Set> sets;
	public bool playing;

	[System.Serializable]
	public class Set {
		public AudioClip[] tracks;
	}

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

	}

	void Start () {
		source1.volume = 0;
		source2.volume = 0;

	}
	

	void Update() {
		if (currentSource == 1) {
			if (!source1.isPlaying) {
				PlaySet (currentSet);	
			}
		} else {
			if (!source2.isPlaying) {
				PlaySet (currentSet);	
			}
		}
	}

	public void PlaySet(int set){
		currentSet = set;
		if (currentSet< sets.Count && currentSet >= 0) {
			if (sets [set].tracks.Length > 0) {
				ChooseNewTrack ();
				ChangeSource (currentSource);
			}
		} else {
		}
	}

	void ChooseNewTrack() {
		int newTrack = Random.Range (0, sets [currentSet].tracks.Length);
		if (currentSource == 1) {
			source2.clip = sets [currentSet].tracks [newTrack];
			source2.Play();
		} else {
				source1.clip = sets [currentSet].tracks [newTrack];
			source1.Play();
		}
	}

	void ChangeSource(int source){
		if (source == 1) {
			StartCoroutine(SetVolume (source1, 0));
			StartCoroutine(SetVolume (source2, 1));
			currentSource = 2;
		} else {
			StartCoroutine(SetVolume (source2, 0));
			StartCoroutine(SetVolume (source1, 1));
			currentSource = 1;
		}


	}

	IEnumerator SetVolume(AudioSource source, float newVolume){
		while (source.volume < newVolume) {
			source.volume += 0.005f;
			yield return null;
		}
		while (source.volume > newVolume) {
			source.volume -= 0.005f;
			yield return null;
		}
		yield break;
	}

}
