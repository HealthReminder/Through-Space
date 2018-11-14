using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

	SpringJoint2D sj;
	Rigidbody2D rb;
	bool dead = false;

	[Header("planetary")]
	public Planet closestPlanet, orbitingNow;
	Planet[] planetsAvailable;

	[Header("Spring")]
	public bool attached=false;
	public bool spawnWithForce = true;
	public bool canAttach = false;
	
	[Header("GUI")]
	public LineRenderer Lgravitational;
	public LineRenderer Lclosest;
	public Image deathScreen;


	float angle;
	Vector2 v;

	

	
	
	void Start () {
		planetsAvailable = FindObjectsOfType<Planet>();
		deathScreen = FindObjectOfType<Image> ();
		rb = this.GetComponent<Rigidbody2D> ();
		sj = this.GetComponent<SpringJoint2D> ();


		if (spawnWithForce) {
			Detach ();
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
			rb.AddForce (transform.right * (9), ForceMode2D.Impulse);
		} else {
			rb.AddForce (transform.right * (-2), ForceMode2D.Impulse);
		}
		Attach ();

		StartCoroutine (FixRotation ());

		FindObjectOfType<CameraBehaviour> ().FindPlayer();

	}

	void Update() {
		//Find closes planet
		FindClosestPlanet();
//		if (rb.velocity.x < 5) {
//			rb.AddForce (new Vector2(2, 0));
//		}
		if(closestPlanet){
			if (Input.GetMouseButtonDown (0)) {
			if (attached == true) {
				Detach ();
			} else {
				Attach ();
			}
		}
		}
		if(orbitingNow)
			sj.connectedAnchor = orbitingNow.transform.position;
				
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



	void Attach() {
		if (closestPlanet) {
			attached = true;
			sj.enabled = true;	
			sj.connectedAnchor = closestPlanet.transform.position;
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
			float k = Mathf.LerpUnclamped(0,closestPlanet.influenceRadius,dist);
			k = k/100;
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
				//Find
				float k = dist/orbitingNow.influenceRadius;
				//k = k/10;
				print(dist +" " + orbitingNow.influenceRadius+" "+k);
				Lgravitational.startColor = new Color(k,1-k,0,1);
				Lgravitational.endColor = new Color(k,1-k,0,1);
				Lgravitational.enabled = true;

			}
		} else {
			Lgravitational.enabled = false;
		}
		
	}
	void Detach() {
		attached = false;
		sj.enabled = false;	
		orbitingNow = null;
		rb.AddForce (transform.right*(1), ForceMode2D.Impulse);
		StartCoroutine (DetachedCooldown ());
	}

	IEnumerator FixRotation() {	
		while (true) {
			if (attached) {
				v = rb.velocity;
				angle = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}
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

	IEnumerator Die() {
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<LineRenderer> ().enabled = false;
		GetComponent<Rigidbody2D> ().simulated = false;
		while (deathScreen.color.a <1) {
			deathScreen.color += new Color (0, 0,0, +0.025f);
			yield return null;
		}
		yield return new WaitForSeconds (0.7f);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		yield break;
	}
}
