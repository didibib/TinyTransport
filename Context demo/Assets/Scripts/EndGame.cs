using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    public Text txtMeatCollected;
    int meatCollected;
    public Text txtFood;
    int food;

	void Start () {
        meatCollected = 0;// PlayerPrefs.GetInt("Meat");
        food = 0; // PlayerPrefs.GetInt("Mais");
	}
	
	void Update () {
        if (meatCollected != PlayerPrefs.GetInt("Meat")) {
            meatCollected += 1;
        }
        if (food != meatCollected * 25) {
            food += 25;
        }
        txtMeatCollected.text = "Meat Collected " + meatCollected + " Kg";
        txtFood.text = "Food Blasted " + food + " Kg";
	}
}
