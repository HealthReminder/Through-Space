using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {
	[HideInInspector]
	public static SoundtrackManager instance;
	[Header("Configuration")]
	[SerializeField]	int sourceQuantity = 10;
	[SerializeField][Range(0.001f,0.1f)]	float volumeRate = 0.01f;
	AudioSource[] audioSources;
	int currentSource = 0;
	bool changingSourceAlready = false;
	int currentSetID;


	[Header("Sets")]
	[SerializeField]
	public List<Set> sets;

	[System.Serializable]
	public class Set {
		public Track[] tracks;
	}
	[System.Serializable]
	public struct Track {
		public AudioClip clip;
		public float startFrom;
	}

	
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
		
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.O)){
			currentSetID = Random.Range(0,sets.Count);
			ChangeSet(currentSetID);
		}
		/*//Test if the SM is changing tracks correctly when one is finished.
		if(Input.GetKeyDown(KeyCode.I))
			//audioSources[currentSource].time = audioSources[currentSource].clip.length-2;
		*/		

		if(!audioSources[currentSource].isPlaying)
			ChangeSet(currentSetID);
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


	public void ChangeSet(int id){
		//If the set exists
		if(id >= 0 && id < sets.Count){

			//choose new source
			int newSource = currentSource+1;
			//make sure it is not out of bounds
			if(newSource >= sourceQuantity)
				newSource = 0;

			AudioSource ns = audioSources[newSource];
			AudioSource cs = audioSources[currentSource];

			//choose a random song from the set
			//set and play in an audiosource
			Track newT =  sets[id].tracks[Random.Range(0,sets[id].tracks.Length)];
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

	IEnumerator ChangeSources(AudioSource goingDown, AudioSource goingUp,float rate)
	{
		//This should never go wrong... He said
		goingDown.volume = 1;
		goingUp.volume = 0;
		while(changingSourceAlready)
			yield return null;
		changingSourceAlready = true;
		//If it doesnt then the music shall alternate!
		while(goingDown.volume > 0 || goingUp.volume<1){
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

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour {
	[HideInInspector]
	public static SoundtrackManager instance;
	[Header("Configuration")]
	[SerializeField]
	int sourceQuantity = 10;
	AudioSource[] audioSources;
	int currentSource = 0;


	[Header("Sets")]
	[SerializeField]
	public List<Set> sets;

	[System.Serializable]
	public class Set {
		public Track[] tracks;
	}
	[System.Serializable]
	public struct Track {
		public AudioClip clip;
		public float startFrom;
	}

	
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
		
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.O)){
			ChangeSet(Random.Range(0,sets.Count));
		}
	}


	void Start() {
		//Add the audioSources
		audioSources = new AudioSource[sourceQuantity];
		for(int a = 0; a < sourceQuantity; a++){
			audioSources[a] = gameObject.AddComponent<AudioSource>();
			audioSources[a].playOnAwake = false;
		}
	}


	public void ChangeSet(int id){
		//If the set exists
		if(id >= 0 && id < sets.Count){

			//choose new source
			int newSource = currentSource+1;
			//make sure it is not out of bounds
			if(newSource >= sourceQuantity)
				newSource = 0;

			//choose a random song from the set
			//set and play in an audiosource
			audioSources[newSource].clip =  sets[id].tracks[Random.Range(0,sets[id].tracks.Length)].clip;
			audioSources[newSource].Play();
			StartCoroutine(ChangeSources(audioSources[currentSource],audioSources[newSource],0.01f));
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
		goingDown.volume = 1;
		goingUp.volume = 0;
		//If it doesnt then the music shall alternate!
		while(goingDown.volume > 0 || goingUp.volume<1){
			goingDown.volume-=rate;
			goingUp.volume+=rate;
			yield return null;
		}
		goingDown.Stop();
		goingDown.clip = null;
		yield break;
	}

}
 */
