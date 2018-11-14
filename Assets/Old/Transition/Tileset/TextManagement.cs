using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManagement : MonoBehaviour {

	public string textpath;
	public List<string> textlines = new List<string> ();
	public GameObject dialogue;
	Text dialoguetext;
    public Image fade, fade2;
	public int actualline, max;
    public float timer;
    bool faIn, faOut;
	
	// Use this for initialization
	void Start () {
        fade.color = new Color(1, 1, 1, 0);
        fade2.color = new Color(0, 0, 0, 0);
        faIn = faOut = true;
        dialoguetext = dialogue.GetComponent<Text> ();
		GetCharacterFile(textpath);
        actualline = Random.Range(0, max);
	}

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 4f)
        {
            ChangeText();
        }
        if (timer >= 3 && faIn)
        {
            StartCoroutine(FadeIn());
        }

        if ((Input.anyKey || timer > 6.5f) && faOut)
        {
            StartCoroutine(FadeOut());
        }

        if (fade2.color.a >= 1)
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("NextScene"));
        }
    }

    IEnumerator FadeIn()
    {
        faIn = false;
        while (fade.color.a < 1)
        {
            fade.color += new Color(1, 1, 1, 0.05f);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    IEnumerator FadeOut()
    {
        faOut = false;
        while (fade2.color.a < 1)
        {
            fade2.color += new Color(0, 0, 0, 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }


    public void GetCharacterFile (string textpath) {
		StreamReader sReader = new StreamReader (textpath);
		textlines.Clear ();
		while (!sReader.EndOfStream) {
			string line = sReader.ReadLine ();
			textlines.Add (line);
		}

		sReader.Close ();
	}
	int CheckIfLineExists (int actualline){
		if (actualline >= textlines.Count) {
			actualline = 0;
		} else {
			
		}
		return (actualline);
	}

	public void ChangeText (){
			actualline = CheckIfLineExists (actualline);
			dialoguetext.text = textlines [actualline];
	}

}
