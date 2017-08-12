using UnityEngine;
using System.Collections;

public class CowHealth : MonoBehaviour
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
        Debug.Log("hitting cow");
        transform.GetComponent<CowMovement>().AddFood(.1f);
        currentHealth -= damage;        
        if (currentHealth == 0) {
            transform.GetComponent<CowMovement>().defeated = true;
            StartCoroutine(Defeated());
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

    IEnumerator Defeated()
    {
        GameManager.instance.lstCows.Remove(gameObject);        
        Instantiate(deathParticles, transform.position + Vector3.up * 1.5f, transform.rotation);
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
        ItemSpawn();
        //Destroy(gameObject);
    }

    void ItemSpawn()
    {
        GameObject clone;
        clone = Instantiate(item, transform.position + Vector3.up * 1.5f, transform.rotation) as GameObject;
    }
}