using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour
{
    public Transform tR;
    public Rigidbody2D orbitingRb;
    public SpringJoint2D springJoint;
    int direction;
    float rotationVelocity;
    public AnimationCurve heightDecayCurve;
    float life;

   
    Transform parent;

    private void OnEnable() {
        tR.GetComponent<TrailRenderer>().Clear();

        rotationVelocity = Random.Range(0.001f,0.1f);

        direction = Random.Range(0,2);
        if(direction == 0)
            direction = -1;

        life = 0;
        
        if(!parent)
            parent = transform.parent;
        parent.position = orbitingRb.transform.position;

        Vector3 offset = new Vector3(0,0,0);
        int coin;
        coin = Random.Range(0,2);
        if(coin == 0)
            offset += new Vector3(40,0,0);
        else
            offset = new Vector3(-40,0,0);
        coin = Random.Range(0,2);
        if(coin == 0)
            offset += new Vector3(0,40,0);
        else
            offset = new Vector3(0,-40,0);

        transform.localPosition = orbitingRb.transform.position + offset;
    }
    private void Update() {
        transform.parent.Rotate(new Vector3(0,0,rotationVelocity*direction*10),Space.Self);
        transform.position+=heightDecayCurve.Evaluate(life)*(orbitingRb.transform.position-transform.position)/1000;
        tR.position = transform.position;
        life+= Time.deltaTime;
        
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "surface")
            gameObject.SetActive(false);
    }

}
