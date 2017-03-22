using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    public int startingHealth = 3;
    public GameObject hitParticles;
    public GameObject deathParticles;
    public GameObject item;
    public Transform target;

    private int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
    }

    public void EatMais(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Health " + currentHealth);
        if (currentHealth <= 0) {
            Defeated();
            ItemSpawn();
        }
    }

    public void Damage(int damage, Vector3 hitPoint)
    {
        Instantiate(hitParticles, hitPoint, Quaternion.identity);
        currentHealth -= damage;
        if (currentHealth == 0) {
            Defeated();            
        }
    }

    void Defeated()
    {
        GameManager.instance.lstCows.Remove(gameObject);
        GameManager.instance.AddScore(1);
        Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(gameObject, .5f);
        ItemSpawn();
    }

    void ItemSpawn()
    {
        GameObject clone;
        clone = Instantiate(item, transform.position, transform.rotation) as GameObject;
    }
}