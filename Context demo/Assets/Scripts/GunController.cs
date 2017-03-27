using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    public int ammo;
    public int clipSize;
    [HideInInspector]
    public int clip;
    public Image clipBar;
    float pct;
    //public int damage = 1;
    public Transform gunEnd;
    public GameObject player;
    CameraShake camShake;

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
        camShake = GameObject.FindGameObjectWithTag("GM").GetComponent<CameraShake>();
    }

    void Start()
    {
        clip = clipSize;
        pct = 1.0f / clipSize;
    }

    void Update()
    {
        Shooting();
        Ammunition();
        //Debug.Log("ammo " + ammo + " clip " + clip);
    }

    void Ammunition()
    {
        List<GameObject> lstCow = GameManager.instance.lstCows;
        List<GameObject> lstAmmoBuckets = GameManager.instance.lstAmmoBuckets;
        for (int i = 0; i < lstCow.Count; i++) {
            if (lstCow[i] != null && lstCow[i].GetComponent<CowMovement>().goal != null) {
                if (lstCow[i] != null && Vector3.Distance(lstCow[i].transform.position, lstCow[i].GetComponent<CowMovement>().goal.transform.position) < 2) {
                    ammo -= (int)Time.deltaTime;
                    lstCow[i].GetComponent<CowMovement>().goal.GetComponent<BucketHealth>().AttackBucket();
                }
            }
        }
        if (ammo > 0) {
            if (clip <= 0 || clip < clipSize) {
                Reloading();
            }
        }
    }

    void Shooting()
    {
        if (Input.GetMouseButton(0)) {
            if (clip > 0) {
                List<GameObject> bullets = GameManager.instance.lstBullets;
                for (int i = 0; i < bullets.Count; i++) {
                    if (!bullets[i].activeInHierarchy) {
                        GameManager.instance.BlastedMais(1);
                        //camShake.Shake(0.1f, 0.1f);
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
    }

    void Reloading()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            int r = Random.Range(0, GameManager.instance.lstAmmoBuckets.Count);
            Debug.Log("r " + r);
            Debug.Log("ammo " + ammo + " clip " + clip);
            int newAmmo = clipSize - clip;
            if (newAmmo > ammo) {
                clip += ammo;
                ammo = 0;
                clipBar.fillAmount = (float)clip / clipSize;
                GameManager.instance.AttackBuckets(newAmmo);
            } else {
                clip += newAmmo;
                ammo -= newAmmo;
                clipBar.fillAmount = 1;
                GameManager.instance.AttackBuckets(newAmmo);
            }
        }
    }

    //void HitScan()
    //{
    //    //Cast ray
    //    RaycastHit hit;
    //    Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

    //    if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime) {
    //        //Firerate
    //        nextFireTime = Time.time + fireRate;

    //        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range)) {
    //            //Cow health
    //            EnemyHealth dmgScript = hit.collider.gameObject.GetComponent<EnemyHealth>();
    //            if (dmgScript != null) {
    //                dmgScript.EatMais(damage);
    //            }

    //            //show particles
    //            lineRenderer.SetPosition(0, gunEnd.position);
    //            lineRenderer.SetPosition(1, hit.point);
    //            Instantiate(hitParticles, hit.point, Quaternion.identity);
    //        }
    //        StartCoroutine(ShotEffect());
    //    }
    //}
}