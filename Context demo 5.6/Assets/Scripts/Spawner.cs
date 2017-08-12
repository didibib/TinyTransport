using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;
    public bool stop;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(waitSpawner());
    }

    void Update()
    {
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
    }

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(startWait);

        while (!stop) {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));

            List<GameObject> cows = GameManager.instance.lstCows;
            for (int i = 0; i < cows.Count; i++) {
                if (!cows[i].activeSelf) {
                    cows[i].transform.position = transform.position + spawnPosition;
                    cows[i].transform.rotation = transform.rotation;
                    cows[i].SetActive(true);
                    break;
                }
            }

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
