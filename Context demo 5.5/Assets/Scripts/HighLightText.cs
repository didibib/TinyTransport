using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighLightText : MonoBehaviour {

    public float speed;
    Color initColor;
    Text text;

	void Start () {
        text = GetComponent<Text>();
        initColor = text.color;
	}
	
	void Update () {
        float s = speed * Time.time;
        float size = Mathf.Sin(s) * 75;
        text.color = new Color(initColor.r, initColor.g, initColor.b, Mathf.PingPong(Time.time * speed, 1.0f));

        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetMouseButtonDown(0)) {
            gameObject.GetComponent<LevelManager>().NextLevel();
        }
    }
}
