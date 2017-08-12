using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int damageMin, damageMax;
    public float bigger;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Cow") {
            Vector3 pos = col.transform.Find("Koe_mesh").GetComponent<SkinnedMeshRenderer>().bounds.center;
            Vector3 newPos = new Vector3(pos.x, pos.y - 0.1f, pos.z);
            int damage = Random.Range(damageMin, damageMax);
            col.transform.GetComponent<CowHealth>().EatMais(damage, newPos);
            col.transform.Find("Koe_mesh").GetComponent<GrowingBigger>().Grow(bigger);
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        Invoke("Destroy", 2f);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
