using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;
using DialogueEditor;

public class MainCharacterAI : Interactable
{
    public Transform player;
    public Transform[] patrolPoints;
    public NavMeshAgent agent;
    public Animator animator;
    public float idleDuration = 10f;

    [Header("Conversation")]
    public NPCConversation firstConversation;
    public NPCConversation secondConversation;
    public AudioSource speakingAudio;

    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private bool followPlayer = false;
    private bool inConversation = false;

    private PlayerInput playerInput;
    private int layerDefault;
    private int layerInteractable;

    protected override void Start()
    {
        base.Start();
        layerDefault = LayerMask.NameToLayer("Default");
        layerInteractable = LayerMask.NameToLayer("Interactable");
        playerInput = FindFirstObjectByType<PlayerInput>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (speakingAudio != null)
            speakingAudio.Stop();

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        if (player == null || patrolPoints.Length == 0) return;

        // Don't patrol while in conversation
        if (inConversation)
        {
            Debug.Log("MC is inConversation with the player");
            // Face the player during conversation
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
        else if (followPlayer)
        {
            Debug.Log("MC is following the player");
            
            // follow the player
            agent.SetDestination(player.position);
        }
        else
        {
            Debug.Log("MC is patrolling");

            // patrol
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
            {
                StartCoroutine(WaitAtPatrolPoint());
            }
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetFloat("MotionSpeed", 1f);
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

    public override void Interact()
    {
        base.Interact();

        Debug.Log("Interacting with the MC");
        if (GameManager.Instance.isFirstTimeMeeting(gameObject))
        {
            StartConversation(firstConversation);
        }
        else
        {
            StartConversation(secondConversation);
        }
    }

    private void StartConversation(NPCConversation conversation)
    {
        Debug.Log("Starting conversation with mc");
        inConversation = true;

        // Stop patrolling
        StopAllCoroutines();
        isWaiting = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Disable player input
        playerInput.enabled = false;

        // Make non-interactable during conversation
        gameObject.layer = layerDefault;

        // Play talking sound
        if (speakingAudio != null)
        {
            speakingAudio.Play();
        }

        ConversationManager.Instance.StartConversation(conversation);
        ConversationManager.OnConversationEnded += ConversationEndedHandler;
    }

    private void ConversationEndedHandler()
    {
        inConversation = false;
        if (speakingAudio != null)
            speakingAudio.Stop();

        // Enable player input
        playerInput.enabled = true;

        // Make interactable again
        gameObject.layer = layerInteractable;

        if (GameManager.Instance.isFirstTimeMeeting(gameObject))
        {
            GameManager.Instance.addToListOfPastConversations(gameObject);
        }

        // Resume patrolling
        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPointIndex].position);

        ConversationManager.OnConversationEnded -= ConversationEndedHandler;
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

