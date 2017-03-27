using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowStatic : MonoBehaviour {

    public bool walker;
    public GameObject goal;
    public List<AudioClip> lstSounds = new List<AudioClip>();

    Animator animator;
    AudioSource source;
    float timeEating = 1;
    bool bRoutine;
    UnityEngine.AI.NavMeshAgent agent;

    void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start () {
        if (walker) {
            agent.destination = goal.transform.position;
            agent.speed = 2;
            timeEating = -1;
        } else {
            timeEating = 1;
        }
    }
	
	void Update () {
        animator.SetFloat("TimeEating", timeEating);
        if (!bRoutine) {
            StartCoroutine(PlaySounds());
        }

        if(Vector3.Distance(transform.position, goal.transform.position) < 2) {
            timeEating = 1;
            agent.speed = 0;
            agent.transform.LookAt(goal.transform);
        }
	}

    IEnumerator PlaySounds()
    {
        bRoutine = true;
        //Debug.Log(bWalkingRoutine);
        while (true) {
            source.clip = lstSounds[Random.Range(0, lstSounds.Count)];
            source.pitch = Random.Range(.9f, 1.1f);
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }
}
