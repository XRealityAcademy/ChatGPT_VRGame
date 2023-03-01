using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BatAI;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    public int damage = 1;
    public GameObject bat;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        bullet = this.gameObject;
        bat = GameObject.FindWithTag("Bat");
        hit = bat.GetComponent<BatAI>().isInjured;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bat"))
        {
            hit = true;
            //take bat's dammage
            bat.GetComponent<BatAI>().TakeDamage(damage);
            FlowerInventory.instance.DecreaseFlowerCount();
            
            bat.GetComponent<BatAI>().currentState = BatState.Injured;

            Debug.Log("a bullet being shot");
            Destroy(bullet);
        }
    }
}
