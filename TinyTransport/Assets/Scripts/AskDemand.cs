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
    private float amountFood;

    void Start() {
        loadingBar.fillAmount = currentAmount;
        loadingBar.color = weAreFine;
    }

    void Update() {
        if (currentAmount > 0) {
            currentAmount -= speed * Time.deltaTime;
            loadingBar.color = weAreFine;
        } else if (currentAmount <= 0) {
            loadingBar.color = weAreHungry;
        }
        loadingBar.fillAmount = currentAmount;

        if (Input.GetMouseButtonDown(0)) {
            Distribute(0.1f);
        }
	}

    void Distribute(float _amountFood) {
        currentAmount += _amountFood;
    }
}
