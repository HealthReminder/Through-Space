using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

	SpringJoint2D sj;
	Rigidbody2D rb;
	//bool dead = false;

	[Header("planetary")]
	public Planet closestPlanet, orbitingNow;
	public Transform orbitingStar;
	List<Planet> planetsAvailable;
    public bool hasArrived=true;

	[Header("Spring")]
	public bool attached=false;
	public bool spawnWithForce = true;
	public bool canAttach = false;
	
	[Header("GUI")]
	
	public Text TdistanceToStar;
	public GameObject TdistanceToStarObject;
    
    SpriteRenderer sptR;
	

	[Header("Angle")]
	float angle;
	Vector2 v;
	public CameraBehaviour camBehaviour;

	[Header("Managers")]
	SpaceTravelManager STMan;
	public Image deathScreen;
	public ParticleSystem deathparticle;

	public TimeController timeController;

    public float distanceFromStar;

	PlayerView playerView;
	
	

	#region Setup and System
	void Start () {
        sptR = GetComponent<SpriteRenderer>();
		STMan = FindObjectOfType<SpaceTravelManager>();
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
        Planet[] temp = FindObjectsOfType<Planet>();
        planetsAvailable = new List<Planet>();
        foreach (Planet p in temp)
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
        if (orbitingStar)
            distanceFromStar = Vector2.Distance(orbitingStar.position, transform.position);
        //65 far 80 too far 55 new star
		TdistanceToStar.text = distanceFromStar.ToString("F2") +" au";
        if (hasArrived)
        {	
			
			if(distanceFromStar < 20){
				if(TdistanceToStarObject.activeSelf)
				TdistanceToStarObject.SetActive(false);
			}
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
			if(!TdistanceToStarObject.activeSelf)
				TdistanceToStarObject.SetActive(true);
            if (distanceFromStar < 25)
            {
                hasArrived = true;
                timeController.ChangeTime(1);
				camBehaviour.AlignToStar(orbitingStar.transform);
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
			if(AmbientSoundController.instance)
				if(orbitingNow.GetComponent<BodyData>()){
					Debug.Log(orbitingNow.GetComponent<BodyData>().musicSetName);
					AmbientSoundController.instance.ChangeSet(orbitingNow.GetComponent<BodyData>().musicSetName);
				}
				else{
					BodyData bd = orbitingNow.transform.parent.GetComponent<BodyData>();
					Debug.Log(bd.musicSetName);
					if(bd != null)
						if(!string.IsNullOrEmpty(bd.musicSetName))
							AmbientSoundController.instance.ChangeSet(bd.musicSetName);
				}
				
				
			
		}
	}

	void FindClosestPlanet ()	{
		//Calculate the distance and get the closest one from all Planets
		Planet closest = null;
		float closestDist = 1000;
		//Reference Position and distance for Performance
		Vector3 planetPos;
		float dist=9999;
		foreach(Planet j in planetsAvailable) {
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
			playerView.ToggleProximity(k,planetPos);
			playerView.ToggleProximity(k,planetPos);
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
        sptR.color = Color.white;
        attached = false;
		sj.enabled = false;	
		orbitingNow = null;
		rb.AddForce (rb.velocity.normalized, ForceMode2D.Impulse);
		if(AmbientSoundController.instance)
			AmbientSoundController.instance.Stop(0.1f);
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
                if (collision.GetComponent<Objective>())
                {
                    print("Worked");
                    STMan.lastLevel = STMan.currentLevel;
                    STMan.currentLevel = collision.GetComponent<Objective>().nextLevel;
                    print("Called spawn system.");

                    STMan.SpawnSystem((int)STMan.currentLevel);
					camBehaviour.ToggleFollowCamera();

                }
                else
                {
                    print("Failed");
                }
            }
        }
    }
    public void CheckForProgress()
    {
        if (!STMan)
            STMan = FindObjectOfType<SpaceTravelManager>();
        print("Current player pref" + PlayerPrefs.GetInt("maxLevel") + " to: " + STMan.maxLevel);
        PlayerPrefs.SetInt("currentLevel", STMan.currentLevel);
        if (STMan.maxLevel > PlayerPrefs.GetInt("maxLevel"))
        {
            print("You are on a higher level");
           PlayerPrefs.SetInt("maxLevel", STMan.currentLevel);
        }
          
    }
    public void Die() {
		timeController.isOn = false;
		Time.timeScale = 1;
        CheckForProgress();
        GetComponent<SpriteRenderer> ().enabled = false;
		playerView.ShowProximity(false);
		GetComponent<Rigidbody2D> ().simulated = false;
		deathparticle.gameObject.SetActive(true);
		deathparticle.Play(true);
		StartCoroutine(STMan.Death());
	}
}
