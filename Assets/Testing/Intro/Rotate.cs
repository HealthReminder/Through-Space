using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotate : MonoBehaviour
{
    public Vector3 direction;
    private void Start() {
        for (int a = 0; a < 36; a++)
        {
            
        }
        transform.Rotate(direction*10);
    }
    void Update()
    {
       
            transform.Rotate(direction);
    }
}
