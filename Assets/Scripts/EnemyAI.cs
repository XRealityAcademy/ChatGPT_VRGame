
using System;
using UnityEngine;

namespace ZombieFlower
{
    public class EnemyAI : MonoBehaviour
    {
        //Idle=0, Follow=1, Attack =2,
        public enum EnemyState { Idle, Follow, Attack, Injured, Death }
        public EnemyState currentState;
        public Action<int> EnemyTookDamage;


        // Reference to the player's transform
        private Transform playerTransform;
        private Animator anim;

        // Distance at which the Gnome will start following the player
        public float followDistance = 10f;

        // Distance at which the Gnome will start attacking the player
        public float attackDistance = 2f;

        // Gnome's movement speed
        public float speed = 5f;

        // Health points of the Gnome
        public int health = 3;

        // Attack damage of the Gnome
        public int damage = 1;

        public float attackRate = 10f;
        public bool isInjured;

        private float lastAttack = 0f;

        // Time it takes for the Gnome to recover after being injured
        public float recoveryTime = 10f;

        // Timer to keep track of recovery time
        private float recoveryTimer;

        void Start()
        {        // Find the player's transform
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            anim = GetComponent<Animator>();
        }


        void Update()
        {
            if (anim.GetInteger("EnemyState") != (int)currentState)
                anim.SetInteger("EnemyState", (int)currentState);

            // State machine update
            switch (currentState)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Follow:
                    Follow();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Injured:
                    Injured();
                    break;
                case EnemyState.Death:
                    Death();
                    break;
            }

            //if the Gnome collide with the bullet, then the Gnome will go to the injure state

        }

        // Idle state: Gnome stays in place and does nothing
        void Idle()
        {
            // anim.SetBool("isIdle", true);
            // isInjured = false;
            // Calculate distance to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < followDistance)
            {
                currentState = EnemyState.Follow;

            }
        }

        // Follow state: Gnome follows the player
        void Follow()
        {
            //  isInjured = false;
            // Calculate distance to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // If player is within attack distance, switch to Attack state
            if (distance <= attackDistance)
            {
                //  anim.SetBool("isIdle", false);
                //  anim.SetBool("isFollow", false);
                currentState = EnemyState.Attack;


            }
            // If player is out of follow distance, switch to Idle state
            else if (distance > followDistance)
            {
                //   anim.SetBool("isFollow", false);
                currentState = EnemyState.Idle;

            }
            // Move towards the player
            else
            {
                transform.LookAt(playerTransform);
                transform.position += transform.forward * speed * Time.deltaTime;

            }
        }

        public void TakeDamage(int amount)
        {
            //Pass the "amount" to Invoke
            //CharacterTookDamage? => check if this is "null" => if(CharacterTookDamage!=null)
            EnemyTookDamage?.Invoke(amount);
            health -= amount;
            currentState = EnemyState.Injured;
            if (health <= 0)
            {
                Death();
            }
        }

        // Attack state: Gnome attacks the player
        void Attack()
        {

            // Calculate distance to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // If player is out of attack distance, switch to Follow state
            if (distance > attackDistance)
            {
                currentState = EnemyState.Follow;
            }

            else if (Time.time > lastAttack + attackRate)
            {
                lastAttack = Time.time;

                // anim.SetBool("isAttack", true);
                playerTransform.GetComponent<Player>().TakeDamage(damage);
            }



            // Damage the player



        }

        // Injured state: Gnome is temporarily unable to do anything
        public void Injured()
        {
            // Wait for recovery time to pass
            //  anim.SetBool("isInjure", true);
            recoveryTimer -= Time.deltaTime;


            if (recoveryTimer <= 0)
            {
                // If health is zero, switch to Death state, otherwise switch to Follow state
                if (health <= 0)
                {
                    currentState = EnemyState.Death;
                }
                else
                {
                    currentState = EnemyState.Follow;
                    isInjured = !isInjured;
                }
            }
        }
        // if the Gnome got shot by a bullet
        /*     public void OnTriggerEnter(Collider other)
          {
               if (other.gameObject.CompareTag("Bullet"))
               {
                   isInjured = true;
                   currentState = BatState.Injured;
                  // this.TakeDamage(int amout);
                   FlowerInventory.instance.DecreaseFlowerCount();
                   //Destroy(other);
                   Debug.Log("a Gnome being hit");
               }
           }*/

        // Death state: Gnome dies and is destroyed
        void Death()
        {
            // anim.SetBool("isDie", true);

            Destroy(gameObject);
        }

        // Function to handle taking damage

    }
}