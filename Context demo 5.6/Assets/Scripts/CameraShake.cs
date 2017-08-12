using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    [HideInInspector]
    public Camera mainCam;
    public GameObject crosshair;
    public Transform stopPos;
    float shakeAmount = 0;

    void Awake()
    {
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);        
    }

	void DoShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            //mainCam.transform.position = camPos;

            Vector3 chPos = crosshair.transform.position;
            chPos.x += offsetX;
            chPos.y += offsetY;

            crosshair.transform.position = chPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        //mainCam.transform.localPosition = Vector3.zero;
        crosshair.transform.position = stopPos.position;
    }
}
