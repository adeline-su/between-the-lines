using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MainCharacterAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;
    public NavMeshAgent agent;
    public Animator animator;
    public float idleDuration = 10f;

    private int currentPointIndex = 0;
    private bool isWaiting = false;

    private bool followPlayer = false;

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        if (player == null || patrolPoints.Length == 0) return;

        if (followDuringConversation)
        {
            // follow the player
            agent.SetDestination(player.position);
        }
        else
        {
            // patrol
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
            {
                StartCoroutine(WaitAtPatrolPoint());
            }
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetFloat("MotionSpeed", 1f);

        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        string clipName = clipInfo.Length > 0 ? clipInfo[0].clip.name : "None";
        Debug.Log($"Speed: {speed}, CurrentClip: {clipName}, Controller: {animator.runtimeAnimatorController?.name}");

        

    }

    public void OnBeginFollow()
    {
        followPlayer = true;
    }

    public void OnEndFollow()
    {
        followPlayer = false;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // idle at each patrol point
        yield return new WaitForSeconds(idleDuration);

        agent.isStopped = false;

        // pick the next patrol point
        // randomize the order of patrol points
        int newIndex = currentPointIndex;
        while (newIndex == currentPointIndex && patrolPoints.Length > 1)
        {
            newIndex = Random.Range(0, patrolPoints.Length);
        }

        currentPointIndex = newIndex;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
        isWaiting = false;
    }
}

