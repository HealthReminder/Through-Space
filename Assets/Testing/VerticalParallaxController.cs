using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class VerticalParallaxController : MonoBehaviour
{
    
    public float maxOffset, layerQtd;
    public AnimationCurve movementCurve;
    [Range(-1,1)]    public float currentPercentage = 0;
    
    [System.Serializable]    public struct ParallaxObject {
        public Image imageComponent;
        public bool changeColor;
        public Gradient color;
        
        [HideInInspector]   public Vector3 initialPosition;
    }

    public ParallaxObject[] objs;

    private void Awake() {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].initialPosition = objs[i].imageComponent.rectTransform.position;
        }
        currentPercentage = -1;
        StartCoroutine(Intro());
    }

    IEnumerator Intro() {
        currentPercentage = -1;
        while(currentPercentage <0.85f){
                MoveParallax(currentPercentage);
                currentPercentage+=movementCurve.Evaluate(currentPercentage)*Time.deltaTime;
                yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
    public void MoveParallax(float percentage) {
        foreach(ParallaxObject obj in objs){
            //Percentage (-1,1)
            obj.imageComponent.rectTransform.position=
            obj.initialPosition - new Vector3(0,
            maxOffset * percentage *obj.initialPosition.z*2,0);

            //Percentage (0,1);
            float otherPercentage  = Mathf.InverseLerp(-1,1,percentage);
            if(obj.imageComponent && obj.changeColor)
                    obj.imageComponent.color = obj.color.Evaluate(otherPercentage);

            
                
        }
    }
}
