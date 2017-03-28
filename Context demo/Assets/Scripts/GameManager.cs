using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Lists")]
    public List<GameObject> lstMeat = new List<GameObject>();
    public List<GameObject> lstCows = new List<GameObject>();

    [Header("Ammunition")]
    public GameObject bullet;
    public int pooledAmountBullets = 100;
    [HideInInspector]
    public List<GameObject> lstBullets = new List<GameObject>();
    public List<GameObject> lstAmmoBuckets = new List<GameObject>();
    public GameObject gun;
    int ammo, clip;

    [Header("Order")]
    public List<string> lstOrdersTitles = new List<string>();
    public List<GameObject> lstOrders = new List<GameObject>();
    public GameObject prefabOrder;
    public Vector2 spawnWait;
    float newOrderWait;
    public bool stop;
    public int maxOrders;
    int amtOrders;
    int ordersCompleted;

    [Header("Order Values")]
    public Vector2 randomTime;
    public Vector2 randomAmount;

    [Header("Cows")]
    public GameObject cowWhite;
    public GameObject cowBlack;
    public int pooledAmountCows = 100;

    [Header("meatCollected")]
    public Text txtmeatCollected;
    int meatCollected;
    int maisblasted;
    public Text txtDue;
    int due;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        for (int i = 0; i < pooledAmountBullets; i++)
        {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            lstBullets.Add(obj);
        }

        for (int i = 0; i < pooledAmountCows; i++)
        {
            GameObject obj;
            if (i % 2 == 0)
                obj = Instantiate(cowWhite);
            else
                obj = Instantiate(cowBlack);
            obj.SetActive(false);
            lstCows.Add(obj);
        }
    }

    void Start()
    {
        meatCollected = maisblasted = amtOrders = 0;
        StartCoroutine(CreateOrder());
        ammo = gun.GetComponent<OVRGunController>().ammo;
        clip = gun.GetComponent<OVRGunController>().clip;
    }

    void Update()
    {
        if (ammo != gun.GetComponent<OVRGunController>().ammo)
            ammo = gun.GetComponent<OVRGunController>().ammo;
        if (clip != gun.GetComponent<OVRGunController>().clip)
            clip = gun.GetComponent<OVRGunController>().clip;
        if (lstOrders.Count != 0)
            newOrderWait = Random.Range(spawnWait.x, spawnWait.y);
        else
            newOrderWait = 1;
        //CleanUpParticles();
        UpdateText();
        ManageOrders();
        EndGame();
    }

    void ManageOrders()
    {
        for (int i = 0; i < lstOrders.Count; i++)
        {
            // POSITION
            lstOrders[i].transform.position = new Vector2(i * 180 + 60, 25);
            // DUE DATE
            if (lstOrders[i].GetComponent<Order>().expire)
            {
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
                amtOrders -= 1;
            }
            else if (due == lstOrders[0].GetComponent<Order>().amount)
            {
                meatCollected += 1;
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
                amtOrders -= 1;
            }
            else if (lstOrders.Count == 0)
            {
                due = 0;
            }
        }
    }

    IEnumerator CreateOrder()
    {
        yield return new WaitForSeconds(20);
        while (!stop && amtOrders < 4)
        {
            GameObject newOrder = Instantiate(prefabOrder);
            newOrder.transform.parent = GameObject.Find("Canvas Overlay").transform;
            float minTime = (lstOrders.Count != 0) ? lstOrders[lstOrders.Count - 1].GetComponent<Order>().timer : 0;
            int i = Random.Range(0, lstOrdersTitles.Count);
            newOrder.GetComponent<Order>().txtDes.text = lstOrdersTitles[i];
            newOrder.GetComponent<Order>().timer = Random.Range(minTime + randomTime.x, randomTime.y);
            newOrder.GetComponent<Order>().amount = Random.Range((int)randomAmount.x, (int)randomAmount.y);
            lstOrders.Insert(0, newOrder);
            amtOrders += 1;
            yield return new WaitForSeconds(newOrderWait);
        }
    }

    public void AttackBuckets(int value)
    {
        float newValue = value;
        for (int i = 0; i < lstAmmoBuckets.Count; i++)
        {
            BucketHealth bh = lstAmmoBuckets[i].GetComponent<BucketHealth>();
            float health = bh.health;
            if (health - newValue <= 0)
            {
                newValue = (health - newValue) * -1;
                bh.AttackBucket(value);
            }
            else
            {
                bh.AttackBucket(value);
                break;
            }
        }
    }

    public void AddScore(int value)
    {
        meatCollected += value;
        due += value;
    }

    public void BlastedMais(int value)
    {
        maisblasted += value;
    }

    void UpdateText()
    {
        txtmeatCollected.text = "meatCollected " + meatCollected;
        txtDue.text = "Due " + due;
    }

    void EndGame()
    {
        if (ammo <= 0 && clip <= 0)
        {
            PlayerPrefs.SetInt("Meat", meatCollected);
            PlayerPrefs.SetInt("Mais", maisblasted);
            gameObject.GetComponent<LevelManager>().NextLevel();
            Debug.Log("endgame");
        }
    }

    void CleanUpParticles()
    {
        GameObject[] AllGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < AllGameObjects.Length; i++)
        {
            if (AllGameObjects[i].name.Contains("Particle"))
            {
                Destroy(AllGameObjects[i], 2);
            }
        }
    }
}