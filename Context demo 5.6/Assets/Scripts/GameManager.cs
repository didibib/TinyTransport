using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private bool ovrFlag;

    [Header("Ammunition")]
    public GameObject bullet;
    public int pooledAmountBullets;
    [HideInInspector]
    public List<GameObject> lstBullets = new List<GameObject>();
    public List<GameObject> lstAmmoBuckets = new List<GameObject>();
    public GameObject gun;
    [HideInInspector]
    public int ammo, clip;

    [Header("Cows")]
    public GameObject cowWhite;
    public GameObject cowBlack;
    public int pooledAmountCows;
    //[HideInInspector]
    public List<GameObject> lstCows = new List<GameObject>();

    [Header("Meat Collected")]
    public Text txtmeatCollected;
    [HideInInspector]
    public int meatCollected;
    int maisblasted;
    public Text txtDue;
    [HideInInspector]
    public int due;

    [Header("Audio")]
    public List<GameObject> lstEatingSources = new List<GameObject>();
    public List<GameObject> lstEatPlayerSources = new List<GameObject>();
    private GameObject cowEatMais, cowEatPlayer;
    private bool eatingMais = false, eatingPlayer = false;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        for (int i = 0; i < pooledAmountBullets; i++) {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            lstBullets.Add(obj);
        }

        for (int i = 0; i < pooledAmountCows; i++) {
            GameObject obj;
            if (i % 2 == 0)
                obj = Instantiate(cowWhite);
            else
                obj = Instantiate(cowBlack);
            obj.SetActive(false);
            lstCows.Add(obj);
        }
    }

    public virtual void Start()
    {
        meatCollected = maisblasted = 0;
        if (gun.GetComponent<OVRGunController>()) {
            ovrFlag = true;
            ammo = gun.GetComponent<OVRGunController>().ammo;
            clip = gun.GetComponent<OVRGunController>().clip;
        } else if (gun.GetComponent<GunController>()) {
            ovrFlag = false;
            ammo = gun.GetComponent<GunController>().ammo;
            clip = gun.GetComponent<GunController>().clip;
        }

        for (int i = 0; i < lstAmmoBuckets.Count; i++) {
            lstAmmoBuckets[i].GetComponent<BucketHealth>().health = ammo / lstAmmoBuckets.Count;
        }

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (obj.name.Contains("Cow Eating")) {
                obj.GetComponent<PlayEatingSounds>().enabled = false;
                lstEatingSources.Add(obj);
            }
            if (obj.name.Contains("Cow Eat Player")) {
                obj.GetComponent<PlayEatingSounds>().enabled = false;
                lstEatPlayerSources.Add(obj);
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        if (ovrFlag) {
            if (ammo != gun.GetComponent<OVRGunController>().ammo)
                ammo = gun.GetComponent<OVRGunController>().ammo;
            if (clip != gun.GetComponent<OVRGunController>().clip)
                clip = gun.GetComponent<OVRGunController>().clip;
        } else {
            if (ammo != gun.GetComponent<GunController>().ammo)
                ammo = gun.GetComponent<GunController>().ammo;
            if (clip != gun.GetComponent<GunController>().clip)
                clip = gun.GetComponent<GunController>().clip;
        }

        UpdateText();
        CheckAmmo();

        CheckCowEating();
        PlayEatingSounds(eatingMais, eatingPlayer);
    }

    public void AmmoBuckets(int value)
    {
        float newValue = value;
        for (int i = 0; i < lstAmmoBuckets.Count; i++) {
            BucketHealth bh = lstAmmoBuckets[i].GetComponent<BucketHealth>();
            float health = bh.health;
            if (health - newValue <= 0) {
                newValue = (health - newValue) * -1;
                bh.UseBucket(value);
            } else {
                bh.UseBucket(value);
                break;
            }
        }
    }

    public void AddScore(int value)
    {
        meatCollected += value;
        due += value;
        //Debug.Log("score: " + meatCollected);
    }

    public void BlastedMais(int value)
    {
        maisblasted += value;
        //Debug.Log("maisblasted: " + maisblasted);
    }

    void UpdateText()
    {
        txtmeatCollected.text = "meatCollected " + meatCollected;
        txtDue.text = "Due " + due;
    }

    void CheckAmmo()
    {
        if (ammo <= 0 && clip <= 0) {
            PlayerPrefs.SetInt("Meat", meatCollected);
            PlayerPrefs.SetInt("Mais", maisblasted);
            Debug.Log("endgame");
        }
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("Meat", meatCollected);
        PlayerPrefs.SetInt("Mais", maisblasted);
        gameObject.GetComponent<LevelManager>().NextLevel();
    }

    void PlayEatingSounds(bool eatingMais, bool eatingPlayer)
    {
        if (eatingMais) {
            for (int i = 0; i < lstEatingSources.Count; i++) {
                lstEatingSources[i].GetComponent<AudioSource>().volume = .25f;
            }
        } else {
            for (int i = 0; i < lstEatingSources.Count; i++) {
                lstEatingSources[i].GetComponent<AudioSource>().volume = 0;
            }
        }

        if (eatingPlayer) {
            for (int i = 0; i < lstEatingSources.Count; i++) {
                lstEatPlayerSources[i].GetComponent<AudioSource>().volume = 0.25f;
            }
        } else {
            for (int i = 0; i < lstEatingSources.Count; i++) {
                lstEatPlayerSources[i].GetComponent<AudioSource>().volume = 0;
            }
        }

    }

    void CheckCowEating()
    {
        if (cowEatMais != null) {
            if (!cowEatMais.GetComponent<CowMovement>().eating)
                eatingMais = false;
            //Debug.Log("eating " + eating);
        }
        if (cowEatPlayer != null) {
            if (!cowEatPlayer.GetComponent<CowMovement>().eatPlayer)
                eatingPlayer = false;
            //Debug.Log("eat player " + eatPlayer);
        }

        if (!eatingMais || !eatingPlayer) {
            for (int i = 0; i < lstCows.Count; i++) {
                if (lstCows[i].activeInHierarchy) {
                    if (lstCows[i].GetComponent<CowMovement>().eatPlayer) {
                        cowEatPlayer = lstCows[i];
                        eatingPlayer = true;
                        //Debug.Log("eat player SET " + cowEatPlayer);
                    }
                    if (lstCows[i].GetComponent<CowMovement>().eating) {
                        cowEatMais = lstCows[i];
                        eatingMais = true;
                        //Debug.Log("eating SET " + eating);
                    }
                }
            }
        }
    }
}