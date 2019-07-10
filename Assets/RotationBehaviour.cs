using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    public float delay = 0;
    public Vector3 rotationVector;
    
    private void Awake() {
        StartCoroutine(Rotation());
    }

    IEnumerator Rotation() {
        while(true){
            transform.Rotate(rotationVector);
            yield return new WaitForSeconds(delay);
        }
        yield break;
    }
}
