using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour {

	AudioManager aM;

	public Text textBox;
	public bool writing = false;

	void Awake() {aM = FindObjectOfType<AudioManager> ();}

	public void WriteText(string text, float interval) {
		writing = true;
		StartCoroutine (WriteCo (text,interval));
	}

	IEnumerator WriteCo(string text,float interval) {
		Clear ();
		char[] newText= text.ToCharArray();
		int index = 0;
		while (textBox.text != text) {

			textBox.text += newText [index];
			index++;
			aM.Play ("write");
			yield return new WaitForSeconds (interval);
			yield return null;
		}
		int flicker = 5;

		while (flicker > 0) {
		
			textBox.color += new Color (0, 0, 0, -1);
			yield return new WaitForSeconds (0.05f);
			textBox.color += new Color (0, 0, 0, 1);
			yield return new WaitForSeconds (0.05f);
			flicker--;
			yield return null;
		
		}

		writing = false;
		yield break;
	}

	public void Clear() {
		textBox.text = null;
	}

}
