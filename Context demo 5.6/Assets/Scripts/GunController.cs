using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Oculus;

public class GunController : MonoBehaviour
{
    List<GameObject> bullets;
    public int ammo;
    public int clipSize;
    [HideInInspector]
    public int clip;
    public Image clipBar;
    public Image clipBarBg;

    public int force;
    private float pct;
    public float fireRate;
    private float nextFire;
    public int spreadMaxAmt;
    private int spreadAmt;
    public float spreadRate;
    private float nextSpread;
    private float scale = 1; 

    public Transform gunEnd;
    CameraShake camShake;
    
    private AudioSource source;
    public AudioClip gunsound;
    private float nextFireTime;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        camShake = GameObject.FindWithTag("GM").GetComponent<CameraShake>();
    }

    void Start()
    {
        bullets = GameManager.instance.lstBullets;
        clip = clipSize;
        pct = 1.0f / clipSize;
    }

    void Update()
    {
        Shooting();
        Ammunition();
    }

    void Ammunition()
    {
        ammo = GameManager.instance.ammo;
        if (ammo > 0) {
            if (clip <= 0 || clip < clipSize) {
                Reloading();
            }
        }
    }

    void Shooting()
    {
        if (Input.GetMouseButton(1)) {
            if (clip > 0) {
                if (Time.time > nextSpread && spreadAmt < spreadMaxAmt) {
                    nextSpread = Time.time + spreadRate;
                    camShake.Shake(.01f, .1f);
                    spreadAmt += 1;
                    scale += 0.1f;
                    Vector3 v = new Vector3(scale, scale, scale);
                    clipBarBg.transform.localScale = v;
                    //Debug.Log(spreadAmt);
                }
                if (Input.GetMouseButtonDown(0)) {
                    // shotgun sound
                    for (int i = 0; i < spreadAmt; i++) {
                        for (int j = 0; j < bullets.Count; j++) {
                            if (!bullets[i].activeInHierarchy) {
                                SpreadShotSound();
                                GameManager.instance.BlastedMais(1);
                                PullTrigger(bullets[i]);
                                clip -= 1;
                                clipBar.fillAmount -= pct;
                                break;
                            }
                        }
                    }
                    spreadAmt = 0;
                    scale = 1;
                    clipBarBg.transform.localScale = Vector3.one;
                }
            }
        } else if (Input.GetMouseButton(0)) {
            if (clip > 0) {
                if (Time.time > nextFire) {
                    nextFire = Time.time + fireRate;
                    for (int i = 0; i < bullets.Count; i++) {
                        if (!bullets[i].activeInHierarchy) {
                            GunSound();
                            camShake.Shake(.01f, .1f);
                            GameManager.instance.BlastedMais(1);
                            PullTrigger(bullets[i]);
                            clip -= 1;
                            clipBar.fillAmount -= pct;
                            break;
                        }
                    }
                }
            } else {
                // empty clip sound
            }
        }
    }

    void PullTrigger(GameObject bullet)
    {
        bullet.transform.rotation = gunEnd.transform.rotation;
        bullet.transform.position = Random.insideUnitSphere * .2f + gunEnd.transform.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force);
        bullet.transform.Rotate(90, 0, 0);
    }

    void Reloading()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            int r = Random.Range(0, GameManager.instance.lstAmmoBuckets.Count);
            int newAmmo = clipSize - clip;
            if (newAmmo > ammo) {
                clip += ammo;
                ammo = 0;
                clipBar.fillAmount = (float)clip / clipSize;
                GameManager.instance.AmmoBuckets(newAmmo);
            } else {
                clip += newAmmo;
                ammo -= newAmmo;
                clipBar.fillAmount = 1;
                GameManager.instance.AmmoBuckets(newAmmo);
            }
        }
    }

    void GunSound()
    {
        //source.clip = gunsound;
        source.pitch = Random.Range(0.8f, 1f);
        source.PlayOneShot(gunsound, .2f);
    }

    void SpreadShotSound()
    {
        source.pitch = Random.Range(.7f, .9f);
        Debug.Log("pitch " + source.pitch);
        source.PlayOneShot(gunsound, .2f);
    }
}