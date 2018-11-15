using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn : MonoBehaviour {
    float timer, dist;
    public int chance;
    int rand;
    public float tempo, velMax, distMax;
    public GameObject meteor, meteorCapsule;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= tempo)
        {
            timer = 0;
            rand = Random.Range(0, 100);
            dist = Random.Range(20, 22);
            if(rand <= chance)
            {
                meteorCapsule.GetComponent<MeteorBehaviour>().velMax = velMax;
                var newMeteorCapsule = Instantiate(meteorCapsule);
                var newMeteor = Instantiate(meteor, new Vector3(0, dist, 0),transform.rotation);
                newMeteor.transform.parent = newMeteorCapsule.transform;
            }
        }
	}
}
