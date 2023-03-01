
using System;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    public enum BatState { Idle, Follow, Attack, Injured, Death }
    public BatState currentState;
    public Action<int> BatTookDamage;
   

    // Reference to the player's transform
    private Transform playerTransform;
    private Animator anim;
    private GameObject bat;

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

    public float attackRate = 10f;
    public bool isInjured;

    private float lastAttack = 0f;

    // Time it takes for the bat to recover after being injured
    public float recoveryTime = 1f;

    // Timer to keep track of recovery time
    private float recoveryTimer;

    void Start()
    {
        isInjured = false;
        bat = this.gameObject;
        // Find the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
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

        //if the bat collide with the bullet, then the bat will go to the injure state
        
    }

    // Idle state: bat stays in place and does nothing
    void Idle()
    {
       // isInjured = false;
        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if(distance < followDistance)
        {
            currentState = BatState.Follow;
            anim.SetBool("isIdle", true);
        }
    }

    // Follow state: bat follows the player
    void Follow()
    {
      //  isInjured = false;
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
            anim.SetBool("isFollow", true);
        }
    }

    public void TakeDamage(int amount)
    {
        //Pass the "amount" to Invoke
        //CharacterTookDamage? => check if this is "null" => if(CharacterTookDamage!=null)
        BatTookDamage?.Invoke(amount);
        health -= amount;
        currentState = BatState.Injured;
        if (health <= 0)
        {
            Death();
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

        else if (Time.time > lastAttack + attackRate)
        {
            lastAttack = Time.time;
            anim.SetBool("isAttack", true);
            playerTransform.GetComponent<Player>().TakeDamage(damage);
        }


        // Damage the player



    }

    // Injured state: bat is temporarily unable to do anything
    public void Injured()
    {
        // Wait for recovery time to pass
        anim.SetBool("isInjure", true);
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
                isInjured = !isInjured;
            }
        }
    }
    // if the bat got shot by a bullet
 /*     public void OnTriggerEnter(Collider other)
   {
        if (other.gameObject.CompareTag("Bullet"))
        {
            isInjured = true;
            currentState = BatState.Injured;
           // this.TakeDamage(int amout);
            FlowerInventory.instance.DecreaseFlowerCount();
            //Destroy(other);
            Debug.Log("a bat being hit");
        }
    }*/

    // Death state: bat dies and is destroyed
    void Death()
    {
        anim.SetBool("isDie", true);

        Destroy(gameObject);
    }

    // Function to handle taking damage
    
}