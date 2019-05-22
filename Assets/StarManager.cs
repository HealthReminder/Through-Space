/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class BodyData {

	[Header("Data")]
	public Transform transform;
	public float rotationSpeed;
	//public string musicSetName;
	public float influenceRadius;
	[Range(1,4)]
	public int gravitationalForce;

	//[Header("Lore")]
	//public string name;
	//public string typeOfPlanet;
	//public string howIsPronounced;
}


public class StarManager : MonoBehaviour {

	[SerializeField] public BodyData[] planets;
	
	void Update () {
		foreach(BodyData b in planets)
			b.transform.Rotate (0, 0, Time.deltaTime*b.rotationSpeed/5);
	}
	

}
 */