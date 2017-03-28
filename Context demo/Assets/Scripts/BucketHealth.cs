using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketHealth : MonoBehaviour {

    public float health;
    int ammo;
    public GameObject gun;

	void Start () {
        ammo = gun.GetComponent<OVRGunController>().ammo;
        health = ammo / GameManager.instance.lstAmmoBuckets.Count;
    }
	
	public void AttackBucket()
    {
        health -= Time.deltaTime;
        if (health <= 0) {
            GameManager.instance.lstAmmoBuckets.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void AttackBucket(int value)
    {
        health -= value;
        Debug.Log("bucket health " + health);
        if (health <= 0) {
            GameManager.instance.lstAmmoBuckets.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
