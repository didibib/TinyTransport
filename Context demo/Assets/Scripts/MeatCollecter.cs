using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCollecter : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player") {
            Debug.Log("added meat");
            GameManager.instance.AddScore(1);
            GameManager.instance.lstMeat.Add(gameObject);
        }
    }
}
