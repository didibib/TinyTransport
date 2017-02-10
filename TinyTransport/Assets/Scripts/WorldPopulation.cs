using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldPopulation : MonoBehaviour {

    public Text PopulationCounter;
    public int startPopulation;

    private float timeCounter = 0;
    public int doAfterSeconds = 1;

    public static WorldPopulation wp;

    void Awake() {
        if (wp == null) {
            wp = this;
        }
    }

    void Start() {
        PopulationCounter.text = startPopulation.ToString();
    }

    void Update() {
        timeCounter += Time.deltaTime;

        if (timeCounter > doAfterSeconds) {
            timeCounter = 0;
        }
        PopulationCounter.text = (startPopulation).ToString();
    }
}
