using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Lines : MonoBehaviour {
    //ToRender[MyTransform] = true
    //public Transform testTrans;    

    static public Dictionary<Transform, ConnectionPoint> ToRender = new Dictionary<Transform, ConnectionPoint>();
    
    [SerializeField]
    public class ConnectionPoint {
        public bool active;
        public bool trackMouse;
        public Vector3 endPoint;
        private Draggable myDraggable;
        public DropZone AssignedDropZone;

        public Draggable MyDraggable
        {
            get
            {
                return myDraggable;
            }
        }

        public ConnectionPoint(Vector3 endPoint, Draggable myDraggable, bool active = true, bool trackMouse = true, DropZone AssignedDropZone = null) {
            this.endPoint = endPoint;
            this.active = active;
            this.trackMouse = trackMouse;
            this.myDraggable = myDraggable;
            this.AssignedDropZone = AssignedDropZone;
        }
    }
    public List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();

    public Material mat;
    private Vector3 mousePos;


    //private Coroutine updater;
    //  if (updater == null)
    //        updater = StartCoroutine(OnPostRenderCoroutine());

    void Start() {
        //ToRender.Add(testTrans, true);
        //ToRender[ToRender.Keys.ElementAt(0)] = !ToRender.ElementAt(0).Value;
    }

    void Update() {
        mousePos = Input.mousePosition;
    }

    IEnumerator OnPostRender() {
        yield return new WaitForEndOfFrame();
        int l = ToRender.Count;
        for (int i = 0; i < l; i++) {
            if (!ToRender.Values.ElementAt(i).active)
                continue;
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            ConnectionPoint cP = ToRender.Values.ElementAt(i);
            Vector3 pos = ToRender.Keys.ElementAt(i).transform.position;
            GL.Vertex(new Vector3(pos.x / Screen.width, pos.y / Screen.height, 0));
            if (cP.trackMouse)
                cP.endPoint = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);
            GL.Vertex(cP.endPoint);
            GL.End();
            GL.PopMatrix();
        }
    }
}