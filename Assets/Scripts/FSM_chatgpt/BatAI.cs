
using UnityEngine;

public class BatAI : MonoBehaviour
{
    public enum BatState { Idle, Follow, Attack, Injured, Death }
    public BatState currentState;

    // Reference to the player's transform
    private Transform playerTransform;

    // Distance at which the bat will start following the player
    public float followDistance = 10f;

    // Distance at which the bat will start attacking the player
    public float attackDistance = 2f;

    // Bat's movement speed
    public float speed = 5f;

    // Health points of the bat
    public int health = 3;

    // Attack damage of the bat
    public int damage = 1;

    // Time it takes for the bat to recover after being injured
    public float recoveryTime = 1f;

    // Timer to keep track of recovery time
    private float recoveryTimer;

    void Start()
    {
        // Find the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // State machine update
        switch (currentState)
        {
            case BatState.Idle:
                Idle();
                break;
            case BatState.Follow:
                Follow();
                break;
            case BatState.Attack:
                Attack();
                break;
            case BatState.Injured:
                Injured();
                break;
            case BatState.Death:
                Death();
                break;
        }
    }

    // Idle state: bat stays in place and does nothing
    void Idle()
    {
        // Do nothing
    }

    // Follow state: bat follows the player
    void Follow()
    {
        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // If player is within attack distance, switch to Attack state
        if (distance <= attackDistance)
        {
            currentState = BatState.Attack;
        }
        // If player is out of follow distance, switch to Idle state
        else if (distance > followDistance)
        {
            currentState = BatState.Idle;
        }
        // Move towards the player
        else
        {
            transform.LookAt(playerTransform);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    // Attack state: bat attacks the player
    void Attack()
    {
        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // If player is out of attack distance, switch to Follow state
        if (distance > attackDistance)
        {
            currentState = BatState.Follow;
        }
        // Damage the player
        else
        {
            //*** playerTransform.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    // Injured state: bat is temporarily unable to do anything
    void Injured()
    {
        // Wait for recovery time to pass
        recoveryTimer -= Time.deltaTime;
        if (recoveryTimer <= 0)
        {
            // If health is zero, switch to Death state, otherwise switch to Follow state
            if (health <= 0)
            {
                currentState = BatState.Death;
            }
            else
            {
                currentState = BatState.Follow;
            }
        }
    }

    // Death state: bat dies and is destroyed
    void Death()
    {
        Destroy(gameObject);
    }

    // Function to handle taking damage
    public void TakeDamage(int damage)
    {
        // Subtract damage from health
        //***  health -= damage;

        // If health is zero or less, switch to In
    }
}