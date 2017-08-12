using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncherPool : MonoBehaviour {

    public GameObject target;    
    public GameObject decalLauncher;
    public int pooledAmtLaunchers;
    [HideInInspector]
    public List<GameObject> lstLaunchers;

    private Vector3 targetPos;
    private int launcherIndex;
    
    void Start () {
        lstLaunchers = new List<GameObject>();
        for (int i = 0; i < pooledAmtLaunchers; i++) {
            GameObject obj = Instantiate(decalLauncher);
            obj.SetActive(false);
            lstLaunchers.Add(obj);
        }
        launcherIndex = 0;
    }

    void Update()
    {
        targetPos = target.transform.position;
    }

    public void BloodStream(Vector3 streamPos)
    {
        if (launcherIndex >= pooledAmtLaunchers)
            launcherIndex = 0;

        Vector3 relativePos = targetPos - streamPos;
        Vector3 streamRot = Quaternion.LookRotation(relativePos).eulerAngles;

        lstLaunchers[launcherIndex].transform.position = streamPos;
        streamRot.x -= 45;
        lstLaunchers[launcherIndex].transform.rotation = Quaternion.Euler(streamRot);
        lstLaunchers[launcherIndex].SetActive(true);
        lstLaunchers[launcherIndex].GetComponent<ParticleSystem>().Play();

        launcherIndex++;
    }
}
