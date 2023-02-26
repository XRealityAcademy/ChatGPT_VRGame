using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
   float currentSize;
   float growSpeed = 0.05f;

    void Start() {
        currentSize = 0f;
    }
    
    void Update() {
        currentSize += growSpeed * Time.deltaTime;
        if (currentSize >= 1f) {
            currentSize = 1f; 
        } 
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);
    }

    
}
