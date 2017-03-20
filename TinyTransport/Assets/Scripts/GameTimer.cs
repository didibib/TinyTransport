using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {

    public float timeCounter = 0;

    public static GameTimer gt;

    void Awake() {
        if(gt == null) {
            gt = this;
        }
    }

    void Update() {
        timeCounter += Time.deltaTime;
    }
}
