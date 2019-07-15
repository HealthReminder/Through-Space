using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiLookAtBehaviour : MonoBehaviour
{
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        //Vector3 lookPos = target.position - transform.position;
        //lookPos.z = 0;
        //lookPos.y = 0;
        //Quaternion rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
         Vector3 targetPostition = new Vector3( target.position.x, 
                                        this.transform.position.y, 
                                        target.position.z ) ;
        this.transform.LookAt( targetPostition ) ;
    }
}
