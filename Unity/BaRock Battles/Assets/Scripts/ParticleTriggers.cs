using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggers : MonoBehaviour {

    // List of particle effects that the player has to handle
    public GameObject m_droppingParticle;
    public GameObject m_drumParticle;
    public GameObject m_fatCloudParticle;
    public GameObject m_fluteParticle;
    public GameObject m_hitMarkerParticle;
    public GameObject m_speedParticle;
    public GameObject m_violinParticle;
    
    public void SpawnVictoryParticle(PlayerControl m_victor)
    {

    }

    public void SpawnDroppingParticle(PlayerControl a_player)
    {
        Instantiate(m_droppingParticle, a_player.transform);
    }

    public void SpawnDrumParticle(PlayerControl a_player, Wave a_wave)
    {
        Instantiate(m_drumParticle, a_player.transform);
    }

    public void SpawnViolinParticle(PlayerControl a_player, Wave a_wave)
    {
        Instantiate(m_violinParticle, a_player.transform);
    }

    public void SpawnFluteParticle(PlayerControl a_player, Wave a_wave)
    {
        Instantiate(m_fluteParticle, a_player.transform);
    }

    public void SpawnDidgeridooParticle(PlayerControl a_player, PlayerControl a_target)
    {

    }

    public void SpawnShieldParticle(BlockPole a_pole)
    {

    }

    public void SpawnCloneParticle(PlayerControl a_player)
    {

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
