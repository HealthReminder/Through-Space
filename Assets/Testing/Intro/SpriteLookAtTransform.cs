using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtTransform : MonoBehaviour
{
    public Transform lookingAt;
    void Update()
    {
        if(lookingAt)
             transform.LookAt(lookingAt.position, Vector3.up);
    }
}
