using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieFlower
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        public int damage = 1;

        private void Start()
        {
            Invoke("RemoveBullet", 10f);
        }


        //The first Gnome dies, and the following bats are never die
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {

                //take Gnome's dammage
                EnemyAI enemy = other.gameObject.GetComponentInParent<EnemyAI>();
                enemy.TakeDamage(damage);
                FlowerInventory.instance.DecreaseFlowerCount();
                Debug.Log("The bullet hit " + enemy.gameObject);
                RemoveBullet();
            }

            if (other.gameObject.CompareTag("Gnome"))
            {

                //take Gnome's dammage
                EnemyAI enemy = other.gameObject.GetComponentInParent<EnemyAI>();
                enemy.TakeDamage(damage);
                FlowerInventory.instance.DecreaseFlowerCount();
                Debug.Log("The bullet hit " + enemy.gameObject);
                RemoveBullet();
            }
        }

        void RemoveBullet() { Destroy(this.gameObject); }
    }
}
