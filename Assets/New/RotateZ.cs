using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZ : MonoBehaviour {

	public float rotationSpeed;

	void Update () {
		transform.Rotate (0, 0, Time.deltaTime*rotationSpeed/1);
	}
}
