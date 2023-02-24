using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleUI : MonoBehaviour
{
    public GameObject circleUI;
    public GameObject flowerPanel;
    public bool isFlowerPanelOpen;

    // Start is called before the first frame update
    void Start()
    {
        isFlowerPanelOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlowerPanel()
    {
        isFlowerPanelOpen = !isFlowerPanelOpen;
        flowerPanel.SetActive(isFlowerPanelOpen);
    }

}
