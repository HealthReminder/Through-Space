using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetData : MonoBehaviour {

	

	[Header("Information")]
	public new string name;
	[SerializeField]	public Track ambientSound;
	//public string howIsPronounced;
	//public Transform startSystem;
	[Header("Properties")]
	
	public float influenceRadius;
	//Ranges from small/Mercury - 1   medium/Mars - 2   BIg/Venus - 3   Huge/Saturn - 4
	[Range(1,4)]
	public int gravitationalForce =1;

}
