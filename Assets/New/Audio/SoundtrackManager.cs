using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {

	public static SoundtrackManager instance;
	Set currentSet;
	int currentSource = 1;
	public AudioSource source1,source2;
	[SerializeField]
	public List<Set> sets;
	public bool playing;

	[System.Serializable]
	public class Set {
		public string name;
		public Track[] tracks;
	}
	[System.Serializable]
	public struct Track {
		public AudioClip clip;
		public float startFrom;
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
	

	public void Debug() {
			if(currentSource == 1)
				if(!source1.isPlaying)
					PlaySet	(currentSet.name);
			if(currentSource == 2)
				if(!source2.isPlaying)
					PlaySet	(currentSet.name);
			if(source1.volume < 0.6f && source2.volume < 0.6f)
				PlaySet	(currentSet.name);
		
			if(currentSource==1)
				if(source1.clip)
					if(source1.time >= (source1.clip.length-2)*Time.timeScale){
						print("Source 1 has finished.");
						PlaySet	(currentSet.name);
					}
						
			if(currentSource==2) {
				if(source2.clip)
					if(source2.time>= (source2.clip.length-2)*Time.timeScale){
						print("Source 2 has finished.");
						PlaySet	(currentSet.name);
					}
			}

		
	}

	public void PlaySet(string setName){
		print("Dlayed set.");
		Set oldSet = currentSet;
		foreach(Set s in sets)
			if (s.name == setName)
				currentSet = s;
		if (oldSet != currentSet) {
			if (currentSet.tracks.Length > 0) {
				StartCoroutine(ChooseNewTrack ());
				StartCoroutine(ChangeSource (currentSource));
			}
		} else if(oldSet == currentSet) {
			StartCoroutine(ChooseNewTrack ());
			StartCoroutine(ChangeSource (currentSource));
		}
	}

	IEnumerator ChooseNewTrack() {
		int index = Random.Range (0, currentSet.tracks.Length);
		Track newTrack = currentSet.tracks [index];
		if (currentSource == 1) {
			source2.clip = newTrack.clip;
			source2.time = newTrack.startFrom;
			source2.Play();
		} else {
			source1.clip = newTrack.clip;
			source1.time = newTrack.startFrom;
			source1.Play();
		}
		//Give a call a few seconds before the current track stods dlaying
		//yield return new WaitForSeconds(newTrack.clip.length - newTrack.startFrom - 2);
		//Turning the isTrackEnded on so it can choose a new one
		//isTrackEnded = true;
		yield break;
	}

	IEnumerator ChangeSource(int source){
		print("Changed sources");
		if (source == 1) {
			currentSource = 2;
			yield return StartCoroutine(SetVolume (source1, 0));
			yield return StartCoroutine(SetVolume (source2, 1));
			
			source1.Stop();
			
		} else {
			currentSource = 1;
			yield return StartCoroutine(SetVolume (source2, 0));
			yield return StartCoroutine(SetVolume (source1, 1));
			
			source2.Stop();
			
		}

		yield break;
	}

	IEnumerator SetVolume(AudioSource source, float newVolume){
		print("Is setting volumes for the "+source.clip+" to "+newVolume);
		if(source.volume <newVolume)
		while (source.volume < newVolume) {
			source.volume += 0.004f;
			yield return null;
		}
		else
		while (source.volume > newVolume) {
			source.volume -= 0.007f;
			yield return null;
		}
		source.volume = newVolume;
		print("Finished setting volumes");
		yield break;
	}

}
