using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[HideInInspector]
	public static AudioManager instance;
	public AnimationCurve rollOfCurve;
	public int sourceQuantity;
	public int currentSource = 0;
	AudioSource[] sources;
	[SerializeField]	public List<Track> tracks;

	[System.Serializable]	public class Track {
		[Header("Data")]
		public string name;
		public AudioClip track;
		[Header("Aspect")]
		[Range(0,3)]
		public float volume = 1;
		[Range(0.01f,3)]
		public float pitch = 1;
		
		[Header("Clipping")]
		public Vector2 startAt;
		public float endAt = 0;
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
			StartCoroutine(PlayTrack(currentTrack.track,currentTrack.pitch,currentTrack.volume,currentTrack.startAt,currentTrack.endAt));
		}

		//}
		
	}

	IEnumerator PlayTrack(AudioClip s, float pitch, float volume, Vector2 startAt, float endAt) {
		sources[currentSource].pitch = pitch;
		sources[currentSource].volume = volume;
		sources[currentSource].clip = s;
		sources[currentSource].time = Random.Range(startAt.x,startAt.y);
		sources[currentSource].Play();

		AudioSource oldSource = null;
		if(endAt != 0){
			oldSource = sources[currentSource];
		}

		currentSource++;
		if(currentSource >= sources.Length)
			currentSource = 0;
		
		if(endAt != 0){
			yield return new WaitForSeconds(endAt);
			float drog = 0;
			while(drog <= 1) {
				drog+=Time.deltaTime*0.5f;
				oldSource.volume = rollOfCurve.Evaluate(drog);
				//print(rollOfCurve.Evaluate(drog) + " "+ drog);
				yield return null;
			}
			oldSource.volume=0;
			//oldSource.Stop();
		}
		


		
		
		yield break;
	}

}
