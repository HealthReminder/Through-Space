using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    Vector3 initialSize;
    [Range(0,1)] public float frequency;
    [Range(0,0.5f)]public float deltaVariation;
    public AnimationCurve curve;
    float currentIndex = 0;
    void Start()
    {
        initialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentIndex+= frequency;
        transform.localScale = new Vector3(initialSize.x*curve.Evaluate(currentIndex)+Random.Range(-deltaVariation,deltaVariation),
        initialSize.y*curve.Evaluate(currentIndex)+Random.Range(-deltaVariation,deltaVariation),
        initialSize.z*curve.Evaluate(currentIndex)+Random.Range(-deltaVariation,deltaVariation));
        if(currentIndex>= 1) 
            currentIndex= 0;
    }
}
