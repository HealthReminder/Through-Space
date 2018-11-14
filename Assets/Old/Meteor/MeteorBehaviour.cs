using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {
    public float vel, velMax;
	// Use this for initialization
	void Start () {
        vel = Random.Range(-velMax, velMax);
        while (vel > -0.5f && vel < 0.5f)
        {
            vel = Random.Range(-velMax, velMax);
        }
        GetComponent<RotateZ>().rotationSpeed = vel;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
