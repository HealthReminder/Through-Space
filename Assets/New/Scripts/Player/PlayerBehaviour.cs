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
	public LineRenderer Lgravitational;
	public LineRenderer Lclosest;
    
    SpriteRenderer sptR;
	

	[Header("Angle")]
	float angle;
	Vector2 v;
	public CameraBehaviour camBehaviour;

	[Header("Managers")]
	SpaceTravelManager STMan;
	public Image deathScreen;
	public ParticleSystem deathparticle;

	public TimeController TMan;

    public float distanceFromStar;

	
	
	void Start () {
        sptR = GetComponent<SpriteRenderer>();
		STMan = FindObjectOfType<SpaceTravelManager>();
		Time.timeScale = 1;
        //print(sj.connectet)
        UpdateAvailablePlanets();
		rb = this.GetComponent<Rigidbody2D> ();
		sj = this.GetComponent<SpringJoint2D> ();

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

	void Update() {
		//Find closes planet
		FindClosestPlanet();
//		if (rb.velocity.x < 5) {
//			rb.AddForce (new Vector2(2, 0));
//		}
		//If the tlayer tress K or J change timeScale
		

		if(orbitingNow)
			sj.connectedAnchor = orbitingNow.transform.position;
        if (orbitingStar)
            distanceFromStar = Vector2.Distance(orbitingStar.position, transform.position);
        //65 far 80 too far 55 new star
        if (hasArrived)
        {
            if (distanceFromStar >= 50)
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
            if (distanceFromStar < 35)
            {
                hasArrived = true;
                TMan.ChangeTime(1);
                //print(distanceFromStar + " haha");
            }
        }
		
    }

    public void PressedOnScreen()
    {
        print("clicked");
        if (closestPlanet) { 
            print("is in rect");
            if (attached == true)
            {
                Detach();
            }
            else
            {
                Attach();
            }
        }
    }



	void Attach() {
		if (closestPlanet) {
			
			attached = true;
			sj.enabled = true;	
			sj.connectedAnchor = closestPlanet.transform.position;
            rb.AddForce(transform.right * closestPlanet.gravitationalForce * closestPlanet.gravitationalForce * 10);
            Debug.DrawRay(transform.position, transform.right * closestPlanet.gravitationalForce,Color.blue, 10);

			//sj.frequency = (float)closestPlanet.gravitationalForce/4;
			//The distances must be lower for less dense planet
			//sj.distance = Vector2.Distance (transform.position+new Vector3(rb.velocity.x,rb.velocity.y,0)*3, closestPlanet.transform.position);
			sj.distance = Vector2.Distance (transform.position, closestPlanet.transform.position);
			orbitingNow = closestPlanet;
			
		}
	}

	void FindClosestPlanet (){
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
			planetPos = closestPlanet.transform.position;
			dist = Vector2.Distance(new Vector2(planetPos.x,planetPos.y), new Vector2(transform.position.x,transform.position.y));
			Lclosest.SetPosition(0,transform.position);
			Lclosest.SetPosition(1, closestPlanet.transform.position);
			float k = dist/closestPlanet.influenceRadius;
			Lclosest.colorGradient = new Gradient();
            Lclosest.startColor = new Color(1,1,1,1-k);
			Lclosest.endColor = new Color(1,1,1,1-k);
			Lclosest.enabled = true;
		} else {
			Lclosest.enabled = false;
		}

		//Utdate gravitational line 
		if(orbitingNow){
			planetPos = orbitingNow.transform.position;
			dist = Vector2.Distance(new Vector2(planetPos.x,planetPos.y), new Vector2(transform.position.x,transform.position.y));
			
			if(dist > orbitingNow.influenceRadius){
				//Too far
				orbitingNow = null;
				Lgravitational.enabled = false;
			} else {
				//Close enough
				//Set line tositions
				Lgravitational.SetPosition(0,transform.position);
				Lgravitational.SetPosition(1, orbitingNow.transform.position);
				//Find progress
				float k = dist/orbitingNow.influenceRadius;
				//if(k >= 0.9f)
				//	TMan.ChangeTime(1);else 
				if(k >= 0.75f)
					TMan.ChangeTime(1);
                //k = k/10;
                //				print(dist +" " + orbitingNow.influenceRadius+" "+k);

                //Calculate the color for the player
                float H, S, V;
                Color.RGBToHSV(new Color(k, 1 - k, 0, 1), out H, out S, out V);
                sptR.color = Color.HSVToRGB(H, S, 1);

				Lgravitational.startColor = new Color(k,1-k,0,1);
				Lgravitational.endColor = new Color(k,1-k,0,1);
				Lgravitational.enabled = true;

			}
		} else {
			Lgravitational.enabled = false;
		}
		
	}
	void Detach() {
        sptR.color = Color.white;
        attached = false;
		sj.enabled = false;	
		orbitingNow = null;
		rb.AddForce (rb.velocity.normalized, ForceMode2D.Impulse);
		StartCoroutine (DetachedCooldown ());
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
		Time.timeScale = 1;
        CheckForProgress();
        GetComponent<SpriteRenderer> ().enabled = false;
		Lgravitational.enabled = false;
		Lclosest.enabled = false;
		GetComponent<Rigidbody2D> ().simulated = false;
		deathparticle.gameObject.SetActive(true);
		deathparticle.Play(true);
		StartCoroutine(STMan.Death());
	}
}
