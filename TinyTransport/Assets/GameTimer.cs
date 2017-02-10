using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {

    private float timeCounter = 0;
    public int doAfterSeconds = 1;

    public static GameTimer gt;

    void Awake() {
        if(gt == null) {
            gt = this;
        }
    }

    void Update() {
        timeCounter += Time.deltaTime;

        if(timeCounter > doAfterSeconds) {
            timeCounter = 0;
        }
    }
}
