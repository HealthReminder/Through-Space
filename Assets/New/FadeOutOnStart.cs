using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutOnStart : MonoBehaviour {

	Image sR;

	void Start () {
		sR = GetComponent<Image> ();
		sR.color = new Color (0, 0, 0, 1);
		StartCoroutine (FadeOut ());
	}
	IEnumerator FadeOut() {
		sR.enabled = true;
		while (sR.color.a > 0) {
			sR.color += new Color (0, 0, 0, -0.01f);
			yield return null;
		}
		sR.enabled = false;

		//Destroy (this.gameObject);


	}
}
