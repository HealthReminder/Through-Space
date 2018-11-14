using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransicao : MonoBehaviour {
    public int anim;
    public float timer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer < 2)
        {
            anim = 0;
        }
        else
        {
            anim = 1;
            if (timer > 2.5f)
            {
                anim = 2;
            }
        }

        if (transform.position.x > 0 && anim == 0)
        {
            transform.Translate(new Vector3(-0.05f, 0, 0));
            FindObjectOfType<AudioManager>().Play("transition");
        }
        if (anim == 1)
        {
          
            transform.Translate(new Vector3(0.005f, 0, 0));
        }
        if (anim == 2)
        {
            transform.Translate(new Vector3(-0.2f, 0, 0));
        }
    }
}
