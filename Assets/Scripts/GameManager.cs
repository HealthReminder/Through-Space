﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("player Information")]
    public int lastLevel;
	public int currentLevel;
	public int maxLevel;
	[Header("prefabs")]
	public GameObject pplayer;
	PlayerManager player;
    Rigidbody2D pRb;

    public GameObject[] levels;
	[Header("GUI")]
	public Image overlay;
	[Header("System Information")]
	public StarController currentSolarSystem;
    GameObject oldSolarSystem;
    [Header("Development")]
    [SerializeField]
    bool overrideLevel;
    [SerializeField]
    int newLevel = 0;
    
    public static GameManager instance;
    private void Awake() {
        instance = this;
    }
	
	
	void Start () {
        //Play intro
        StartCoroutine(Intro());
        //Get current progress
        //PlayerPrefs.SetInt("currentLevel", 1);
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        maxLevel = PlayerPrefs.GetInt("maxLevel");

        // print(PlayerPrefs.GetInt("currentLevel") + "  " + PlayerPrefs.GetInt("maxLevel"));

        //Dev only
        if (overrideLevel)
            currentLevel = newLevel;
       

        //Else spawn the player in a random direction
        if (currentLevel != 0)
        {

           
            player = Instantiate(pplayer, new Vector3(-50, -50, 0), Quaternion.identity).transform.GetChild(0).GetComponent<PlayerManager>();
            if(!pRb)
                pRb = player.GetComponent<Rigidbody2D>();

            //Zero the rigidbody velocity
            pRb.velocity = Vector3.zero;
            //Rotate the player 
            player.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));


            //Add a force to the player  
            Debug.DrawRay(player.transform.position, player.transform.right * 50, Color.red, 100000);
            //pRb.AddForce(player.transform.right * 30);
            //Spawn system
        }
        SpawnSystem(currentLevel);
        //Find player
        player = FindObjectOfType<PlayerManager>();
        //Set its transform to this object so it doesn't get destroyed or disabled by the solar systems
        player.transform.parent.SetParent(transform,true);
        //transform.SetParent(player.transform);
    }

    public void SpawnSystem(int index)
    {
        if (lastLevel != index) { 
        lastLevel = currentLevel;
        currentLevel = index;
        //Spawn the right system in front of it
        if (index < levels.Length && index >= 0)
        {
                if (oldSolarSystem)
                    oldSolarSystem.SetActive(false);
                if (currentSolarSystem)
                    oldSolarSystem = currentSolarSystem.gameObject;

            if (currentLevel == 0)
            {
                    Debug.Log("Generating first level.");
                    currentSolarSystem = Instantiate(levels[index], new Vector3(0, 0, 0), Quaternion.identity).GetComponent<StarController>();
            }
            else
            {
                    pRb = player.GetComponent<Rigidbody2D>();
                    pRb.velocity = Vector3.zero;
                    //Add a force to the player  
                   // Debug.DrawRay(player.transform.position, player.transform.right * 50, Color.red, 100000);
                    pRb.AddForce(player.transform.right * 200);

                    Debug.Log("Generating longiquous star system.");
                    Vector3 newpos = player.transform.position + player.transform.right * 50*4;
                    
                    currentSolarSystem = Instantiate(levels[index], newpos, Quaternion.identity).GetComponent<StarController>();
                    player.hasArrived = false;
                }

               //Rotate the system towards the right direction so that the player can have a safe anchor point
                if(!player)
                    player = FindObjectOfType<PlayerManager>();
                //Find rotation vector
                Vector2 v = currentSolarSystem.transform.position - player.transform.position;
                //Find angle using the rotation vector
                float angle = Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
                //Rotate system
                currentSolarSystem.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
                //Find the new planets it can orbit (performance)
                player.UpdateAvailablePlanets();
                player.currentOrbitingStar = currentSolarSystem;
                Vector3 newCameraPos = currentSolarSystem.transform.position;
                newCameraPos.z = player.camBehaviour.wideCamera.transform.position.z;
                player.camBehaviour.wideCamera.transform.position = newCameraPos;
               
               // player.camBehaviour.followCamera.transform.rotation = 
                //Quaternion.LookRotation(player.orbitingStar.transform.right,
               // player.camBehaviour.followCamera.transform.up);

                //Debug.DrawRay(player.orbitingStar.transform.position,player.orbitingStar.transform.right*20,Color.red,10);
               // Debug.DrawRay(player.camBehaviour.followCamera.transform.position,player.camBehaviour.followCamera.transform.up*20,Color.green,10);

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
        if (currentLevel > maxLevel)
            maxLevel = currentLevel;
        player.CheckForProgress();
    }

	public IEnumerator Ending()
	{
		Time.timeScale = 1;
        yield return new WaitForSeconds(3f);
		overlay.color = new Color(0,0,0,0);
		while(overlay.color.a < 1){
			
			overlay.color+= new Color(0,0,0,+0.05f);
			yield return null;
		}

		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(0);
		yield return null;
	}

    public IEnumerator Death()
	{
		Time.timeScale = 1;
		overlay.color = new Color(0,0,0,0);
        yield return new WaitForSeconds(1f);
		while(overlay.color.a < 1){
			
			overlay.color+= new Color(0,0,0,+0.05f);
			yield return new WaitForSeconds(Time.deltaTime*2);
		}

		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield return null;
	}

	public IEnumerator Intro()
	{
		overlay.color = new Color(0,0,0,1);
		while(overlay.color.a > 0){
			
			overlay.color+= new Color(0,0,0,-0.1f);
			yield return new WaitForSeconds(0.1f);
		}

		yield return null;
	}
	
}
