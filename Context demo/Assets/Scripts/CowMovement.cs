using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowMovement : MonoBehaviour
{
    [Header("Nav Mesh Agent")]
    public float speed;
    public float fatspeed;
    public bool goToPlayer = false;
    UnityEngine.AI.NavMeshAgent agent;
    //public Transform player;
    [HideInInspector]
    public GameObject goal;
    private List<GameObject> lstFood;

    [Header("Finite State Machine")]
    CowState state;
    [Range(0.0f, 2.0f)]
    public float idleWaitTime;
    private Animator animator;
    [HideInInspector]
    public float timeEating;
    public GameObject eatParticles;
    [HideInInspector]
    public bool beingfat, defeated;

    [Header("Sounds")]
    bool bWalkingRoutine, bEatingRoutine, bDeathRoutine = false;
    IEnumerator coWalking, coEating, coDeath;
    AudioSource source;
    public AudioClip eatingSound;
    public List<AudioClip> lstIdleSounds = new List<AudioClip>();
    public List<AudioClip> lstDeathSounds = new List<AudioClip>();

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (!goToPlayer) {
            goal = GameManager.instance.lstAmmoBuckets[Random.Range(0, GameManager.instance.lstAmmoBuckets.Count)];
            agent.destination = goal.transform.position;
        } else {
            goal = GameObject.FindWithTag("Player");
            agent.destination = goal.transform.position;
        }
        state = CowState.Idle;
        beingfat = defeated = false;
        timeEating = 0;
        eatParticles.SetActive(false);

        coWalking = PlayWalkingSound();
        coEating = PlayEatingSound();
        coDeath = PlayDeathSound();

        source.clip = lstIdleSounds[Random.Range(0, lstIdleSounds.Count)];
        source.Play();
    }

    void Update()
    {
        if (!goToPlayer) {
            if (goal != null) {
                if (Vector3.Distance(transform.position, goal.transform.position) < 2) {
                    timeEating = goal.GetComponent<BucketHealth>().health;
                    agent.speed = 0;
                    agent.transform.LookAt(goal.transform);
                }
            } else {
                goal = GameManager.instance.lstAmmoBuckets[Random.Range(0, GameManager.instance.lstAmmoBuckets.Count)];
                agent.destination = goal.transform.position;
                agent.speed = speed;
            }
        }        

        PlayingSounds();
        HandleStates();

        animator.SetFloat("TimeEating", timeEating);
        animator.SetBool("isFat", beingfat);
        if (defeated) {
            state = CowState.Defeated;
        } else if (timeEating > 0)
            state = CowState.Eating;
    }

    public void HandleStates()
    {
        switch (state) {
            case (CowState.Idle):
                StartCoroutine(BeingIdle(idleWaitTime));
                break;
            case (CowState.Walking):
                bEatingRoutine = false;
                agent.speed = speed;
                break;
            case (CowState.FatWalking):
                bEatingRoutine = false;
                agent.speed = fatspeed;
                break;
            case (CowState.Eating):
                bWalkingRoutine = false;
                agent.speed = 0;
                eatParticles.SetActive(true);
                EatingFood();
                break;
            case (CowState.Defeated):
                agent.speed = 0;
                animator.SetBool("Defeated", defeated);
                break;
        }
    }

    public void AddFood(float i)
    {
        timeEating += i;
    }

    void PlayingSounds()
    {
        if (!bWalkingRoutine && (state == CowState.Walking || state == CowState.FatWalking)) {

            StopRoutines();
            StartCoroutine(coWalking);
        } else if (!bEatingRoutine && state == CowState.Eating) {
            StopRoutines();
            StartCoroutine(coEating);
        } else if (!bDeathRoutine && state == CowState.Defeated) {
            StopRoutines();
            StartCoroutine(coDeath);
        }
    }

    void EatingFood()
    {
        if (timeEating <= 0) {
            timeEating = 0;
            eatParticles.SetActive(false);
            if (!defeated) {
                state = (beingfat) ? CowState.FatWalking : CowState.Walking;
            } else
                state = CowState.Defeated;
        } else {
            timeEating -= Time.deltaTime;
        }
    }

    IEnumerator BeingIdle(float waitTime)
    {
        agent.speed = 0;
        yield return new WaitForSeconds(waitTime);
        state = CowState.Walking;
    }

    IEnumerator PlayWalkingSound()
    {
        bWalkingRoutine = true;
        //Debug.Log(bWalkingRoutine);
        while (true) {
            source.clip = lstIdleSounds[Random.Range(0, lstIdleSounds.Count)];
            source.pitch = (beingfat) ? Random.Range(.8f, .9f) : Random.Range(.9f, 1.1f);
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    IEnumerator PlayEatingSound()
    {
        bEatingRoutine = true;
        while (true) {
            source.clip = eatingSound;
            source.pitch = Random.Range(.9f, 1.1f);
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    IEnumerator PlayDeathSound()
    {
        bDeathRoutine = true;
        while (true) {
            source.clip = lstDeathSounds[Random.Range(0, lstDeathSounds.Count)];
            source.pitch = Random.Range(1.1f, 1.3f);
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    void StopRoutines()
    {
        source.Stop();
        StopCoroutine(coWalking);
        StopCoroutine(coEating);
        StopCoroutine(coDeath);
    }

    public enum CowState { Walking, FatWalking, Idle, Eating, Defeated }
}