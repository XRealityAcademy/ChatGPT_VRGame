using UnityEngine;
using TMPro;

public class FlowerInventory : MonoBehaviour
{
    public static FlowerInventory instance;

    public int flowerCount = 0; // number of flowers collected

    public TextMeshProUGUI flowerCountText; // TextMeshPro object to display flower count

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementFlowerCount()
    {
        flowerCount++;
        UpdateInventoryUI();
    }

    public void DecreaseFlowerCount()
    {
        flowerCount--;
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        flowerCountText.text = flowerCount.ToString();
    }
}