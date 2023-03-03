using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BatAI;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    //private GameObject bullet;
    public int damage = 1;
    public GameObject Gnome;
    public GameObject Bat;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
       // bullet = this.gameObject;
        Gnome = GameObject.FindWithTag("Gnome");
        Bat = GameObject.FindWithTag("Bat");
        hit = Gnome.GetComponent<BatAI>().isInjured;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //The first Gnome dies, and the following bats are never die
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bat"))
        {
            hit = true;
            //take Gnome's dammage
            Bat.GetComponent<BatAI>().TakeDamage(damage);
            FlowerInventory.instance.DecreaseFlowerCount();

            Bat.GetComponent<BatAI>().currentState = BatState.Injured;

            Debug.Log("a bullet being shot");
            Destroy(this.gameObject);
        }
    }
}
