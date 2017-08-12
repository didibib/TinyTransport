using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth;
    public int fatHealth;
    public GameObject deathParticles;
    public GameObject item;
    public Transform target;

    private int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
    }

    void Update()
    {
        //Debug.Log("current health " + currentHealth);
    }

    public void EatMais(int damage)
    {
        transform.GetComponent<CowMovement>().AddFood();
        currentHealth -= damage;        
        if (currentHealth <= 0) {
            transform.GetComponent<CowMovement>().defeated = true;
            Defeated();
            ItemSpawn();
        } else if (currentHealth <= fatHealth) {
            transform.GetComponent<CowMovement>().beingfat = true;
        }
    }

    //public void Damage(int damage, Vector3 hitPoint)
    //{
    //    currentHealth -= damage;
    //    if (currentHealth == 0) {
    //        Defeated();            
    //    }
    //}

    void Defeated()
    {
        GameManager.instance.lstCows.Remove(gameObject);
        GameManager.instance.AddScore(1);
        Instantiate(deathParticles, transform.position + Vector3.up * 1.5f, transform.rotation);
        Destroy(gameObject, .5f);
        ItemSpawn();
    }

    void ItemSpawn()
    {
        GameObject clone;
        clone = Instantiate(item, transform.position, transform.rotation) as GameObject;
    }
}