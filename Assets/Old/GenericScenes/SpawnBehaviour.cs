using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour {
	float altura;
	public float x, max, min;
	public GameObject player;
    public  bool canSpawn = true;
	void Update ()
	{
		if (altura > max)
			altura = max;
		if (altura < min)
			altura = min;
		altura += Input.GetAxis("Mouse Y");

		transform.position = new Vector2 (x, altura);
        if (canSpawn)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Instantiate(player, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
	}
}
