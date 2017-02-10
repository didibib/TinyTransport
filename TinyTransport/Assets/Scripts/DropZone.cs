using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public enum Slot { INVENTORY, LAND, OCEAN, EMPTY }
    public Slot typeOfSlot = Slot.INVENTORY;

    public AskDemand peopleDemands;

    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null) return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            d.placeholderParent = this.transform;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left && typeOfSlot == Slot.LAND) {
            //Debug.Log("you clicked on a LAND");           
            if (GameManager.gm.connectWithThis != null) {
                Lines.ConnectionPoint cP = Lines.ToRender[GameManager.gm.connectWithThis];
                if (cP.active) {
                    //Debug.Log("this object is active and you click on a LAND");
                    if (GameManager.gm.droppedOnThis != this) {
                        //GameManager.gm.connectWithThis.GetComponent<Draggable>().changeZone = false;
                        cP.AssignedDropZone = this;
                        Vector3 p = cP.AssignedDropZone.GetComponent<Transform>().position;
                        cP.endPoint = new Vector3(p.x / Screen.width, p.y / Screen.height);
                        cP.trackMouse = false;
                        Debug.Log("connection made");
                    }
                }
            }
            //ConnectionPoint cP = Lines.ToRender[];
            //cP.trackMouse = false;
            //cP.
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        //Debug.Log("OnPointerExit");
        if (eventData.pointerDrag == null) return;
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform) {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name + eventData.position);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            if (typeOfSlot == Slot.LAND) {
                d.parentToReturnTo = this.transform;
                d.positionToReturnTo = eventData.position;
                GameManager.gm.droppedOnThis = this;
                Debug.Log("dropped on " + GameManager.gm.droppedOnThis);
            }            
            else if (typeOfSlot == Slot.INVENTORY)
                d.parentToReturnTo = this.transform;
            else if (typeOfSlot == Slot.OCEAN)
                eventData.pointerDrag.transform.position = d.positionToReturnTo;
        }
    }
}