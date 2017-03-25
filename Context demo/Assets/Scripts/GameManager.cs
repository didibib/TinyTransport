using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<GameObject> lstMeat = new List<GameObject>();
    public List<GameObject> lstCows = new List<GameObject>();
    public List<GameObject> lstOrders = new List<GameObject>();
    public GameObject bullet;
    public int pooledAmount = 100;
    public List<GameObject> lstBullets = new List<GameObject>();

    [Header("Order")]
    public List<string> lstOrdersTitles = new List<string>();
    public GameObject prefabOrder;
    public Vector2 spawnWait;
    float newOrderWait;
    public bool stop;

    [Header("Score")]
    public Text txtScore;
    int score;
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
        //DontDestroyOnLoad(gameObject);

        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            lstBullets.Add(obj);
        }
    }

    void Start()
    {
        score = 0;
        StartCoroutine(CreateOrder());
    }

    void Update()
    {
        newOrderWait = Random.Range(spawnWait.x, spawnWait.y);
        //CleanUpParticles();
        UpdateText();
        ManageOrders();        
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
        yield return new WaitForSeconds(5);
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

    void UpdateText()
    {
        txtScore.text = "Cows Killed " + score;
        txtDue.text = "Due " + due;
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