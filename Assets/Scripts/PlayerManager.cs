using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour {

	public bool isDead = false;
	SpringJoint2D sj;
	Rigidbody2D rb;
	//bool dead = false;
	public bool isInFirstLevel = false;
	
	[Header("Environment Information")]
    public float distanceFromStar;
	public PlanetData closestPlanet, orbitingNow;
	public StarController currentOrbitingStar;
	List<PlanetData> planetsAvailable;
    public bool hasArrived=true;

	[Header("Spring")]
	public bool attached=false;
	public bool spawnWithForce = true;
	public bool canAttach = false;	

	[Header("Angle")]
	float angle;
	Vector2 v;
	public CameraManager camBehaviour;

	[Header("References")]
	GameManager gameManager;

	PlayerView playerView;
	public TimeController timeController;
	
	

	#region Setup and System
	void Start () {
		gameManager = GameManager.instance;
		//if(!isInFirstLevel)
			Time.timeScale = 1;
        //print(sj.connectet)
        UpdateAvailablePlanets();
		rb = this.GetComponent<Rigidbody2D> ();
		sj = this.GetComponent<SpringJoint2D> ();
		playerView = PlayerView.instance;

		Attach ();

		StartCoroutine (FixRotation ());

        if (spawnWithForce)
            rb.AddForce(transform.right*300);

	}

	public void UpdateAvailablePlanets()
    {
        PlanetData[] temp = FindObjectsOfType<PlanetData>();
        planetsAvailable = new List<PlanetData>();
        foreach (PlanetData p in temp)
            planetsAvailable.Add(p);

    }

	IEnumerator FixRotation() {	
		while (true) {
                v = rb.velocity;
                angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
			yield return null;
		}
	}

	IEnumerator DetachedCooldown() {
		canAttach = false;
		int frames = 12;

		while (frames > 0) {
			canAttach = false;
			frames--;
			yield return null;
		}

		canAttach = true;
		yield break;
	}


	#endregion

    

	void Update() {
		//Find closes planet
		FindClosestPlanet();
//		if (rb.velocity.x < 5) {
//			rb.AddForce (new Vector2(2, 0));
//		}
		//If the tlayer tress K or J change timeScale

		if(Input.GetKeyDown(KeyCode.Q))
			timeController.ChangeTime(10);
		if(Input.GetKeyUp(KeyCode.Q))
			timeController.ChangeTime(1);

		if(Input.GetKeyDown(KeyCode.E))
			PressedOnScreen();
		if(Input.GetKeyUp(KeyCode.E))
			ReleasedScreen();
		
		if(Input.GetKeyDown(KeyCode.W))
			camBehaviour.ToggleWideCamera();
		if(Input.GetKeyUp(KeyCode.W))
			camBehaviour.ToggleWideCamera();

		

		if(orbitingNow)
			sj.connectedAnchor = orbitingNow.transform.position;
        if (currentOrbitingStar){
            distanceFromStar = Vector2.Distance(currentOrbitingStar.transform.position, transform.position);
			playerView.ChangeDistanceText(distanceFromStar.ToString("F2") +" au"); 
		}
		
        if (hasArrived)
        {	
			
			if(distanceFromStar < 20)
				playerView.ToggleDistanceFromStar(0);
			else if (distanceFromStar >= 50)
            {
                // print(distanceFromStar + " TOO FAR");
                Die();
            }
            else if (distanceFromStar >= 40)
            {
                 //print(distanceFromStar + " CAREFUL");
            }
        } else
        {
			playerView.ToggleDistanceFromStar(1);
            if (distanceFromStar < 25)
            {
                hasArrived = true;
                timeController.ChangeTime(1);
				camBehaviour.AlignToStar(currentOrbitingStar.transform);
                //print(distanceFromStar + " haha");
            }else  if (distanceFromStar < 60)
            {
                timeController.ChangeTime(10);
                //print(distanceFromStar + " haha");
            } else  if (distanceFromStar < 120)
            {
                timeController.ChangeTime(20);
                //print(distanceFromStar + " haha");
            } else {
				timeController.ChangeTime(30);
			}
        }
		
    }
	#region Player Interaction
    public void PressedOnScreen()
    {
        //print("clicked");
        if (closestPlanet) { 
           // print("is in rect");
            if (attached == false)       
                Attach();   
			if(isInFirstLevel)
				Time.timeScale = 1;         
        }
    }

	public void ReleasedScreen()
    {
        //print("clicked");
        if (closestPlanet) { 
			if (attached == true)   
            	Detach();
        }
    }
	#endregion



	void Attach() {
		if (closestPlanet) {
			
			attached = true;
			sj.enabled = true;	
			sj.connectedAnchor = closestPlanet.transform.position;
            rb.AddForce(transform.right * closestPlanet.gravitationalForce * closestPlanet.gravitationalForce * 4);
            Debug.DrawRay(transform.position, transform.right * closestPlanet.gravitationalForce,Color.blue, 10);

			//sj.frequency = (float)closestPlanet.gravitationalForce/4;
			//The distances must be lower for less dense planet
			//sj.distance = Vector2.Distance (transform.position+new Vector3(rb.velocity.x,rb.velocity.y,0)*3, closestPlanet.transform.position);
			sj.distance = Vector2.Distance (transform.position, closestPlanet.transform.position);
			orbitingNow = closestPlanet;

			//Find right music set
			if(AmbientSoundManager.instance)
				if(orbitingNow.ambientSound != null)
					AmbientSoundManager.instance.StartAmbientSound(orbitingNow.ambientSound);
			
				
				
			
		}
	}

	void FindClosestPlanet ()	{
		//Calculate the distance and get the closest one from all Planets
		PlanetData closest = null;
		float closestDist = 1000;
		//Reference Position and distance for Performance
		Vector3 planetPos;
		float dist=9999;
		foreach(PlanetData j in planetsAvailable) {
			planetPos = j.transform.position;
			dist = Vector2.Distance(new Vector2(planetPos.x,planetPos.y), new Vector2(transform.position.x,transform.position.y));
			if(j == orbitingNow)
				if(dist > j.influenceRadius)
					Detach ();
			
			if(dist <= closestDist){
				//Found closest planet
				//Check if you are in range
				if(dist <= j.influenceRadius){
					closestDist = dist; 
					closest = j;
					//Now you can orbit a Planet
				}
			}
		}
		
		closestPlanet = closest;
		//Change line according to result
		if(closestPlanet){
			if(!playerView.isProximityOn)
				playerView.ShowProximity(true);
			
			//Get the position of the closes planet
			planetPos = closestPlanet.transform.position;
			//Get 0 - 1 float
			dist = Vector2.Distance(new Vector2(planetPos.x,planetPos.y), new Vector2(transform.position.x,transform.position.y));
			float k = dist/closestPlanet.influenceRadius;
			
			if(k >= 0.75f)
				if(orbitingNow)
			    	timeController.ChangeTime(1);
			playerView.UpdateProximity(k,planetPos);
			playerView.UpdateProximity(k,planetPos);
			if(orbitingNow){
				playerView.ShowOrbit(true);
				playerView.ToggleOrbit(k,orbitingNow.transform.position);
			}
			else
			{
				playerView.ShowOrbit(false);
			}
			
		} else 
			if(playerView.isProximityOn)
				playerView.ShowProximity(false);
				
		
	}
	void Detach() {
        attached = false;
		sj.enabled = false;	
		orbitingNow = null;
		rb.AddForce (rb.velocity.normalized, ForceMode2D.Impulse);
		if(AmbientSoundManager.instance)
			AmbientSoundManager.instance.StopAmbientSound();
		StartCoroutine (DetachedCooldown ());
	}

	
	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
        if (collisionInfo.gameObject.tag == "surface")
            Die();        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "objective")
        {
            if (!orbitingNow)
            {
                print("Touched objective");
                if (collision.GetComponent<ObjectiveData>())
                {
                    print("Worked");
                    gameManager.lastLevel = gameManager.currentLevel;
                    gameManager.currentLevel = collision.GetComponent<ObjectiveData>().nextLevel;
                    print("Called spawn system.");

                    gameManager.SpawnSystem((int)gameManager.currentLevel);
					camBehaviour.ToggleFollowCamera();

                }
                else
                {
                    print("Failed");
                }
            }
        } else {
			if(collision.gameObject.tag == "blackHole")
				End();
		}
    }

	public void End() {
		Time.timeScale = 1;
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;
		playerView.ToggleGUIContainer(0);
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);			
		}

		StartCoroutine(gameManager.Ending());
	}
    public void CheckForProgress()
    {
        if (!gameManager)
            gameManager = FindObjectOfType<GameManager>();
        print("Current max level changed from " + PlayerPrefs.GetInt("maxLevel") + " to: " + gameManager.maxLevel);
        PlayerPrefs.SetInt("currentLevel", gameManager.currentLevel);
        if (gameManager.maxLevel > PlayerPrefs.GetInt("maxLevel"))
        {
            print("You are on a higher level");
           PlayerPrefs.SetInt("maxLevel", gameManager.currentLevel);
        }
          
    }
    public void Die() {
		if(isDead)
			return;
		isDead = true;
		playerView.OnDeath();
		timeController.isOn = false;
		Time.timeScale = 1;
        CheckForProgress();
        GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<Rigidbody2D> ().simulated = false;
		StartCoroutine(gameManager.Death());
	}
}
