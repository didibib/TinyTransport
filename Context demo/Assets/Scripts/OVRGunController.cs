using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRGunController : MonoBehaviour {

    [Header("Gun Settings")]
    public int ammo;
    public int clipSize;
    public float fireRate = .25f;
    public float range = 50;
    public ParticleSystem smokeParticles;
    public ParticleSystem hitParticles;
    public int damage = 1;

    [Header("Gun Transform")]
    public Transform gunEnd;
    public GameObject player;
    public GameObject cam;
    public GameObject gun;
    public GameObject hand;

    private LineRenderer lineRenderer;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    private AudioSource source;
    private float nextFireTime;

    private Quaternion handRotation;
    
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        source = GetComponent<AudioSource>();        
    }

    void Start()
    {
        gun.transform.position = cam.transform.position;
        gun.transform.rotation = cam.transform.rotation;
    }

    void Update()
    {
        Debug.Log(handRotation.eulerAngles.x);
        handRotation = hand.transform.rotation;
        GunTransform();
        Shooting();
        Ammunition();
    }

    void GunTransform()
    {
        gun.transform.rotation = handRotation;
        if(gun.transform.position != cam.transform.position)
        {
            gun.transform.position = cam.transform.position;
        }
    }

    void Ammunition()
    {
        List<GameObject> lstCow = GameObject.FindWithTag("GM").GetComponent<GameManager>().lstCows;
        for (int i = 0; i < lstCow.Count; i++)
        {
            if (lstCow[i] != null && Vector3.Distance(lstCow[i].transform.position, player.transform.position) < 5)
            {
                // Debug.Log("eating ammo");
                // eat ammunition
            }
        }
        if(handRotation.eulerAngles.x > 40)
        {
            Debug.Log("reloading");
        }
    }

    void Shooting()
    {
        //Cast ray
        RaycastHit hit;
        Vector3 rayOrigin = gunEnd.transform.position;

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && Time.time > nextFireTime)
        {
            //Firerate
            nextFireTime = Time.time + fireRate;

            if (Physics.Raycast(rayOrigin, gunEnd.transform.forward, out hit, range))
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
