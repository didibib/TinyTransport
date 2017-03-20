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
    
    public Text txtScore;
    int score;
    public Text txtDue;
    int due;
    public GameObject prefabOrder;

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
    }

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        UpdateText();
        ManageOrders();
        CreateOrder();
    }

    void ManageOrders()
    {
        for (int i = 0; i < lstOrders.Count; i++) {
            // POSITION
            lstOrders[i].transform.position = new Vector2(i * 160 + 10, 25);
            // DUE DATE
            if (lstOrders[i].GetComponent<Order>().expire) {
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
            } else if (due == lstOrders[0].GetComponent<Order>().amount){
                score += 1;
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
            }
        }
    }

    void CreateOrder()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject newOrder = Instantiate(prefabOrder);
            newOrder.transform.parent = GameObject.Find("Canvas").transform;
            float minTime = (lstOrders.Count != 0) ? lstOrders[0].GetComponent<Order>().timer : 0;
            newOrder.GetComponent<Order>().timer = Random.Range(minTime + randomTime.x, randomTime.y);
            newOrder.GetComponent<Order>().amount = Random.Range((int)randomAmount.x, (int)randomAmount.y);
            lstOrders.Add(newOrder);
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
}
