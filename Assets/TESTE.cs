using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTE : MonoBehaviour
{
    public Transform transformm;

    private void Update() {
        if(Input.GetMouseButton(0)){
            AlignToTransform(transformm);
        }
    }
    public void AlignToTransform(Transform t) {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, t.up);
	}
}
