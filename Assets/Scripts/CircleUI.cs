using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleUI : MonoBehaviour
{
    public GameObject ControllerUICanvas;
    public GameObject circlePanel;
    public GameObject flowerPanel;
    public bool isPanelActive;

    // Start is called before the first frame update
    void Start()
    {
        circlePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            // Toggle panel on and off
            isPanelActive = !isPanelActive;
            circlePanel.SetActive(isPanelActive);
        }

    }

}
