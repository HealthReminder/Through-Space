using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundController : MonoBehaviour {
	[Header("Configuration")]
	[SerializeField]int sourceQuantity = 10;
	[SerializeField][Range(0.001f,0.1f)]	float volumeRate = 0.004f;
	AudioSource[] audioSources;
	int currentSource = 0;
	bool changingSourceAlready = false;
	Set currentSet;


	[Header("Sets")]
	[SerializeField]
	public List<Set> sets;

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
	void Update()
	{

		if(!audioSources[currentSource].isPlaying && currentSet != null)
			ChangeSet(currentSet.name);
	}


	void Start() {
		//Add the audioSources
		audioSources = new AudioSource[sourceQuantity];
		AudioSource aS;
		for(int a = 0; a < sourceQuantity; a++){
			aS = audioSources[a] = gameObject.AddComponent<AudioSource>();
			aS.playOnAwake = false;
			//aS.loop = true;
		}
	}


	public void ChangeSet(string name){

		
				
		if(sets.Count> 0){

			//If the set exists
			bool exists = false;
			foreach(Set s in sets)
			if(s.name == name){
				exists = true;
				currentSet = s;
			}

			//choose new source
			int newSource = currentSource+1;
			//make sure it is not out of bounds
			if(newSource >= sourceQuantity)
				newSource = 0;

			AudioSource ns = audioSources[newSource];
			AudioSource cs = audioSources[currentSource];

			if(exists){
			//choose a random song from the set
			//set and play in an audiosource
			Track newT =  currentSet.tracks[Random.Range(0,currentSet.tracks.Length)];
			ns.clip = newT.clip;
			ns.time = newT.startFrom;
			ns.Play();
			}
			StartCoroutine(ChangeSources(cs,ns,volumeRate));
			//stop last audio
			////audioSources[currentSource].Stop();
			/////audioSources[currentSource].clip = null;
			//change source
			currentSource = newSource;
			

		}
	}

	IEnumerator ChangeSources(AudioSource goingDown, AudioSource goingUp,float rate)
	{
		//This should never go wrong... He said
		goingDown.volume = 0.5f;
		goingUp.volume = 0;
		while(changingSourceAlready)
			yield return null;
		changingSourceAlready = true;
		//If it doesnt then the music shall alternate!
		while(goingDown.volume > 0 || goingUp.volume<0.5f){
			changingSourceAlready =	 true;
			goingDown.volume-=rate;
			goingUp.volume+=rate;
			yield return null;
		}
		goingDown.Stop();
		goingDown.clip = null;
		changingSourceAlready = false;
		yield break;
	}

}
