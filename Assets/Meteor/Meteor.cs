using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {
    float speed;
    float y;
	// Use this for initialization
	void Start () {
        Color newColor = new Color(0.2f+Random.Range(0.2f, 1), 0.2f+Random.Range(0.5f, 1), 0.2f+Random.Range(0.5f, 1),1);
        GetComponent<SpriteRenderer>().color = newColor;
        transform.GetChild(0).GetComponent<TrailRenderer>().startColor = newColor-new Color(0.2f,0.2f,0.2f);
        transform.GetChild(0).GetComponent<TrailRenderer>().endColor= newColor-new Color(0.2f,0.2f,0.2f);
        speed = Random.Range(0.01f, 0.05f);
        y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        y -= speed;
        transform.localPosition = new Vector3(0, y, 0);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "surface")
        {
            Disable();
        }
    }

    void Disable() {
        Destroy(gameObject.transform.parent.gameObject);
        
    }
}
