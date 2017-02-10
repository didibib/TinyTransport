using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Connections : MonoBehaviour, IPointerClickHandler {

    [HideInInspector]
    public bool menuOpen = false;
    public float timeToCancel = 2;
    private Vector3 mousePosMenu;
    [HideInInspector]
    public bool makingConnection = false;
    public Material materialerial;
    //[HideInInspector]
    //public Lines TheLines;
    [HideInInspector]
    public DropZone.Slot typeOfZone = DropZone.Slot.EMPTY;

    //void Start() {
    //    TheLines = Camera.main.GetComponent<Lines>();
    //}

    void Update() {
        if (Input.GetMouseButtonDown(1) && menuOpen) {
            menuOpen = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right && typeOfZone == DropZone.Slot.LAND) {
            menuOpen = true;
            mousePosMenu = Input.mousePosition;
        }
    }

    void OnGUI() {
        if (menuOpen) {
            if (GUI.Button(new Rect(mousePosMenu.x, mousePosMenu.y * -1 + Screen.height, 120, 30), "Make Connection")) {
                Lines.ToRender.Add(this.transform, new Lines.ConnectionPoint(Vector3.zero, GetComponent<Draggable>()));
                GameManager.gm.connectWithThis = this.transform;
                makingConnection = true;
                Debug.Log("making connection " + makingConnection);
                menuOpen = false;
            }
        }
        if (makingConnection && Input.GetMouseButtonDown(1)) {
            if (GUI.Button(new Rect(mousePosMenu.x, mousePosMenu.y * -1 + Screen.height, 60, 30), "Cancel")) {
                Debug.Log("canceling");
            }
        }
    }
}