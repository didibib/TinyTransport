using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image bloodScreen;
    public Image blackScreen;
    public float startingHealth;
    private float health;

    private float median;
    private float prc;
    private Color c;

    private bool beingEaten = false;
    private float alphaSlider, scaleSlider;

    public float frequency = 0.0f;
    public float speed;
    private float _frequency,_frequencyInv;
    private float phase = 0.0f;

    void Start()
    {
        blackScreen.gameObject.SetActive(true);
        bloodScreen.gameObject.SetActive(true);

        health = startingHealth;
        bloodScreen.color = new Color(bloodScreen.color.r, bloodScreen.color.g, bloodScreen.color.b, 0);
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);

        c = bloodScreen.color;
        median = bloodScreen.GetComponent<Transform>().localScale.x;
        _frequency = frequency;
    }

    void Update()
    {
        if (frequency != _frequency)
            CalcNewFreq();

        _frequency = health / startingHealth;
        _frequencyInv = 1.0f - _frequency;
        if (beingEaten) {
            alphaSlider = Mathf.PingPong(Time.time * _frequencyInv, _frequencyInv);
            scaleSlider = Mathf.PingPong(Time.time * .1f, .1f) + median;
            c.a = alphaSlider;            
            beingEaten = false;           
        } else {
            if(alphaSlider != 0) {
                c.a -= Time.deltaTime;
                scaleSlider = median;              
            }
        }

        bloodScreen.color = c;
        blackScreen.color = c;
        bloodScreen.GetComponent<Transform>().localScale = new Vector2(scaleSlider, scaleSlider);
    }

    public void EatPlayer(float eatRate)
    {
        beingEaten = true;
        float damage = Time.deltaTime * 100 * eatRate;
        health -= damage;
        if (health <= 0) {
            GameManager.instance.EndGame();
        }
    }

    void CalcNewFreq()
    {
        float curr = (Time.time * _frequency + phase) % (2.0f * Mathf.PI);
        float next = (Time.time * frequency) % (2.0f * Mathf.PI);
        phase = curr - next;
        _frequency = frequency;
    }
}

