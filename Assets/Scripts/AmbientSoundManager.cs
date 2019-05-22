using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]	public struct Track {
		public string name;
		public AudioClip clip;
		[Range(0,1)] public float randomPortion;
}

public class AmbientSoundManager : MonoBehaviour {
	[Header("Configuration")]
	public float ambientSoundVolume = 1f;
	int currentAudioSource = 0;

	AudioSource audioSource1;
	AudioSource audioSource2;
	Track currentTrack;


	[Header("Sets")]
	[SerializeField]
	public List<Track> tracks;

	


	//Singleton pattern
	[HideInInspector]	public static AmbientSoundManager instance;
	void Awake()	{	
		//Make it the only one
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		Setup();
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.T))
			StartAmbientSound("Earth");
		if(Input.GetKeyDown(KeyCode.Y))
			StopAmbientSound();
		
	}

	public void StartAmbientSound(string name) {
		Debug.Log("Now playing ambient sound: "+name);
		bool wasFound = false;
		Track foundTrack = new Track();
		foreach(Track t in tracks)
			if(t.name == name){
				foundTrack = t;
				wasFound = true;
			}

		if(!wasFound)
			Debug.Log("Didn't find track");
		else {
			if(currentAudioSource == 0){
				currentAudioSource = 1;
				StartCoroutine(PlaySource(foundTrack, audioSource1));
				StartCoroutine(StopSource(audioSource2));
			} else {
				currentAudioSource = 0;
				StartCoroutine(PlaySource(foundTrack, audioSource2));
				StartCoroutine(StopSource(audioSource1));	
			}
		}

		return;
	}

	public void StopAmbientSound(){
		Debug.Log("Stopped ambient sound");
		if(currentAudioSource == 0){
			StartCoroutine(StopSource(audioSource2));
		} else {
			StartCoroutine(StopSource(audioSource1));
		}
	}
	bool isStartingSource = false;
	IEnumerator PlaySource(Track newTrack, AudioSource source) {
		isStartingSource = false;
		yield return null;
		isStartingSource = true;

		source.volume = 0;
		source.clip = newTrack.clip;
		source.time = Random.Range(0,Mathf.Lerp(0, newTrack.clip.length, newTrack.randomPortion));
		source.Play();

		while(isStartingSource) {
			if(source.volume < 1*ambientSoundVolume)
				source.volume+= Time.deltaTime/10/ambientSoundVolume;
			else
				isStartingSource = false;
			yield return null;
		}

		yield break;
	}
	bool isStoppingSource = false;
	IEnumerator StopSource(AudioSource source) {
		isStoppingSource = false;
		yield return null;
		isStoppingSource = true;

		while(isStoppingSource) {
			if(source.volume > 0)
				source.volume-= Time.deltaTime/10/ambientSoundVolume;
			else
				isStoppingSource = false;
			yield return null;
		}

		//source.Stop();

		yield break;
	}
	void Setup() {
		audioSource1 = gameObject.AddComponent<AudioSource>();
		audioSource1.playOnAwake = false;
		audioSource1.loop = true;
		audioSource1.spatialBlend = 0;
		audioSource2 = gameObject.AddComponent<AudioSource>();
		audioSource2.playOnAwake = false;
		audioSource2.loop = true;
		audioSource2.spatialBlend = 0;
	}

}
