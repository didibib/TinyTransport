using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketHealth : MonoBehaviour {

    [HideInInspector]
    public int health;
    int ammo;

	void Start () {

    }
	
	public void EatBucket(float rate)
    {
        rate *= 100;
        int damage = (int)(Time.deltaTime * rate);
        health -= damage;
        GameManager.instance.ammo -= damage;
        if (health <= 0) {
            GameManager.instance.lstAmmoBuckets.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void UseBucket(int value)
    {
        health -= value;
        Debug.Log("bucket health " + health);
        if (health <= 0) {
            GameManager.instance.lstAmmoBuckets.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
