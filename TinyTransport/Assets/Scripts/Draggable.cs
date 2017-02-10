using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Draggable : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [HideInInspector]
    public Transform parentToReturnTo = null;
    [HideInInspector]
    public Vector2 positionToReturnTo;
    [HideInInspector]
    public Transform placeholderParent = null;
    GameObject placeholder = null;

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
    public DropZone.Slot typeOfZone = DropZone.Slot.LAND;
    [HideInInspector]
    public bool changeZone = true;

    void Update() {
        if (Input.GetMouseButtonDown(1) && menuOpen) {
            menuOpen = false;
        }
        if(changeZone == false)
        Debug.Log("change zone " + changeZone);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right && typeOfZone == DropZone.Slot.LAND) {
            menuOpen = true;
            mousePosMenu = Input.mousePosition;
            Debug.Log("mousepos menu " + mousePosMenu);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left && !menuOpen) {
            //Debug.Log("OnBeginDrag");

            placeholder = new GameObject();
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.name = "placeholder";
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;
            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            parentToReturnTo = this.transform.parent;
            placeholderParent = parentToReturnTo;
            this.transform.SetParent(this.transform.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            //DropZone[] zones = GameObject.FindObjectOfType<DropZone>(); // e.g. to make items glow 
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left && !menuOpen) {
            Vector3 p = eventData.position;
            this.transform.position = new Vector3(p.x, p.y, 0);
            if (placeholder.transform.parent != placeholderParent) {
                placeholder.transform.SetParent(placeholderParent);
            }

            int newSiblingsIndex = placeholderParent.childCount;

            for (int i = 0; i < placeholderParent.childCount; i++) {
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x) {

                    newSiblingsIndex = i;

                    if (placeholder.transform.GetSiblingIndex() < newSiblingsIndex) {
                        newSiblingsIndex--;
                    }
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSiblingsIndex);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left && !menuOpen) {
            //Debug.Log("OnEndDrag");
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(placeholder);
            //EventSystem.current.RaycastAll(eventData);
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
            if (GUI.Button(new Rect(mousePosMenu.x - Screen.width / 2, mousePosMenu.y * -1 + Screen.height / 2, 60, 30), "Cancel")) {
                Debug.Log("canceling");
            }
        }
    }
}
