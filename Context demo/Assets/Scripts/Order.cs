using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour {

    public Text txtTimer;
    public Text txtAmount;

    [HideInInspector]
    public bool expire = false;
    [HideInInspector]
    public float timer;
    [HideInInspector]
    public int amount;
    [HideInInspector]
    public Vector2 position;

    void Update()
    {
        DueDate();
        CountDown();
        UpdateText();
    }

    void DueDate()
    {
        if(timer <= 0)
        {
            expire = true;
        }
    }

    void CountDown()
    {
        timer -= Time.deltaTime;
    }

    void UpdateText()
    {
        txtTimer.text = "" + timer;
        txtAmount.text = "" + amount;
    }
}
