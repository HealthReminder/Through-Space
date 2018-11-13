using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour {

	AudioManager aM;
	public Text title, pressAnyKey;
	public TextWriter tW;

	void Start () {
		aM = FindObjectOfType<AudioManager> ();
		StartCoroutine (TitleSequence ());
	}

	IEnumerator TitleSequence() {
		FindObjectOfType<SoundtrackManager> ().PlaySet (0);
		int state = 0;
		//pressAnyKey.color += new Color (0, 0, 0, -1);

		while (state == 0) {
			int flicks = 0;
			while (flicks <= 9) {

				title.color += new Color (0, 0, 0, -1);
				yield return new WaitForSeconds (0.01f);
				title.color += new Color (0, 0, 0, 1);
				aM.Play ("flicker");
				yield return new WaitForSeconds (0.01f);
				flicks++;
				yield return null;
			}
			flicks = 0;
			while (flicks <= 6) {
				title.color += new Color (0, 0, 0, -1);
				yield return new WaitForSeconds (0.05f);
				title.color += new Color (0, 0, 0, 1);
				aM.Play ("flicker");
				yield return new WaitForSeconds (0.05f);
				flicks++;
				yield return null;
			}
			flicks = 0;
			while (flicks <= 3) {
				title.color += new Color (0, 0, 0, -1);
				yield return new WaitForSeconds (0.1f);
				title.color += new Color (0, 0, 0, 1);
				aM.Play ("flicker");
				yield return new WaitForSeconds (0.1f);
				flicks++;
				yield return null;
			}

			yield return null;
			state = 1;
		}
		while (state == 1) {
			bool done = false;
			while (!Input.anyKey) {
				title.color += new Color (0, 0, 0, 1);
				pressAnyKey.color = new Color (1, 1, 1, 1);
				aM.Play ("beepSuccess");
				yield return null;
			}
			yield return new WaitForSeconds (0.5f);
			title.color = new Color (1, 1, 1, -1);
			pressAnyKey.color = new Color (1, 1, 1, -1);
			yield return new WaitForSeconds (1f);
			yield return null;
			state = 2;
		}
		while (state == 2) {
			tW.WriteText ("booting (A:)\n.\n.\n.\nboot successful\n.\n.\n.\n.\ninitializing AD4M.exe....................", 0.1f);
			while (tW.writing == true) {
				yield return null;
			}
			tW.Clear ();
			yield return new WaitForSeconds (0.5f);
			yield return null;
			state = 3;
		}
		while (state == 3) {
			tW.WriteText ("Starting calibration sequence\n.\n.\n.\n.\n.\n.\n.\n.\n.", 0.085f);
			while (tW.writing == true) {
				yield return null;
			}
			tW.Clear ();
			yield return new WaitForSeconds (0.5f);
			yield return null;
			state = 4;
		}
			
		SceneManager.LoadScene ("level1");



		yield break;
	}
}
