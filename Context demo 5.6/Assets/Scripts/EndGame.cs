using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGame : MonoBehaviour
{
    public Transform start, end;
    public float fillRate;
    public float duration;

    [Header("UI")]
    public Image maisImage;
    public Image meatImage;
    public Image cowImage;
    public GameObject maisText;
    public GameObject meatText;
    public GameObject cowText;

    [Header("Prefabs")]
    public GameObject mais;
    public GameObject meat;
    public GameObject[] cows;

    [Header("Spawners")]
    public Transform maisSpawn;
    public Transform meatSpawn;
    public Transform cowSpawn;

    public int cowsKilled;
    private int maisShot, meatCollected;
    private int biggestNumber;
    int score = 0;
    float fill = 0;

    void Start()
    {
        maisImage.fillAmount = 0;
        meatImage.fillAmount = 0;
        cowImage.fillAmount = 0;

        //cowsKilled = PlayerPrefs.GetInt("Meat");        
        meatCollected = cowsKilled * 10;
        maisShot = cowsKilled * 7;
        biggestNumber = maisShot;

        StartCoroutine(SpawnObject(mais, maisSpawn.position, maisShot, maisSpawn));
        StartCoroutine(SpawnObject(meat, meatSpawn.position, meatCollected, meatSpawn));
        StartCoroutine(SpawnObject(cows, cowSpawn.position, cowsKilled, cowSpawn));

        StartCoroutine(BarGraph(maisImage, maisText, maisShot));
        StartCoroutine(BarGraph(meatImage, meatText, cowsKilled));
        StartCoroutine(BarGraph(cowImage, cowText, cowsKilled));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.LoadLevel(0);
        }
    }

    IEnumerator SpawnObject(GameObject obj, Vector3 pos, int amount, Transform parent)
    {
        for (int i = 0; i < amount; i++) {
            Instantiate(obj, pos, Random.rotation).transform.parent = parent;
            yield return null;
        }
    }

    IEnumerator SpawnObject(GameObject[] obj, Vector3 pos, int amount, Transform parent)
    {
        for (int i = 0; i < amount; i++) {
            Instantiate(obj[Random.Range(0, cows.Length)], pos, Random.rotation).transform.parent = parent;
            yield return null;
        }
    }

    IEnumerator BarGraph(Image img, GameObject txt, float amount)
    {
        float max = amount / biggestNumber;           
        fill = 0;

        for (float timer = 0; timer < duration; timer += Time.deltaTime) {
            float progress = timer / duration;
            score = (int)Mathf.Lerp(0, amount, progress);
            txt.GetComponent<TextMeshPro>().SetText(score.ToString());
            fill = Mathf.Lerp(0, max, progress);
            Debug.Log(txt.name + " max: " + max + " fill: " + fill);
            img.fillAmount = fill;
            yield return null;
        }
        score = (int) amount;
        txt.GetComponent<TextMeshPro>().SetText(score.ToString());
    }

    IEnumerator CountTo (int target, TextMeshPro txt)
    {
        int start = score;
        for (float timer = 0; timer < duration; timer += Time.deltaTime) {
            float progress = timer / duration;
            score = (int)Mathf.Lerp(start, target, progress);
            txt.SetText(score.ToString());
            yield return null;
        }
        score = target;
        txt.SetText(score.ToString());
    }
}

