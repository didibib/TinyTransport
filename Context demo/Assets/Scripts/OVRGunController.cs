using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRGunController : MonoBehaviour
{

    [Header("Gun Settings")]
    public int ammo;
    public int clipSize;
    int clip;
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
        gun.transform.rotation = hand.transform.rotation;
        clip = clipSize;
    }

    void Update()
    {
        handRotation = hand.transform.rotation;
        GunTransform();
        Shooting();
        Ammunition();
        Reloading();
    }

    void GunTransform()
    {
        gun.transform.rotation = handRotation;
        Debug.Log(handRotation.eulerAngles.x);
        if (gun.transform.position != cam.transform.position)
        {
            gun.transform.position = cam.transform.position;
        }
    }

    void Ammunition()
    {
        Debug.Log("ammo : " + ammo + " clip: " + clip);
        List<GameObject> lstCow = GameManager.instance.lstCows;
        for (int i = 0; i < lstCow.Count; i++)
        {
            if (lstCow[i] != null && Vector3.Distance(lstCow[i].transform.position, player.transform.position) < 2)
            {
                ammo -= 1;
            }
        }
    }

    void Shooting()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            if (clip > 0)
            {
                List<GameObject> bullets = GameManager.instance.lstBullets;
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (!bullets[i].activeInHierarchy)
                    {
                        bullets[i].transform.rotation = gunEnd.transform.rotation;
                        bullets[i].transform.position = Random.insideUnitSphere * .2f + gunEnd.transform.position;
                        bullets[i].SetActive(true);
                        bullets[i].GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 500);
                        clip -= 1;
                        break;
                    }
                }
            }
        }

        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            Debug.Log("handtrigger");
        }
    }

    void Reloading()
    {
        if (handRotation.eulerAngles.x > 60 && handRotation.eulerAngles.x < 180)
        {
            Debug.Log("reloading");
            if (ammo > 0)
            {
                if (clip <= 0 || clip < clipSize)
                {
                    int newAmmo = clipSize - clip;
                    if (newAmmo > ammo)
                    {
                        clip += ammo;
                        ammo = 0;
                    }
                    else
                    {
                        clip += newAmmo;
                        ammo -= newAmmo;
                    }
                }
            }            
        }
    }
}
