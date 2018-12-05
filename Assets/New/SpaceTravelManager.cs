using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceTravelManager : MonoBehaviour {

    [Header("player Information")]
    public int lastLevel;
	public int currentLevel;
	public int maxLevel;
	[Header("prefabs")]
	public GameObject pplayer;
	PlayerBehaviour player;
	public LevelInfo[] levels;
	[Header("GUI")]
	public Image overlay;
	[Header("System Information")]
	public Transform currentSolarSystem;
    public bool isFirstLevel = false;

	[System.Serializable]
	public struct LevelInfo {
		public int ID;
		public GameObject prefab;
	}
		
	//Can be used to find the right forward vector (oopsies)
	void Update()
	{
		if(player)
		Debug.DrawRay(player.transform.position,player.transform.right, Color.red, 1);
	}
	//Debug.DrawRay(player.transform.position,player.transform.right, Color.red, 1);
	
	void Start () {
        if (isFirstLevel)
        {
            lastLevel = -1;
            currentLevel = 0;
        }
       
        //If this is the first level, spawn the Solar System prefab or maybe don't do shit
        if (!isFirstLevel) { 
		StartCoroutine(Intro());
		//Get current progress
		//currentLevel = PlayerPrefs.GetInt("currentLevel");
		//maxLevel = PlayerPrefs.GetInt("maxLevel");
            //Else spawn the player in a random direction
            player = Instantiate(pplayer, new Vector3(-50, -50, 0), Quaternion.identity).transform.GetChild(0).GetComponent<PlayerBehaviour>();
            player.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            //Zero the rigidbody velocity
            Rigidbody2D pRb = player.GetComponent<Rigidbody2D>();
            pRb.velocity = Vector3.zero;
            //Add a force to the player   
            pRb.AddForce(player.transform.right * 30);
            //Spawn system
            SpawnSystem(currentLevel);
        }
        player = FindObjectOfType<PlayerBehaviour>();
    }

    public void SpawnSystem(int index)
    {
        lastLevel = currentLevel;
        currentLevel = index;
        //Spawn the right system in front of it
        if (index < levels.Length && index >= 0)
        {
            Vector3 newpos = player.transform.position + player.transform.right * 50;
            currentSolarSystem = Instantiate(levels[index].prefab, newpos, Quaternion.identity).transform;
        }
        else
        {
            Debug.Log("Start system not found.");
        }
        //Rotate the system towards the right direction so that the player can have a safe anchor point
        Vector2 v = currentSolarSystem.transform.position - player.transform.position;
        float angle = Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
        currentSolarSystem.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        player.UpdateAvailablePlanets();
    }

	public IEnumerator Death()
	{
		Time.timeScale = 1;
		overlay.color = new Color(0,0,0,0);
		while(overlay.color.a < 1){
			
			overlay.color+= new Color(0,0,0,+0.01f);
			yield return null;
		}

		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield return null;
	}

	public IEnumerator Intro()
	{
		overlay.color = new Color(0,0,0,1);
		while(overlay.color.a > 0){
			
			overlay.color+= new Color(0,0,0,-0.01f);
			yield return null;
		}

		yield return null;
	}
	
}
