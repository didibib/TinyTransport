using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedCow : MonoBehaviour {

    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        Invoke("Destroy", 3f);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)], 0.7f);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
