using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodedCows : MonoBehaviour
{
    public GameObject whiteCow;
    //public GameObject blackCow;
    public int pooledAmtExplCows;
    [HideInInspector]
    public List<Transform> lstSliceParts;
    [HideInInspector]
    public List<GameObject> lstExplCowsWhite;
    [HideInInspector]
    public List<GameObject> lstExplCowsBlack;

    private int explCowIndexWhite;
    private int explCowIndexBlack;
    private int childrenCount;

    void Start()
    {
        lstExplCowsWhite = new List<GameObject>();
        for (int i = 0; i < pooledAmtExplCows; i++) {
            GameObject obj;
            obj = Instantiate(whiteCow);
            obj.SetActive(false);
            lstExplCowsWhite.Add(obj);
        }

        //lstExplCowsBlack = new List<GameObject>();
        //for (int i = 0; i < pooledAmtExplCows; i++) {
        //    GameObject obj;
        //    obj = Instantiate(whiteCow);
        //    obj.SetActive(false);
        //    lstExplCowsBlack.Add(obj);
        //}

        childrenCount = whiteCow.transform.childCount;
        for (int i = 0; i < childrenCount; i++) {
            lstSliceParts.Add(whiteCow.transform.GetChild(i));
        }

        explCowIndexWhite = 0;
        explCowIndexBlack = 0;
    }

    public void InstantiateExplCowWhite(Transform cow)
    {
        if (explCowIndexWhite >= lstExplCowsWhite.Count)
            explCowIndexWhite = 0;

        for (int i = 0; i < childrenCount; i++) {
            lstExplCowsWhite[explCowIndexWhite].transform.GetChild(i).transform.localPosition = lstSliceParts[i].transform.localPosition;
            lstExplCowsWhite[explCowIndexWhite].transform.GetChild(i).transform.localRotation = lstSliceParts[i].transform.localRotation;
        }

        lstExplCowsWhite[explCowIndexWhite].transform.position = cow.position;
        lstExplCowsWhite[explCowIndexWhite].transform.rotation = cow.rotation;
        lstExplCowsWhite[explCowIndexWhite].SetActive(true);

        explCowIndexWhite++;
    }

    public void InstantiateExplCowBlack(Transform cow)
    {
        if (explCowIndexBlack >= lstExplCowsBlack.Count)
            explCowIndexBlack = 0;

        for (int i = 0; i < childrenCount; i++) {
            lstExplCowsBlack[explCowIndexWhite].transform.GetChild(i).transform.localPosition = lstSliceParts[i].transform.localPosition;
            lstExplCowsBlack[explCowIndexWhite].transform.GetChild(i).transform.localRotation = lstSliceParts[i].transform.localRotation;
        }

        for (int i = 0; i < lstExplCowsBlack.Count; i++) {
            if (!lstExplCowsBlack[i].activeSelf) {
                lstExplCowsBlack[i].transform.position = cow.position;
                lstExplCowsBlack[i].SetActive(true);
                break;
            }
        }

        explCowIndexBlack++;
    }
}
