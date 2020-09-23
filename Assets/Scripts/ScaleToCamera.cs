using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    public Camera camera;
    private void Update()
    {
        float s = ((camera.orthographicSize)/10) + 0.5f;
        if (s < 0.5f)
            s = 0.5f;
        transform.localScale = new Vector3(s, s, 1);
    }
}
