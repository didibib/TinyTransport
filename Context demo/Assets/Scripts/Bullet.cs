using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int damage;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Cow") {            
            col.transform.GetComponent<CowHealth>().EatMais(damage);
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
