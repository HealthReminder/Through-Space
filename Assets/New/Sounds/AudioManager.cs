
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[HideInInspector]
	public static AudioManager instance;
	public AnimationCurve rollofCurve;
	public int sourceQuantity;
	public int currentSource = 0;
	
	string testingNow = "";

	AudioSource[] sources;
	[SerializeField]
	public List<Track> tracks;
	//public bool playing;

	[System.Serializable]
	public class Track {
		public string name;
		[Range(0,3)]
		public float volume = 1;
		[Range(0.01f,3)]
		public float pitch = 1;
		public AudioClip track;
		
		[Header("Clipping")]
		public Vector2 startFrom;
		public float playFor = 0;
	}

	float lastIndex;

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

		sources = new AudioSource[sourceQuantity];
		for(int a = 0; a< sources.Length; a++){
			sources[a] = gameObject.AddComponent<AudioSource>();
			sources[a].playOnAwake = false;
		}
			

	}

	

	public void Play(string name){
		//DISABLED FOR NOW 
		//if( true == false ){
		if(name != null && name!= "") {
			Track currentTrack = tracks[0];
			foreach(Track s in tracks) {
				if(s.name == name)
					currentTrack = s;
			}
			StartCoroutine(PlayTrack(currentTrack.track,currentTrack.pitch,currentTrack.volume,currentTrack.startFrom,currentTrack.playFor));
		}

		//}
		
	}

	IEnumerator PlayTrack(AudioClip s, float pitch, float volume, Vector2 startFrom, float playFor) {
		sources[currentSource].pitch = pitch;
		sources[currentSource].volume = volume;
		sources[currentSource].clip = s;
		sources[currentSource].time = Random.Range(startFrom.x,startFrom.y);
		sources[currentSource].Play();

		AudioSource oldSource = null;
		if(playFor != 0){
			oldSource = sources[currentSource];
		}

		currentSource++;
		if(currentSource >= sources.Length)
			currentSource = 0;
		
		if(playFor != 0){
			yield return new WaitForSeconds(playFor);
			float drog = 0;
			while(drog <= 1) {
				drog+=Time.deltaTime*0.5f;
				oldSource.volume = rollofCurve.Evaluate(drog);
				//print(rollofCurve.Evaluate(drog) + " "+ drog);
				yield return null;
			}
			oldSource.volume=0;
			//oldSource.Stop();
		}
		


		
		
		yield break;
	}

}
