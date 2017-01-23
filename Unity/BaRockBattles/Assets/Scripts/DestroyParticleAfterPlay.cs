using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleAfterPlay : MonoBehaviour {

    ParticleSystem m_system;

    public void Start()
    {
        m_system = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = m_system.main;
        Destroy(gameObject, main.startLifetime.constantMax);
    }
}
