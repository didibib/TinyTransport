using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    void Awake() {
        if (gm == null) {
            gm = this;
        }
    }

    [HideInInspector]
    public Transform connectWithThis;
    [HideInInspector]
    public DropZone droppedOnThis;

    void Update() {
        for (int i = 0; i < Lines.ToRender.Count; i++) {
            //Lines.ToRender.Values.ElementAt(i).AssignedDropZone.peopleDemands
        }
    }
}
