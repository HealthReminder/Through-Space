using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffOnStartBehaviour : MonoBehaviour
{
    public float initialDelay;
    public GameObject[] objectsBeingTurnedOff;
    public Rigidbody2D playerRb;
    private void Update() {
        if(initialDelay >= 0)
            initialDelay -= Time.deltaTime;
        else {
            Debug.Log("Turned player off.");
            playerRb.simulated = false;
            foreach (GameObject g in objectsBeingTurnedOff)
                g.SetActive(false);
        }
            
    }
}

