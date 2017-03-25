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
    public int pooledAmount = 100;
    [HideInInspector]
    public List<GameObject> lstBullets = new List<GameObject>();
    public List<GameObject> lstAmmoBuckets = new List<GameObject>();
    public GameObject gun;
    int ammo;

    [Header("Order")]
    public List<string> lstOrdersTitles = new List<string>();
    public List<GameObject> lstOrders = new List<GameObject>();
    public GameObject prefabOrder;
    public Vector2 spawnWait;
    float newOrderWait;
    public bool stop;

    [Header("Score")]
    public Text txtScore;
    int score;
    int maisblasted;
    public Text txtDue;
    int due;

    [Header("Values")]
    public Vector2 randomTime;
    public Vector2 randomAmount;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            lstBullets.Add(obj);
        }
    }

    void Start()
    {
        score = maisblasted = 0;
        StartCoroutine(CreateOrder());
        ammo = gun.GetComponent<GunController>().ammo;
    }

    void Update()
    {
        if (ammo != gun.GetComponent<GunController>().ammo)
            ammo = gun.GetComponent<GunController>().ammo;
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
        for (int i = 0; i < lstOrders.Count; i++) {
            // POSITION
            lstOrders[i].transform.position = new Vector2(i * 160 + 20, 25);
            // DUE DATE
            if (lstOrders[i].GetComponent<Order>().expire) {
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
            } else if (due == lstOrders[0].GetComponent<Order>().amount) {
                score += 1;
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
            } else if (lstOrders.Count == 0) {
                due = 0;
            }
        }
    }

    IEnumerator CreateOrder()
    {
        yield return new WaitForSeconds(3);
        while (!stop) {
            GameObject newOrder = Instantiate(prefabOrder);
            newOrder.transform.parent = GameObject.Find("Canvas").transform;
            float minTime = (lstOrders.Count != 0) ? lstOrders[lstOrders.Count - 1].GetComponent<Order>().timer : 0;
            int i = Random.Range(0, lstOrdersTitles.Count);
            newOrder.GetComponent<Order>().txtDes.text = lstOrdersTitles[i];
            newOrder.GetComponent<Order>().timer = Random.Range(minTime + randomTime.x, randomTime.y);
            newOrder.GetComponent<Order>().amount = Random.Range((int)randomAmount.x, (int)randomAmount.y);
            lstOrders.Insert(0, newOrder);
            yield return new WaitForSeconds(newOrderWait);
        }
    }

    public void AddScore(int value)
    {
        //score += value;
        due += value;
    }

    public void BlastedMais(int value)
    {
        maisblasted += value;
    }

    void UpdateText()
    {
        txtScore.text = "Score " + score;
        txtDue.text = "Due " + due;
    }

    void EndGame()
    {
        if(ammo == 0) {
            PlayerPrefs.SetInt("Meat", score);
            PlayerPrefs.SetInt("Mais", maisblasted);
            gameObject.GetComponent<LevelManager>().NextLevel();
        }
    }

    void CleanUpParticles()
    {
        GameObject[] AllGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < AllGameObjects.Length; i++) {
            if (AllGameObjects[i].name.Contains("Particle")) {
                Destroy(AllGameObjects[i], 2);
            }
        }
    }
}