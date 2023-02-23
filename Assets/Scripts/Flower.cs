using UnityEngine;

public class Flower : MonoBehaviour
{
    public float growthRate = 0.045f; // rate at which the flower grows per second
    public float timeToGrow = 20f; // time it takes for the flower to grow in seconds
    public GameObject flower;
    private bool isGrown = false;

    private void Start()
    {
        isGrown = false;
        InvokeRepeating("Grow", 0f, 1f);
    }

    private void Grow()
    {
        if (!isGrown)
        {
            transform.localScale += new Vector3(growthRate, growthRate, growthRate);
            if (transform.localScale.x >= 1f)
            {
                isGrown = true;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            FlowerInventory.instance.IncrementFlowerCount();
            Destroy(flower);
            Debug.Log("a flower being destroyed");
        }
    }
}
