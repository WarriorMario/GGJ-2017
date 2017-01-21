using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPole : MonoBehaviour
{
    int m_spawner;
    float m_timeLeft;

    public void Init(float a_timeLeft, int a_spawner)
    {
        m_spawner = a_spawner;
        m_timeLeft = a_timeLeft;
    }

    private void Update()
    {
        m_timeLeft -= Time.deltaTime;
        if(m_timeLeft < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wave")
        {
            Wave w = other.GetComponent<Wave>();
            w.Bounce();
        }
        if (other.tag == "ReversedWave")
        {
            ReversedWave w = other.GetComponent<ReversedWave>();
            w.Bounce();
        }
    }
}
