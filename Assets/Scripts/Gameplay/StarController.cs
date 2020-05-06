using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class StarController : MonoBehaviour {

	public string name;
	public Color mainColor; public Color detailColor;
	public BodyData[] rotationData;
	[SerializeField]	public PlanetData[] planetDatas;
 
	
	void Update () {
		foreach(BodyData b in rotationData)
			b.transform.Rotate (0, 0, Time.deltaTime*b.rotationSpeed/5);			
	}

}
