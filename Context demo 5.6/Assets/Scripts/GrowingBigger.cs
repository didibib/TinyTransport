using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBigger : MonoBehaviour {

    SkinnedMeshRenderer rend;
    float steps = 0;

    void Start()
    {
        rend = GetComponent<SkinnedMeshRenderer>();
    }

    public void Grow(float amount)
    {
        steps += amount;
        rend.material.SetFloat("_Amount", steps);
    }
}
