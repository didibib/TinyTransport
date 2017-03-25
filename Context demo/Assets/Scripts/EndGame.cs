using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    public Text txtScore;
    int score;
    public Text txtFood;
    int food;

	void Start () {
        score = 0;// PlayerPrefs.GetInt("Meat");
        food = 0; // PlayerPrefs.GetInt("Mais");
	}
	
	void Update () {
        if(score != PlayerPrefs.GetInt("Meat")) {
            score += 1;
        }
        if (food != PlayerPrefs.GetInt("Mais")) {
            food += 1;
        }
        txtScore.text = "Meat Collected " + score;
        txtFood.text = "Food Blasted " + food;
	}
}
