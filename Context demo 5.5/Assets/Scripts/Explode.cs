using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour
{

	public GameObject explosionParticles;
	public float blastRadius = 1;

	private bool explode;

	void OnCollisionEnter()
	{

		explosionParticles.SetActive(true);
		explosionParticles.transform.SetParent(null);
		explode = true;
	}

	void FixedUpdate()
	{
		if (explode)
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if (hitColliders[i].GetComponent<Rigidbody>() != null)
				{
					hitColliders[i].GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, blastRadius);
				}
			}
			Instantiate(explosionParticles, transform.position, Quaternion.identity);
			this.gameObject.SetActive(false);
		}
	}

}