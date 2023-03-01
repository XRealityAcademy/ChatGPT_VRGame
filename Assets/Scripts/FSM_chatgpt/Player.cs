using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Action is an event
    public Action<int> CharacterTookDamage;
    public GameObject bulletPrefab; // The prefab for the bullet object
    public Transform bulletSpawnPoint; // The point from where the bullet will be spawned
    public float bulletSpeed = 10f; // The speed at which the bullet will travel
    public Transform batTransform;
    public int bulletPower=1;
    private OVRInput.Controller controller; // The Quest 2 controller being used

    [field:SerializeField]
    public int Health { get; private set; }

    [field: SerializeField]
    public int MaxHealth { get; private set; }

    void Start()
    {
        controller = OVRInput.Controller.RTouch;
    }

    void Update()
    {
        // Check if the index trigger on the controller is pressed down
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Spawn a new bullet object at the bullet spawn point
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Get the rigidbody component of the bullet object
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();

        // Add force to the bullet in the forward direction
        bulletRigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);



    }




    public void TakeDamage(int amount)
    {
        //Pass the "amount" to Invoke
        //CharacterTookDamage? => check if this is "null" => if(CharacterTookDamage!=null)
        CharacterTookDamage?.Invoke(amount);

        Health -= amount;
        if(Health <= 0)
        {
           // Die();
        }
    }

    public void Attack() 
    {

    
    
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
