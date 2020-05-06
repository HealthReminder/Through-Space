using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateBehaviour : MonoBehaviour
{
    public float initialDelay = 2;
    void Start()
    {
        StartCoroutine(Work());
    }

    IEnumerator Work (){
        yield return new WaitForSeconds(initialDelay);
        gameObject.SetActive(false);
        yield break;
    }
}
