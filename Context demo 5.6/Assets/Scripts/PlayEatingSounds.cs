using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEatingSounds : MonoBehaviour
{
    public Vector2 pitchRange;
    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayList();
    }

    void PlayList()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        Debug.Log("clip " + audioSource.clip.name);
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.Play();
        Invoke("PlayList", audioSource.clip.length);
    }
}
