using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyToPlayer : MonoBehaviour {

    GameObject player;
    [Range(0, 3)]
    public float flyTime;

	void Start () {
        player = GameObject.Find("FPSController");
        StartCoroutine(MoveObject(transform, transform.position, player.transform.position, flyTime));
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while(i < 1.0)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return 0;
        }
    }
}
