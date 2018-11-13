using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	public int waitBetweenInstructions = 1;
	public float writingSpeed = 0.1f;
	TutorialSpaceship player;
	TextWriter text;
	CameraBehaviour mainCamera;
	UIInput uiInput;
	public Image blackscreen1,blackscreen2;
	public int state = 0;
	AudioManager aM;
	void Start () {
		aM = FindObjectOfType<AudioManager> (); 
		player = FindObjectOfType<TutorialSpaceship> ();
		text = this.GetComponent<TextWriter> ();
		mainCamera = FindObjectOfType<CameraBehaviour> ();
		uiInput = FindObjectOfType<UIInput> ();

		StartCoroutine (TutorialSequence());
	}
		

	IEnumerator TutorialSequence() {
		FindObjectOfType<SoundtrackManager> ().PlaySet (1);
		blackscreen1.color = new Color (0, 0, 0, 1);
		player.isOn = false;
		mainCamera.isOn = false;
		uiInput.isOn = false;

		bool pulaEntrada, pulaOne,pulaOrbita, textoEmocional,pulaFinal, pulaTutorial;
		pulaTutorial = pulaEntrada = pulaOne = pulaOrbita = textoEmocional = pulaFinal = false;
		//pulaEntrada = pulaOne = pulaOrbita = textoEmocional = pulaFinal = true;

		bool skippson = false;


		text.WriteText ("Do you want to go through the calibration process? \n\n Press {Y} for yes. \n Press {N} for no.", writingSpeed);
		while (text.writing == true)
			yield return null;
		aM.Play ("beepCommon");
		while (skippson == false) {
			if (Input.GetKeyDown (KeyCode.Y)) {
				pulaTutorial = false;
				skippson = true;
			} else if (Input.GetKeyDown (KeyCode.N)) {
				pulaTutorial = true;
				skippson = true;
			}
			
			yield return null;

		}
		aM.Play ("beepSuccess");
		text.Clear ();

		if (!pulaTutorial) {
			if (!pulaEntrada) {
				//Rise and shine Adam.
				text.WriteText ("Rise and shine Adam.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
				text.Clear ();
				//Test the {5} number key.
				text.WriteText ("Test the {5} number key.", writingSpeed);
				while (text.writing == true)
					yield return null;
				while (Input.GetKeyDown (KeyCode.Alpha5) == false)
					yield return null;
				//Great.
				aM.Play ("beepSuccess");
				text.WriteText ("Great.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);

				text.Clear ();
			}

		}
			while (blackscreen1.color.a > 0) {
				blackscreen1.color += new Color (0, 0, 0, -0.05f);
				yield return null;
			}
			uiInput.isOn = true;
			uiInput.canInputAssist = uiInput.canInputFields = uiInput.canInputNames = false;

		if (!pulaTutorial) {
			if (!pulaOne) {
				//This is your home star Sol.
				text.WriteText ("This is your home star Sol.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
				//You can turn names on by pressing the {1} button.
				text.WriteText ("You can turn names on by pressing the {1} number key.", writingSpeed);
				while (text.writing == true)
					yield return null;
				uiInput.canInputNames = true;
				while (Input.GetKeyDown (KeyCode.Alpha1) == false)
					yield return null;
				aM.Play ("beepSuccess");
				uiInput.canInputNames = false;
				text.Clear ();
				uiInput.canInputNames = false;

				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);
				uiInput.names.Toggle ();
				yield return new WaitForSeconds (0.5f);

				text.WriteText ("Flawless.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
	
				text.Clear ();
				//Take a look at you orbiting your home planet Tera.
			}

			if (!pulaOrbita) {

				text.WriteText ("Take a look at you orbiting your home planet Tera.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
				//Shows planet.
				mainCamera.GetComponent<Camera> ().orthographicSize = 4;
				int ticks = 360;
				while (ticks > 0) {
					mainCamera.transform.position = new Vector3 (player.orbiting.position.x, player.orbiting.position.y, mainCamera.transform.position.z);
					ticks--;
					yield return null;
				}
				mainCamera.transform.position = new Vector3 (0, 0, mainCamera.transform.position.z);
				mainCamera.GetComponent<Camera> ().orthographicSize = 15;
				//Pretty small.
				text.WriteText ("Pretty small.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
		
				//After I am done teaching you, you may use the right mouse button
				text.WriteText ("After I am done teaching you, you may use the right mouse button", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
	
				text.Clear ();
				//to change focus between your ship and the star you are orbiting.
				text.WriteText ("to change focus between your ship and the star you are orbiting.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
				//Press the Right mouse button.
				text.WriteText ("Press the Right mouse button.", writingSpeed);
				while (!Input.GetMouseButtonDown (1))
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
				text.Clear ();
				//It is working.
				text.WriteText ("It is working.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);

				text.Clear ();
			}



			if (!textoEmocional) {
				//You can use the mouse wheel to zoom in.
				text.WriteText ("You can use the mouse wheel to zoom in.", writingSpeed);
				while (text.writing == true)
					yield return null;
				mainCamera.isOn = true;
				mainCamera.canFocus = false;
				mainCamera.canZoom = true;
				while (Input.GetAxis ("Mouse ScrollWheel") == 0)
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
				mainCamera.canZoom = false;
				mainCamera.GetComponent<Camera> ().orthographicSize = 15;
				//But let's focus on my instructions first.
				text.WriteText ("But let's focus on my instructions first.", writingSpeed);
				while (text.writing == true)
					yield return null;
				text.Clear ();

				while (blackscreen2.color.a < 1) {
					blackscreen2.color += new Color (0, 0, 0, +0.05f);
					yield return null;
				}

				//I am your main directrix.
				text.WriteText ("I am your main directrix.\n\n\nI was developed to guide you.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);

				text.Clear ();
				//Your creator and its team created you and me.
				text.WriteText ("Your creator and its team created you and me.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
	
				text.Clear ();
				//We are the fruit of a lifework.
				text.WriteText ("We are the fruit of a lifework.\n\n\nA lifework dedicated towards the stars.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
	
				text.Clear ();
				//Your only purpose is to travel through our local cluster of stars just to be launched towards Andromeda, our nearest galaxy.
				//Y
				//Now proceed to turning on your guidance assitance.
				text.WriteText ("Your only purpose is to travel through our local cluster of stars just to be launched towards Andromeda, our nearest galaxy.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
	
				text.Clear ();

				text.WriteText ("There you may reveal all the human legacy and culture to anyone who finds you.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
			}
		
		}
				text.Clear ();
				FindObjectOfType<SoundtrackManager> ().PlaySet (2);
				yield return new WaitForSeconds (1);
				while (blackscreen1.color.a > 0) {
					blackscreen1.color += new Color (0, 0, 0, -0.05f);
					yield return null;
				}
		if(!pulaTutorial){
				text.WriteText ("Now proceed to turning on your guidance assistance.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);

				text.Clear ();

				while (blackscreen2.color.a > 0) {
					blackscreen2.color += new Color (0, 0, 0, -0.05f);
					yield return null;
				}
		
				uiInput.canInputAssist = true;
				//Press the {3} number key.
				yield return new WaitForSeconds (1);
				text.WriteText ("Press the {3} number key.", writingSpeed);
				while (!Input.GetKeyDown (KeyCode.Alpha3))
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
				text.Clear ();
				uiInput.canInputAssist = false;


			uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);uiInput.assist.Toggle ();
			yield return new WaitForSeconds (0.5f);

				//You need to get to the arrow keys on the left to visit our neighbouring star system.
				text.WriteText ("You need to get to the arrow on the left to visit our neighbouring star system.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
				//Press the {2} number key.
				yield return new WaitForSeconds (1);
				uiInput.canInputFields = true;
				text.WriteText ("Press the {2} number key.", writingSpeed);
				while (!Input.GetKeyDown (KeyCode.Alpha2))
					yield return null;
				aM.Play ("beepSuccess");
				yield return new WaitForSeconds (waitBetweenInstructions);
				text.Clear ();
				uiInput.canInputFields = false;

			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);			uiInput.fields.Toggle ();
			yield return new WaitForSeconds (0.5f);

				//These are the gravitational fields of the planets around you.
				text.WriteText ("These are the gravitational fields of the planets around you.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
	
				text.Clear ();
				//If you are stranded in space you can always press the left mouse button within these fields
				text.WriteText ("If you are stranded in space you can always press the left mouse button within these fields to orbit its planet.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
			
				text.Clear ();
				//Proceed to align yourself to your nearest objective.
				text.WriteText ("Proceed to align yourself to your nearest objective.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
				//And press the left mouse button to shoot for the stars.
				text.WriteText ("And press the left mouse button to shoot for the stars.", writingSpeed);
				while (text.writing == true)
					yield return null;
				aM.Play ("beepCommon");
				yield return new WaitForSeconds (waitBetweenInstructions);
		
				text.Clear ();
		}
			player.GetComponent<Rigidbody2D>().AddForce (transform.right * (2), ForceMode2D.Impulse);
			player.isOn = true;
			mainCamera.canZoom = true;
			mainCamera.isOn = true;
			mainCamera.canFocus = true;
			uiInput.canInputNames = true;
			uiInput.canInputAssist = true;
			uiInput.canInputFields = true;
			
		yield break;
	}
}