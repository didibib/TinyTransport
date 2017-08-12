using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCollecter : MonoBehaviour {

    public GameObject Meat;
    public int pooledAmtMeat;
    [HideInInspector]
    public List<GameObject> lstMeat;
    public float height;
    public float speed;
    public float length;

    private Vector3 startPos;
    
    void Start()
    {
        lstMeat = new List<GameObject>();
        for (int i = 0; i < pooledAmtMeat; i++) {
            GameObject obj = Instantiate(Meat);
            obj.SetActive(false);
            lstMeat.Add(obj);
        }
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v += (transform.right * Mathf.PingPong(Time.time * speed, length));
        transform.position = v;
    }

    public void InstantiateMeat(Vector3 meatPos)
    {
        for (int i = 0; i < lstMeat.Count; i++) {
            if (!lstMeat[i].activeSelf) {
                meatPos.y = height;
                lstMeat[i].transform.position = meatPos;
                lstMeat[i].SetActive(true);
                break;
            }
        }
    }    
}
