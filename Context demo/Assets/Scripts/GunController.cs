using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    public int ammo;
    public int clipSize;
	public float fireRate = .25f;
	public float range = 50;
	public ParticleSystem smokeParticles;
	public ParticleSystem hitParticles;
	public int damage = 1;
	public Transform gunEnd;
    public GameObject player;

	private Camera fpsCam;
	private LineRenderer lineRenderer;
	private WaitForSeconds shotLength = new WaitForSeconds(.07f);
	private AudioSource source;
	private float nextFireTime;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		source = GetComponent<AudioSource>();
		fpsCam = GetComponentInParent<Camera>();
	}

	void Update()
	{
        Shooting();
        Ammunition();
	}

    void Ammunition()
    {
        List<GameObject> lstCow = GameObject.FindWithTag("GM").GetComponent<GameManager>().lstCows;
        for(int i = 0; i < lstCow.Count; i++)
        {
            if (lstCow[i] != null && Vector3.Distance(lstCow[i].transform.position, player.transform.position) < 5)
            {
                // Debug.Log("eating ammo");
                // eat ammunition
            }
        }
    }

    void Shooting()
    {
        //Cast ray
        RaycastHit hit;
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
            //Firerate
            nextFireTime = Time.time + fireRate;

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range))
            {
                //Cow health
                EnemyHealth dmgScript = hit.collider.gameObject.GetComponent<EnemyHealth>();
                if (dmgScript != null)
                {
                    dmgScript.Damage(damage, hit.point);
                }

                //show particles
                lineRenderer.SetPosition(0, gunEnd.position);
                lineRenderer.SetPosition(1, hit.point);
                Instantiate(hitParticles, hit.point, Quaternion.identity);
            }
            StartCoroutine(ShotEffect());
        }
    }

	private IEnumerator ShotEffect()
	{
		lineRenderer.enabled = true;
		source.Play();
		smokeParticles.Play();
		yield return shotLength;
		lineRenderer.enabled = false;
	}
}