using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBehaviour : MonoBehaviour
{
    public float start_delay;
    public float lifetime;
    public AnimationCurve size_curve;

    void Start ()
    {
        StartCoroutine(PopUpRoutine());
    }

    IEnumerator PopUpRoutine()
    {
        transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSecondsRealtime(start_delay);

        float progress = 0;
        while(progress <= 1)
        {
            float v = size_curve.Evaluate(progress);
            transform.localScale = new Vector3(v, v, v);
            progress += 0.02f;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(lifetime);

        progress = 1;
        while (progress >= 0)
        {
            float v = size_curve.Evaluate(progress);
            transform.localScale = new Vector3(v, v, v);
            progress -= 0.04f;
            yield return null;
        }

        transform.localScale = new Vector3(0, 0, 0);

        yield break;
    }
}
