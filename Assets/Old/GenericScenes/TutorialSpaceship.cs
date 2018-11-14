using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialSpaceship : MonoBehaviour {

	SpringJoint2D sj;
	Rigidbody2D rb;

	public Transform orbiting;
	public bool attached=false;
	public bool spawnWithForce = true;

	float angle;
	Vector2 v;

	public bool isOn = false;

	public bool canAttach = false;
	public Image deathScreen;
	bool dead = false;
	public GameObject playerSoul;

	void Start () {
		if (FindObjectOfType<UIInput> () != null) {
			FindObjectOfType<UIInput> ().AddPlayerToVector ();
			transform.GetChild (0).GetComponent<LineRenderer> ().enabled = true;
		}
		PlayerSoul pS = Instantiate (playerSoul, transform.position, Quaternion.identity).GetComponent<PlayerSoul>();
		pS.FindPlayer ();

		deathScreen = FindObjectOfType<Image> ();
		rb = this.GetComponent<Rigidbody2D> ();
		sj = this.GetComponent<SpringJoint2D> ();

		if (spawnWithForce) {
			Detach ();
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
			rb.AddForce (transform.right * (9), ForceMode2D.Impulse);
		} else {
			rb.AddForce (transform.right * (-3), ForceMode2D.Impulse);
		}
			
		if(Camera.main.GetComponent<CameraBehaviour>() !=null)
			Camera.main.GetComponent<CameraBehaviour> ().FindPlayer ();
		Attach ();


	}

	void Update() {
		if(sj.enabled)
			sj.connectedAnchor = orbiting.position;
		if (isOn) {
			if (Input.GetMouseButtonDown (0)) {
				if (attached == true) {
					Detach ();
				} else {
					Attach ();
				}
			}

			if (attached) {
				v = rb.velocity;
				angle = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}

            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
	}



	void Attach() {
		if (orbiting != null) {
			attached = true;
			sj.enabled = true;	
			sj.connectedAnchor = orbiting.position;
			sj.distance = Vector2.Distance (transform.position, orbiting.position);
		}
	}

	void Detach() {
		attached = false;
		sj.enabled = false;	
		orbiting = null;
		rb.AddForce (transform.right*(2), ForceMode2D.Impulse);
		StartCoroutine (DetachedCooldown ());
	}



	void OnTriggerStay2D (Collider2D coll){
		if (canAttach) {
			if (coll.tag == "gravityField") {
				orbiting = coll.transform.parent;
			}
		}
		if (coll.tag == "surface") {
				StartCoroutine (Die ());
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

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.tag == "surface") {
			if(isOn)
				StartCoroutine (Die ());
		}

	}

	IEnumerator Die() {
		
		if (dead == false) {
			int chance = Random.Range (0, 20);
			if(chance == 0)
				FindObjectOfType<AudioManager> ().Play ("fart");
			else 
				FindObjectOfType<AudioManager> ().Play ("death");
			dead = true;
			GetComponent<SpriteRenderer> ().enabled = false;
			transform.GetChild (0).transform.gameObject.SetActive (false);
			GetComponent<Rigidbody2D> ().simulated = false;
			while (deathScreen.color.a < 1) {
				deathScreen.color += new Color (0, 0, 0, +0.025f);
				yield return null;
			}
			yield return new WaitForSeconds (0.7f);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

		}
		yield break;
	}

	
}
