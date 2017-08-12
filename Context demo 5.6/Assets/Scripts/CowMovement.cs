using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowMovement : MonoBehaviour
{
    public float eatingRate;
    public float maisEatTime;

    [Header("Nav Mesh Agent")]
    public bool toPlayer;
    public float speed;
    public float fatspeed;
    UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector]
    public GameObject goal;
    public GameObject freewalkPos;
    Vector3 destination = new Vector3();

    [Header("Finite State Machine")]
    [HideInInspector]
    public CowState state;
    [Range(0.0f, 2.0f)]
    public float idleWaitTime;
    private Animator animator;
    [HideInInspector]
    public float timeEating;
    public GameObject eatParticles;
    private ParticleSystem eatPs;
    public Color maisColor;
    public Color bloodColor;
    [HideInInspector]
    public bool beingfat, defeated, eating = false, eatPlayer = false;

    [Header("Audio")]    
    public List<AudioClip> lstDeathSounds = new List<AudioClip>();
    AudioSource source;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        eatPs = eatParticles.GetComponent<ParticleSystem>();
        if (toPlayer) {
            goal = GameObject.Find("Player");
            agent.destination = goal.transform.position;
        } else {
            goal = GameManager.instance.lstAmmoBuckets[Random.Range(0, GameManager.instance.lstAmmoBuckets.Count)];
            agent.destination = goal.transform.position;
            freewalkPos = GameObject.Find("_FreeWalk");
            destination = freewalkPos.transform.position;
        }

        state = CowState.Idle;
        beingfat = defeated = false;
        timeEating = 0;
        eatParticles.SetActive(false);
    }

    void Update()
    {
        if (!toPlayer) {
            if (goal != null) {
                if (Vector3.Distance(transform.position, goal.transform.position) < 2) {
                    timeEating = goal.GetComponent<BucketHealth>().health;
                    goal.GetComponent<BucketHealth>().EatBucket(eatingRate);
                    agent.speed = 0;
                    agent.transform.LookAt(goal.transform);
                }
            } else {
                if (GameManager.instance.lstAmmoBuckets.Count > 0) {
                    goal = GameManager.instance.lstAmmoBuckets[Random.Range(0, GameManager.instance.lstAmmoBuckets.Count)];
                    agent.destination = goal.transform.position;
                    agent.speed = speed;
                }
            }
        } else {
            if (Vector3.Distance(transform.position, goal.transform.position) < 2f) {
                agent.speed = 0;
                agent.transform.LookAt(goal.transform);
                goal.GetComponent<PlayerHealth>().EatPlayer(eatingRate * .1f);
                timeEating = 1;
                eating = eatPlayer = true;
                var main = eatPs.main;
                main.startColor = new Color(bloodColor.r, bloodColor.g, bloodColor.b);
            }
        }

        HandleStates();

        animator.SetFloat("TimeEating", timeEating);
        animator.SetBool("isFat", beingfat);
        if (defeated) {
            eating = eatPlayer = false;
            StartCoroutine(PlayDeathSound());
            state = CowState.Defeated;
        } else if (timeEating > 0) {
            state = CowState.Eating;
        }

        if (goal == null && !defeated && timeEating <= 0) {
            state = CowState.Walking;
            FreeWalk();
        }
    }

    public void HandleStates()
    {
        eatParticles.SetActive(eating);

        switch (state) {
            case (CowState.Idle):
                StartCoroutine(BeingIdle(idleWaitTime));
                break;
            case (CowState.Walking):
                agent.speed = speed;
                break;
            case (CowState.FatWalking):
                agent.speed = fatspeed;
                break;
            case (CowState.Eating):
                agent.speed = 0;
                EatingFood();
                break;
            case (CowState.Defeated):
                agent.speed = 0;
                animator.SetBool("Defeated", defeated);
                break;
        }
    }

    public void AddFood()
    {
        timeEating += maisEatTime;
        var main = eatPs.main;
        main.startColor = new Color(maisColor.r, maisColor.g, maisColor.b);
        //Debug.Log("eating time " + timeEating);
    }

    void EatingFood()
    {
        if (timeEating <= 0) {
            eating = false;
            timeEating = 0;
            eatParticles.SetActive(false);
            if (!defeated) {
                state = (beingfat) ? CowState.FatWalking : CowState.Walking;
            } else
                state = CowState.Defeated;
        } else {
            eating = true;
            timeEating -= Time.deltaTime * eatingRate;
        }
    }

    IEnumerator BeingIdle(float waitTime)
    {
        agent.speed = 0;
        yield return new WaitForSeconds(waitTime);
        state = CowState.Walking;
    }

    IEnumerator PlayDeathSound()
    {
        while (true) {
            source.clip = lstDeathSounds[Random.Range(0, lstDeathSounds.Count)];
            source.pitch = Random.Range(1.1f, 1.3f);
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    void FreeWalk()
    {
        if (Vector3.Distance(transform.position, destination) < 2) {
            Vector2 randPos = Random.insideUnitCircle * 5;
            Vector3 middle = freewalkPos.transform.position;
            destination = new Vector3(middle.x + randPos.x, middle.y, middle.z + randPos.y);
        }
        agent.destination = destination;
    }

    public enum CowState { Walking, FatWalking, Idle, Eating, Defeated }
}