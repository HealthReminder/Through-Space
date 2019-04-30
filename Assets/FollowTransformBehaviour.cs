using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformBehaviour : MonoBehaviour
{
    public Transform followingTransform;
    public Vector3 offset;

    private void Update() {
        if(followingTransform)
            transform.position = followingTransform.position + offset;
    }
}
