using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[DefaultExecutionOrder(5)]
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
    public GameObject CircleUI;

    [field:SerializeField]
    public int Health { get; private set; }

    [field: SerializeField]
    public int MaxHealth { get; private set; }

    private LaserPointer laserPointer;
    private bool isLookingAtUi;

    void Start()
    {
        controller = OVRInput.Controller.RTouch;
        laserPointer = FindObjectOfType<LaserPointer>();
        laserPointer.OnStateChanged += OnLookingAtUi;
    }

    private void OnLookingAtUi(bool isLooking)
    {
        isLookingAtUi = isLooking;
    }

    void Update()
    {
        // Check if the index trigger on the controller is pressed down
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) && !isLookingAtUi)
        { 
            Shoot();
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Circle();
        }
    }

    void Shoot()
    {
        // Spawn a new bullet object at the bullet spawn point
      /*  GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Get the rigidbody component of the bullet object
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();

        // Add force to the bullet in the forward direction
        bulletRigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange); */

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        Destroy(bullet, 5f);



    }


    public void Circle()
    {
        CircleUI.SetActive(!CircleUI.activeSelf);
        Debug.Log("CicleUI active");
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
