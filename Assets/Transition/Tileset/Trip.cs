using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trip : MonoBehaviour {
    public float wait;
    public float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < wait)
        {
            timer += Time.deltaTime;
        }
        if (timer >= wait)
        {
            gameObject.SetActive(false);
        }
	}
}
