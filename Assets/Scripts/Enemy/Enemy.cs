using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public static event Action KillPlayer;

    [SerializeField] private Sound[] zombieSteps;

    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float returnToPatrolDistance = 15f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private int currentPatrolIndex = 0;

    private bool isChasingPlayer = false;
    private bool playerInSight = false;

    private Animator anim;

    private void Start()
    {
        InitializaSteps();
        InitializationEnemy();
    }

    // find player, chasing, patrolling and set animation
    private void Update()
    {
        CheckPlayerInSight();
        HandleChasing();
        HandlePatrolling();
        HandleAnimations();
    }

    // initialize zombie
    private void InitializationEnemy()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        GoToNextPos();
    }

    // initialize steps and set audio source from 2d to 3d, and set position for better voice in around world
    private void InitializaSteps()
    {
        foreach(Sound sound in zombieSteps)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.name = sound.name;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            sound.source.spatialBlend = 1.0f;

            sound.source.minDistance = 10f;
            sound.source.maxDistance = 25f;
        }
    }

    // default implemantion of play zombie step
    private void PlayStep()
    {
        Sound step = zombieSteps[UnityEngine.Random.Range(0, zombieSteps.Length)];
        step.source.Play();
    }

    // function for better use it in Update function
    private void CheckPlayerInSight()
    {
        playerInSight = IsPlayerInSight();
    }


    // chasing player or come to first position if isChasing player
    private void HandleChasing()
    {
        if(playerInSight && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasingPlayer = true;
            ChasePlayer();
        }
        else if (isChasingPlayer && Vector3.Distance(transform.position, player.position) > returnToPatrolDistance)
        {
            isChasingPlayer = false;
            GoToStartPosition();
        }
    }

    // if it's not chasing and zombie come to position go to next pos
    private void HandlePatrolling()
    {
        if (!isChasingPlayer && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPos();
        }
    }
    
    // change animation if player is come
    private void HandleAnimations()
    {
        anim.SetBool("isMoving", agent.velocity.magnitude > 0);
        anim.SetBool("isChasing", isChasingPlayer);
    }

    // zombie checking next pos
    private void GoToNextPos()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // chase player
    private void ChasePlayer() => agent.SetDestination(player.position);
    private void GoToStartPosition() => agent.SetDestination(startPosition);
    
    // check if player is nearly 
    private bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, directionToPlayer.normalized, out hit,detectionRadius))
        {
            if(hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    // check if zombie come so far to player, if yes end game
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            KillPlayer?.Invoke();
        }
    }

    // draw red gizmos for better manipulate and set detection range and range for come back to first position
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
