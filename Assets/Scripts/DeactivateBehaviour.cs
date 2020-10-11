using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateBehaviour : MonoBehaviour
{
    public float initial_delay = 2;
    public float duration = 5;
    public GameObject appearance;
    void Start()
    {
        appearance.SetActive(false);
        StartCoroutine(Work());
    }

    IEnumerator Work (){
        yield return new WaitForSeconds(initial_delay);
        appearance.SetActive(true);
        yield return new WaitForSeconds(duration);
        appearance.SetActive(false);
        yield break;
    }
}
