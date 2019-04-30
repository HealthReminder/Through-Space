using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelayBehaviour : MonoBehaviour
{
    public float delay;
    private void Start() {
        StartCoroutine(DestroyAfterDelay(delay));
    }

    IEnumerator DestroyAfterDelay (float waitTime) {

        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
        yield break;
    }
}
