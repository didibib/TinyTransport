using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderHandler : GameManager {

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

    public override void Start() {
        base.Start();
        StartCoroutine(CreateOrder());
        amtOrders = 0;
    }
	
	void Update () {
        if (lstOrders.Count != 0)
            newOrderWait = Random.Range(spawnWait.x, spawnWait.y);
        else
            newOrderWait = 1;

        ManageOrders();
    }

    void ManageOrders()
    {
        for (int i = 0; i < lstOrders.Count; i++) {
            // POSITION
            lstOrders[i].transform.position = new Vector2(i * 180 + 60, 25);
            // DUE DATE
            if (lstOrders[i].GetComponent<Order>().expire) {
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
                amtOrders -= 1;
            } else if (due == lstOrders[0].GetComponent<Order>().amount) {
                meatCollected += 1;
                due = 0;
                Destroy(lstOrders[i]);
                lstOrders.Remove(lstOrders[i]);
                amtOrders -= 1;
            } else if (lstOrders.Count == 0) {
                due = 0;
            }
        }
    }

    IEnumerator CreateOrder()
    {
        yield return new WaitForSeconds(20);
        while (!stop && amtOrders < 4) {
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
}
