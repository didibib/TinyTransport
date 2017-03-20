using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCollecter : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "FPSController") {
            GameObject.FindWithTag("GM").GetComponent<GameManager>().lstMeat.Add(gameObject);
        }
    }
}
