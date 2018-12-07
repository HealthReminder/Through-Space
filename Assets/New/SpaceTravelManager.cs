﻿using System.Collections;
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
        //Play intro
        StartCoroutine(Intro());
        //Get current progress
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        maxLevel = PlayerPrefs.GetInt("maxLevel");

        print(PlayerPrefs.GetInt("currentLevel") + "  " + PlayerPrefs.GetInt("maxLevel"));

       

        //Else spawn the player in a random direction
        if (currentLevel != 0)
        {

            Rigidbody2D pRb;
            player = Instantiate(pplayer, new Vector3(-50, -50, 0), Quaternion.identity).transform.GetChild(0).GetComponent<PlayerBehaviour>();
            pRb = player.GetComponent<Rigidbody2D>();
            //Zero the rigidbody velocity
            pRb.velocity = Vector3.zero;
            //Rotate the player 
            player.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));


            //Add a force to the player  
            Debug.DrawRay(player.transform.position, player.transform.right * 50, Color.red, 100000);
            pRb.AddForce(player.transform.right * 30);
            //Spawn system
        }
        SpawnSystem(currentLevel);
        //Find player
        player = FindObjectOfType<PlayerBehaviour>();
        //Set its transform to this object so it doesn't get destroyed or disabled by the solar systems
        player.transform.parent.SetParent(transform);
    }

    public void SpawnSystem(int index)
    {
        if (lastLevel != index) { 
        lastLevel = currentLevel;
        currentLevel = index;
        //Spawn the right system in front of it
        if (index < levels.Length && index >= 0)
        {

            if (currentLevel == 0)
            {
                    Debug.Log("Generating current level.");
                    currentSolarSystem = Instantiate(levels[index].prefab, new Vector3(0, 0, 0), Quaternion.identity).transform;
            }
            else
            {
                Debug.Log("Generating longiquous star system.");
                    Vector3 newpos = player.transform.position + player.transform.right * 50;
                    
                currentSolarSystem = Instantiate(levels[index].prefab, newpos, Quaternion.identity).transform;
                
                }

               //Rotate the system towards the right direction so that the player can have a safe anchor point
                if(!player)
                    player = FindObjectOfType<PlayerBehaviour>();
                //Find rotation vector
                Vector2 v = currentSolarSystem.transform.position - player.transform.position;
                //Find angle using the rotation vector
                float angle = Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
                //Rotate system
                currentSolarSystem.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
                //Find the new planets it can orbit (performance)
                player.UpdateAvailablePlanets();
                

            }
        else
        {
            Debug.Log("Start system not found.");
        }
       
     } else
        {
            print("Same level?");
        }
        Debug.DrawRay(player.transform.position, player.transform.right.normalized * 50, Color.cyan, 100000);
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
