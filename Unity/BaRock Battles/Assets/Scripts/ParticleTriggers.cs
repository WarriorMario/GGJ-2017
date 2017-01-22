using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggers : MonoBehaviour {

    // List of particle effects that the player has to handle
    public GameObject m_droppingParticle;
    public GameObject m_fatCloudParticle;
    public GameObject m_hitMarkerParticle;
    public GameObject m_speedParticle;

    public GameObject m_drumParticle;
    public GameObject m_fluteParticle;
    public GameObject m_violinParticle;
    public GameObject m_didgeridooParticle;

    public void SpawnVictoryParticle(PlayerControl m_victor)
    {

    }

    public void SpawnDroppingParticle(PlayerControl a_player)
    {
        ParticleSystem particles = Instantiate(m_droppingParticle, a_player.transform.position, a_player.transform.rotation).GetComponent<ParticleSystem>();
        Destroy(particles.gameObject, particles.main.startLifetime.constant * 2.0f);
    }

    public void SpawnDrumParticle(PlayerControl a_player, Wave a_wave)
    {
        ParticleSystem particles = Instantiate(m_drumParticle, a_player.transform.position, a_player.transform.rotation).GetComponent<ParticleSystem>();
        Destroy(particles.gameObject, particles.main.startLifetime.constant * 2.0f);
    }

    public void SpawnViolinParticle(PlayerControl a_player, Wave a_wave)
    {
        ParticleSystem particles = Instantiate(m_violinParticle, a_player.transform.position, a_player.transform.rotation).GetComponent<ParticleSystem>();
        Destroy(particles.gameObject, particles.main.startLifetime.constant * 2.0f);
    }

    public void SpawnFluteParticle(PlayerControl a_player, Wave a_wave)
    {
        ParticleSystem particles = Instantiate(m_fluteParticle, a_player.transform.position, a_player.transform.rotation).GetComponent<ParticleSystem>();
        Destroy(particles.gameObject, particles.main.startLifetime.constant * 2.0f);
    }

    public void SpawnDidgeridooParticle(PlayerControl a_player, Wave a_wave)
    {
        ParticleSystem particles = Instantiate(m_didgeridooParticle, a_player.transform.position, a_player.transform.rotation).GetComponent<ParticleSystem>();
        Destroy(particles.gameObject, particles.main.startLifetime.constant * 2.0f);
        
    }

    public void SpawnShieldParticle(BlockPole a_pole)
    {
        Instantiate(m_fatCloudParticle, a_pole.transform);
    }

    public void SpawnCloneParticle(PlayerControl a_player)
    {
        Instantiate(m_fatCloudParticle, a_player.transform);
    }

    public void SpawnDodgeParticle(PlayerControl a_player)
    {
        Instantiate(m_speedParticle, a_player.transform);
    }

    public void SpawnSpeedParticle(PlayerControl a_player)
    {
        Instantiate(m_speedParticle, a_player.transform);
    }
}
