using UnityEngine;
using System.Collections;

public class CowMovement : MonoBehaviour
{
    [Header("Nav Mesh Agent")]
    public float speed;
    UnityEngine.AI.NavMeshAgent agent;
    public Transform goal;

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
            case (CowState.Smelling):
                break;
        }
    }

    IEnumerator BeingIdle(float waitTime)
    {
        agent.speed = 0;
        yield return new WaitForSeconds(waitTime);
        state = CowState.Moving;
    }

    public enum CowState { Moving, Idle, Smelling }
}