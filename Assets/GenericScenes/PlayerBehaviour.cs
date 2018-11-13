using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour {

	SpringJoint2D sj;
	Rigidbody2D rb;

	public Planet orbiting;
	public bool attached=false;
	public bool spawnWithForce = true;


	float angle;
	Vector2 v;

	public bool canAttach = false;

	public Image deathScreen;
	bool dead = false;
	void Start () {

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
//		if (rb.velocity.x < 5) {
//			rb.AddForce (new Vector2(2, 0));
//		}
		if(sj.enabled)
			sj.connectedAnchor = orbiting.transform.position;
		if (Input.GetMouseButtonDown (0)) {
			if (attached == true) {
				Detach ();
			} else {
				Attach ();
			}
		}
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



	void Attach() {
		if (orbiting != null) {
			attached = true;
			sj.enabled = true;	
			sj.connectedAnchor = orbiting.transform.position;
			sj.distance = Vector2.Distance (transform.position, orbiting.transform.position);
		}
	}

	void Detach() {
		attached = false;
		sj.enabled = false;	
		orbiting = null;
		rb.AddForce (transform.right*(2), ForceMode2D.Impulse);
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

	void OnTriggerStay2D (Collider2D coll){
		if (canAttach) {
			if (coll.tag == "gravityField") {
				orbiting = coll.transform.parent.GetComponent<Planet>();
			}
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
			if(dead==false)
				StartCoroutine (Die ());
			dead = true;
		}
		
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
