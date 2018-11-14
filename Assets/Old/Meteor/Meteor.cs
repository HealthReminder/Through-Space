using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {
    float speed;
    float y;
	// Use this for initialization
	void Start () {
        Color newColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1),1);
        GetComponent<SpriteRenderer>().color = newColor;
        string mainColor = "red";
        if (newColor.g > newColor.r)
            mainColor = "green";
        if (mainColor == "green")
            if (newColor.b > newColor.g)
                mainColor = "blue";
        if (mainColor == "red")
            if (newColor.b > newColor.r)
                mainColor = "blue";

        if (mainColor == "red")
            newColor.r += 1;
        else if (mainColor == "green")
            newColor.g += 1;
        else if (mainColor == "blue")
            newColor.b += 1;
        transform.GetChild(0).GetComponent<TrailRenderer>().startColor = newColor;
        transform.GetChild(0).GetComponent<TrailRenderer>().endColor= newColor;
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
        transform.DetachChildren();
        Destroy(gameObject.transform.parent.gameObject);
        
    }
}
