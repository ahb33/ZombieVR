using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    // YET TO BAKE NAVMESH FOR ALL THIS TO EXECUTE
    public enum ZombieState { Idle, Walk, Run, Attacking, Dead }
    // Start is called before the first frame update

    public float health = 100f; // health of zombie
    public float walkSpeed = 2f; // walking speed of zombie
    public float runSpeed = 10f; // running speed of zombie
    public float detectionRange = 10f; // range in which player will be detected
    public float attackRange = 2f; // specifies range which zombie can attack in
    public Vector3 currentWanderPosition;

    private bool isWithinAttackRange = false;

    //private Vector3 lastKnownLocation;

    bool CanSeePlayer;

    // Components & References
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    [SerializeField]
    private Transform player;
    private ZombieState currentState = ZombieState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>(); // get reference to zombie animator component
        animator.SetBool("isDead", false);
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // get reference to store player in
    }

    // Update is called once per frame
    void Update()
    {
        // If zombie cannot see player wander around the world
        // If zombie can see the player attack
        // Player could be in detection range but still invisible to the zombie
        // first check if in detection range then check if zombie can see player to attack- the check needs to be every frame

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(health > 0 )
        {
            if(!CanSeePlayer)
            {
                // Check if player is visible
                CheckPlayerVisibility(distanceToPlayer);

            }

            // Handle zombie actions based on whether it can see the player
            if (CanSeePlayer)
            {
                Debug.Log("Enemy can see player");
                Attack(distanceToPlayer);
            }
            else
            {
                Wander();
            }

        }
        UpdateAgentSpeed();

        // Set animator parameters
        animator.SetBool("canSeePlayer", CanSeePlayer);
        bool inAttackRange = distanceToPlayer <= attackRange;
        animator.SetBool("inAttackRange", inAttackRange);

        // Update animations based on the state
        UpdateAnimation();
    }


    void CheckPlayerVisibility(float distanceToPlayer)
    {
        // Check if player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            // If in range, check line of sight
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            Debug.DrawLine(transform.position + Vector3.up, transform.forward * detectionRange, Color.red);

            if (Physics.Raycast(transform.position + Vector3.up, transform.forward * detectionRange, out hit, detectionRange))
            {
                // If line of sight is clear
                if (hit.collider.CompareTag("Player"))
                {
                    CanSeePlayer = true;
                    Debug.Log("Enemy is seeing the player");

                    return;  // Exit the method early since we found the player
                }
            }
        }

        // If player is out of range or there's no line of sight
        CanSeePlayer = false;
        Debug.Log("Enemy is checking player visibility");
    }

    void UpdateAgentSpeed()
    {
        switch (currentState)
        {
            case ZombieState.Walk:
                navMeshAgent.speed = walkSpeed;
                break;
            case ZombieState.Run:
                navMeshAgent.speed = runSpeed;
                break;
            case ZombieState.Attacking:
            case ZombieState.Idle:
            case ZombieState.Dead:
                navMeshAgent.speed = 0;
                break;
        }
    }


    void UpdateAnimation()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetBool("canSeePlayer", false);
                animator.SetBool("inAttackRange", false);
                break;
            case ZombieState.Walk:
                animator.SetBool("canSeePlayer", false);
                animator.SetBool("inAttackRange", false);
                break;
            case ZombieState.Run:
                animator.SetBool("canSeePlayer", true);
                animator.SetBool("inAttackRange", false);
                break;
            case ZombieState.Attacking:
                animator.SetBool("canSeePlayer", true);
                animator.SetBool("inAttackRange", true);
                break;
            case ZombieState.Dead:
                animator.SetBool("isDead", true);
                break;
        }
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
        Debug.Log("Enemy is taking damage");
    }

    void Die()
    {
        currentState = ZombieState.Dead;
        navMeshAgent.Stop();
        Debug.Log("Enemy is dead");
        Destroy(this.gameObject, 5);
    }

    void Attack(float distanceToPlayer)
    {
        // Attack function should make zombie run towards player and execute attack animation if within attackRange

        isWithinAttackRange = distanceToPlayer <= attackRange;

        if (isWithinAttackRange)
        {
            currentState = ZombieState.Attacking;
            navMeshAgent.SetDestination(transform.position);  // Stop moving
        }
        else
        {
            currentState = ZombieState.Run;
            navMeshAgent.SetDestination(player.position);  // Move towards the player
        }

        animator.SetBool("inAttackRange", isWithinAttackRange);
        Debug.Log("Enemy is attacking");
    }

    void Wander()
    {
        currentState = ZombieState.Walk;  // Set state to Walk when wandering
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Vector3 point;
            if (randomizeMovement(transform.position, 3, out point))
            {
                navMeshAgent.SetDestination(point);
            }
        }
        Debug.Log("Enemy is wandering");
    }

    bool randomizeMovement(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;

    }

    public void HeardGunshot(Vector3 gunshotLocation)
    {
        navMeshAgent.Stop();

        CanSeePlayer = true; // Pretend like the zombie saw the player.
        Debug.Log("Enemy heard gunshot");
    }

}
