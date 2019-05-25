using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevSystemOSTPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundtrackManager.instance.ChangeSet("Calm");
    }
}
