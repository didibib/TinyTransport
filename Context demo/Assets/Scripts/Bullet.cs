using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject eatParticles;
    public int damage;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Cow") {            
            col.transform.GetComponent<EnemyHealth>().EatMais(damage);
            Instantiate(eatParticles, transform.position, transform.rotation);
            gameObject.SetActive(false);
            //Destroy(gameObject);
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
