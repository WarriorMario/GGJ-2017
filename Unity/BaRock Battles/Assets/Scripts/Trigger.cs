using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    ParticleTriggers m_particleTriggers;
    // audio
    // anim

    public void Init()
    {
        m_particleTriggers = FindObjectOfType<ParticleTriggers>();

        // Give a reference of all players to Andrès

    }

    public void PlayerDrumAttackWave(PlayerControl a_player, Wave a_wave)
    {
        m_particleTriggers.SpawnDrumParticle(a_player, a_wave);
    }

    public void PlayerDrumAttackSecondary(PlayerControl a_player, BlockPole a_pole)
    {
        m_particleTriggers.SpawnShieldParticle(a_pole);
    }

    public void PlayerViolinAttackWave(PlayerControl a_player, Wave a_wave)
    {
        m_particleTriggers.SpawnViolinParticle(a_player, a_wave);
    }

    public void PlayerViolinAttackSecondary(PlayerControl a_player, Wave a_wave)
    {
        m_particleTriggers.SpawnSpeedParticle(a_player);
    }

    public void PlayerFluteAttackWave(PlayerControl a_player, Wave a_wave)
    {
        m_particleTriggers.SpawnFluteParticle(a_player, a_wave);
    }

    public void PlayerFluteAttackSecondary(PlayerControl a_player, PlayerControl a_clone)
    {
        m_particleTriggers.SpawnCloneParticle(a_player);
    }

    public void PlayerDidgeridooAttack(PlayerControl a_player, PlayerControl a_target)
    {
        m_particleTriggers.SpawnDidgeridooParticle(a_player, a_target);
    }

    public void PlayerDigeridooAttackSecondary(PlayerControl a_player)
    {
        m_particleTriggers.SpawnDodgeParticle(a_player);
    }

    public void PlayerDies(PlayerControl a_player)
    {
        m_particleTriggers.SpawnDroppingParticle(a_player);
    }

    public void PlayerWins(PlayerControl a_player)
    {
        m_particleTriggers.SpawnVictoryParticle(a_player);
    }
}
