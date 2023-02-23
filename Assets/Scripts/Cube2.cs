using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : MonoBehaviour
{
    private float m_growTime = 20f;
    private static Vector3 m_finalScale = new Vector3(1f, 1f, 1f);
    private float m_timer;

    void Start() {
        m_timer = 0f;
    }

    void Update () {
        if (m_timer >= m_growTime) 
            return;
        m_timer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.zero, m_finalScale, m_timer/m_growTime);
    }
}
