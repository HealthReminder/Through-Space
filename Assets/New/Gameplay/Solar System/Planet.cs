using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	public new string name;
	public float influenceRadius;
	//Ranges from small/Mercury - 1   medium/Mars - 2   BIg/Venus - 3   Huge/Saturn - 4
	[Range(1,4)]
	public int gravitationalForce =1;
	public string howIsPronounced;
	public string system;
	public string layer;

}
