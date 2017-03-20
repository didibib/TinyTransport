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
            Lines.ConnectionPoint cP = Lines.ToRender.Values.ElementAt(i);
            if (!Lines.ToRender.Values.ElementAt(i).trackMouse && !cP.AssignedDropZone.peopleDemands.startDistributing) {
                Debug.Log(cP.AssignedDropZone.peopleDemands.name);
                float af = cP.myDraggable.amountFood;
                int pt = cP.myDraggable.processTime;
                cP.AssignedDropZone.peopleDemands.Distribute(true, af, pt);
            }
        }
    }
}
