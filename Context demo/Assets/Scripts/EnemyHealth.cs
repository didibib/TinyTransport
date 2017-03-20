using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    public int startingHealth = 3;
    public GameObject hitParticles;
	public GameObject Item;
	public Transform target;

    private int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
    }

    public void Damage(int damage, Vector3 hitPoint)
    {
        Instantiate(hitParticles, hitPoint, Quaternion.identity);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Defeated();
			ItemSpawn ();
        }
    }

    void Defeated()
    {
        GameManager.instance.lstCows.Remove(gameObject);
        GameManager.instance.AddScore(1);
		Destroy (gameObject);
    }

	public void ItemSpawn()
	{
		GameObject clone;
		clone = Instantiate (Item, transform.position, transform.rotation) as GameObject;
	}

	void Update()
	{
		
	}
}