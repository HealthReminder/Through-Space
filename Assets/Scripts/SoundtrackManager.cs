using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {
	
	public float soundtrackVolume = 1;
	[Header("Configuration")]
	//Quantity of sources that this script will be using to manage the sounds
	[SerializeField]	int sourceQuantity = 10;
	//Soundtrack volume
	[SerializeField][Range(0.001f,0.1f)]	float volumeRate = 0.01f;

	//Audio source management
	AudioSource[] audioSources;
	int currentSource = 0;
	bool changingSourceAlready = false;

	//Data
	Set currentSet;

	//Sets of soundtrack available to be played in-game
	[Header("Sets")]	[SerializeField]	public List<Set> sets;



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
	

	//Singleton pattern
	[HideInInspector]	public static SoundtrackManager instance;
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

	void Update()
	{
		//This function is responsable for not letting the game be silent ever
		//By playing another song after the one that had finished
		if(!audioSources[currentSource].isPlaying && currentSet != null)
			ChangeSet(currentSet.name);
	}

	//These functions are responsable for stopping the current audio source
	//NOT WORKING
	public void Stop(float rate) {
		StartCoroutine(FadeCurrentSourceOut(rate));
	}
	IEnumerator FadeCurrentSourceOut(float rate) {
		float currentSourceAtStartOfCoroutine = currentSource;
		AudioSource goingDown = audioSources[currentSource];
		while(currentSourceAtStartOfCoroutine == currentSource){
			while(goingDown.volume > 0 ){
				goingDown.volume-=rate;
				yield return null;
			}
			currentSet = null;
			goingDown.Stop();
			goingDown.volume = 0;
			goingDown.clip = null;
			Debug.Log ("Going down volume is "+ goingDown.volume);
			yield break;
		}

		yield break;
	}


	void Setup() {
		//Add the audioSources
		audioSources = new AudioSource[sourceQuantity];
		AudioSource aS;
		for(int a = 0; a < sourceQuantity; a++){
			aS = audioSources[a] = gameObject.AddComponent<AudioSource>();
			aS.playOnAwake = false;
			//aS.loop = true;
		}
	}

	//This function is responsable for changing the music set is player
	//To deliver the right mood
	public void ChangeSet(string name){

		//If the set exists
		bool exists = false;
		foreach(Set s in sets)
			if(s.name == name){
				exists = true;
				currentSet = s;
			}
				
		if(sets.Count> 0 && exists){

			//choose new source
			int newSource = currentSource+1;
			//make sure it is not out of bounds
			if(newSource >= sourceQuantity)
				newSource = 0;

			AudioSource ns = audioSources[newSource];
			AudioSource cs = audioSources[currentSource];

			//choose a random song from the set
			//set and play in an audiosource
			Track newT =  currentSet.tracks[Random.Range(0,currentSet.tracks.Length)];
			ns.clip = newT.clip;
			ns.time = newT.startFrom;
			ns.Play();
			StartCoroutine(ChangeSources(cs,ns,volumeRate));
			//stop last audio
			////audioSources[currentSource].Stop();
			/////audioSources[currentSource].clip = null;
			//change source
			currentSource = newSource;
			

		}
	}
	//This function is responsable for changing source volumes so
	//that the soundtracks fade out and in properly with being thread unsafe .lol
	IEnumerator ChangeSources(AudioSource goingDown, AudioSource goingUp,float rate)
	{
		//This should never go wrong... He said
		goingDown.volume = soundtrackVolume;
		goingUp.volume = 0;
		while(changingSourceAlready)
			yield return null;
		changingSourceAlready = true;
		//If it doesnt then the music shall alternate!
		while(goingDown.volume > 0 || goingUp.volume<soundtrackVolume){
			changingSourceAlready =	 true;
			goingDown.volume-=rate;
			goingUp.volume+=rate;
			yield return null;
		}
		goingUp.volume = soundtrackVolume;
		goingDown.Stop();
		goingDown.clip = null;
		changingSourceAlready = false;
		yield break;
	}

}
