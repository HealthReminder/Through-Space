using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour {

	public BodyData[] planets;
	
	void Update () {
		foreach(BodyData b in planets)
			b.transform.Rotate (0, 0, Time.deltaTime*b.rotationSpeed/5);
	}

}
