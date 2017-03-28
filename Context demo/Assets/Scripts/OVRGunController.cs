using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OVRGunController : MonoBehaviour
{

    [Header("Gun Settings")]
    public int ammo;
    public int clipSize;
    [HideInInspector]
    public int clip;
    public Image clipBar;
    public int damage = 1;
    public int force;
    float pct;

    [Header("Gun Transform")]
    public Transform gunEnd;
    public GameObject cam;
    public GameObject gun;

    private LineRenderer lineRenderer;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    //private AudioSource source;
    private float nextFireTime;

    private Quaternion handRotation;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //source = GetComponent<AudioSource>();
    }

    void Start()
    {
        gun.transform.position = cam.transform.position;
        gun.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.GetActiveController());
        clip = clipSize;
        pct = 1.0f / clipSize;
    }

    void Update()
    {
        handRotation = OVRInput.GetLocalControllerRotation(OVRInput.GetActiveController());
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
        List<GameObject> lstCow = GameManager.instance.lstCows;
        List<GameObject> lstAmmoBuckets = GameManager.instance.lstAmmoBuckets;
        for (int i = 0; i < lstCow.Count; i++)
        {
            if (lstCow[i] != null && lstCow[i].GetComponent<CowMovement>().goal != null)
            {
                if (lstCow[i] != null && Vector3.Distance(lstCow[i].transform.position, lstCow[i].GetComponent<CowMovement>().goal.transform.position) < 2)
                {
                    ammo -= (int)Time.deltaTime;
                    lstCow[i].GetComponent<CowMovement>().goal.GetComponent<BucketHealth>().AttackBucket();
                }
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
                        GameManager.instance.BlastedMais(1);
                        bullets[i].transform.rotation = gunEnd.transform.rotation;
                        bullets[i].transform.position = Random.insideUnitSphere * .2f + gunEnd.transform.position;
                        bullets[i].SetActive(true);
                        bullets[i].GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 500);
                        clip -= 1;
                        clipBar.fillAmount -= pct;
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
            int r = Random.Range(0, GameManager.instance.lstAmmoBuckets.Count);
            //Debug.Log("reloading");
            if (ammo > 0)
            {
                if (clip <= 0 || clip < clipSize)
                {
                    int newAmmo = clipSize - clip;
                    if (newAmmo > ammo)
                    {
                        clip += ammo;
                        ammo = 0;
                        clipBar.fillAmount = (float)clip / clipSize;
                        GameManager.instance.AttackBuckets(newAmmo);
                    }
                    else
                    {
                        clip += newAmmo;
                        ammo -= newAmmo;
                        clipBar.fillAmount = 1;
                        GameManager.instance.AttackBuckets(newAmmo);
                    }
                }
            }            
        }
    }
}
