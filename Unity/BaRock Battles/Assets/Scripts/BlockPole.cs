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
            w.Init(-w.m_dir, m_spawner, w.m_minWidth, w.m_maxLength, w.m_angle, w.m_minPower, w.m_maxPower, transform.position);
        }
        if (other.tag == "ReversedWave")
        {
            ReversedWave w = other.GetComponent<ReversedWave>();
            w.Init(-w.m_dir, m_spawner, w.m_minWidth, w.m_maxLength, w.m_angle, w.m_minPower, w.m_maxPower, transform.position + -w.m_dir * w.m_maxLength);
        }
    }
}
