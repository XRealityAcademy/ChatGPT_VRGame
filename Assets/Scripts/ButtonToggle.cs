using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    public GameObject uiPanel;
    public bool setActive;
    // Start is called before the first frame update
    void Start()
    {
        setActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Toogle()
    {
        setActive = !setActive;
        uiPanel.SetActive(setActive);
    }
}
