using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AskDemand : MonoBehaviour {

    public Image loadingBar;
    public Color weAreFine, weAreHungry;

    [SerializeField] [Range(0f, 1f)]
    private float currentAmount;
    [SerializeField] [Range(0f, 1f)]
    private float speed;
    [HideInInspector]
    public bool startDistributing;
    private float amountFood = 0;
    private int procesTime = 0;
    private float previousMillis = 0 ;

    void Start() {
        loadingBar.fillAmount = currentAmount;
        loadingBar.color = weAreFine;
        startDistributing = false;
    }

    void Update() {
        if (currentAmount > 0) {
            currentAmount -= speed * Time.deltaTime;
            loadingBar.color = weAreFine;
        } else if (currentAmount <= 0) {
            loadingBar.color = weAreHungry;
        }
        loadingBar.fillAmount = currentAmount;

        if (startDistributing) {            
            float currentMillies = Time.time;
            if(currentMillies - previousMillis >= procesTime) {
                previousMillis = currentMillies;
                currentAmount += amountFood;
            }
        }        
	}

    public void Distribute(bool _startDistributing, float _amountFood, int _procesTime) {
        startDistributing = _startDistributing;
        amountFood = _amountFood;
        procesTime = _procesTime;
        Debug.Log("new demands: " + startDistributing + " " + amountFood + " " + procesTime);
    }
}