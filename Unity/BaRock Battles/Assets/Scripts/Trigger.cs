using UnityEngine;


public class Trigger
{
    public static void PlayerDrumAttackWave(PlayerControl a_player, Wave a_wave)
    {
        GameLoop.Instance.m_particleTriggers.SpawnDrumParticle(a_player, a_wave);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlayNormalAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public static void PlayerDrumAttackSecondary(PlayerControl a_player, BlockPole a_pole)
    {
        GameLoop.Instance.m_particleTriggers.SpawnShieldParticle(a_pole);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlaySpecialAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("SpecialAttack");
    }

    public static void PlayerViolinAttackWave(PlayerControl a_player, Wave a_wave)
    {
        GameLoop.Instance.m_particleTriggers.SpawnViolinParticle(a_player, a_wave);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlayNormalAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public static void PlayerViolinAttackSecondary(PlayerControl a_player)
    {
        GameLoop.Instance.m_particleTriggers.SpawnSpeedParticle(a_player);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlaySpecialAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("SpecialAttack");
    }

    public static void PlayerFluteAttackWave(PlayerControl a_player, Wave a_wave)
    {
        GameLoop.Instance.m_particleTriggers.SpawnFluteParticle(a_player, a_wave);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlayNormalAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public static void PlayerFluteAttackSecondary(PlayerControl a_player, PlayerControl a_clone)
    {
        GameLoop.Instance.m_particleTriggers.SpawnCloneParticle(a_player);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlaySpecialAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("SpecialAttack");
    }

    public static void PlayerDidgeridooAttackWave(PlayerControl a_player, Wave a_wave)
    {
        GameLoop.Instance.m_particleTriggers.SpawnDidgeridooParticle(a_player, a_wave);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlayNormalAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public static void PlayerDigeridooAttackSecondary(PlayerControl a_player)
    {
        GameLoop.Instance.m_particleTriggers.SpawnDodgeParticle(a_player);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlaySpecialAttackSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("SpecialAttack");
    }

    public static void PlayerDies(PlayerControl a_player)
    {
        GameLoop.Instance.m_particleTriggers.SpawnDroppingParticle(a_player);
        a_player.transform.GetComponentInChildren<PlayerAudio>().PlayDeathSound();
        a_player.transform.GetComponentInChildren<Animator>().SetTrigger("Death");
    }

    public static void PlayerWins(PlayerControl a_player)
    {
        GameLoop.Instance.m_particleTriggers.SpawnVictoryParticle(a_player);
        // Implement win sound
    }
}
