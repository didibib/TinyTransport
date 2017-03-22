using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowMovement : MonoBehaviour
{
    [Header("Nav Mesh Agent")]
    public float speed;
    UnityEngine.AI.NavMeshAgent agent;
    public Transform goal;
    private List<GameObject> lstFood;

    [Header("Finite State Machine")]
    CowState state;
    [Range(0.0f, 2.0f)]
    public float idleWaitTime;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        state = CowState.Idle;
    }

    void Update()
    {
        HandleState();
    }

    public void HandleState()
    {
        switch (state)
        {
            case (CowState.Idle):
                StartCoroutine(BeingIdle(idleWaitTime));
                break;
            case (CowState.Moving):
                agent.speed = speed;
                break;
            case (CowState.Eating):
                CheckForFood();
                break;
            case (CowState.Smelling):
                break;
        }
    }

    void CheckForFood()
    {
        lstFood = GameManager.instance.lstBullets;
        for (int i = 0; i < lstFood.Count; i++) {
            if(Vector3.Distance(lstFood[i].transform.position, transform.position) < 5) {

            }
        }
    }

    IEnumerator BeingIdle(float waitTime)
    {
        agent.speed = 0;
        yield return new WaitForSeconds(waitTime);
        state = CowState.Moving;
    }

    public enum CowState { Moving, Idle, Eating, Smelling }
}