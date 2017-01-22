﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputWrapper;


[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public Defines.EPlayerType m_playerType;
    public int                 m_controlId;
    // basic variables
    public float m_maxYvel;
    public float m_gravity;

    // References.
    CharacterController m_myController;
    
    Vector3 m_moveDir; // current velocity vector

    // Player type specific variables
    float m_timeUntillAction1DelayTimerASFloatingPointMightAlsoPossbilyBeSetToZeroIfYouDontWantADelay;
    bool m_actionButtonPressed = false;
    static int ms_MadPropsForBeingAweesomeProgrammer = 0;

    float m_action1CooldownTimer;
    float m_action2CooldownTimer;

    // Shield
    bool  m_shieldIsActive;
    float m_shieldTimeActive;

    // Clone
    PlayerControl m_clone;
    bool          m_isClone;
    float         m_cloneTimeAlive;

    // Speed boost
    bool  m_speedBoostIsActive;
    float m_speedBoostTimeActive;

    // CooldownBar
    CooldownBar m_cooldownBar;
    float m_currentCooldownMaxTime = 0.0f;

    
    ///////////////////////////
    // Game loop
    ///////////////////////////
    void Awake()
    {
        m_myController = GetComponent<CharacterController>();
        m_cooldownBar = GetComponentInChildren<CooldownBar>();
    }

	void Update()
    {
        if(!m_myController.isGrounded)
        { 
            if (m_myController.velocity.y > m_maxYvel)
            {
                m_moveDir.y -= m_gravity * Time.deltaTime;
                if(m_moveDir.y > m_maxYvel)
                {
                    m_moveDir.y = m_maxYvel;
                }
            }
        }

        // Check if player should die.
        if(transform.position.y < Defines.PLAYER_MINY)
        {
            if(m_isClone)
            {
                RemoveClone();
                return;
            }
            else
            {
                GameLoop.Instance.NotifyPlayerDeath(this);
                Destroy(gameObject);
            }
        }

        GameplayVariables gameVars   = GameLoop.Instance.m_gameplayVariables;
        PlayerVariables   playerVars = GameLoop.Instance.GetPlayerVariables(m_playerType);
        Scheme            controls   = gameVars.m_controls;

        Vector2 movement = controls.GetPressAsAxis(EKeyPairId.EKeyPairId_HorizontalLeft, EKeyPairId.EKeyPairId_VerticalLeft, m_controlId);
        Vector2 dir      = controls.GetPressAsAxis(EKeyPairId.EKeyPairId_HorizontalRight, EKeyPairId.EKeyPairId_VerticalRight, m_controlId);
        
        if (m_shieldIsActive)
        {
            HeavyVariables vars = GameLoop.Instance.m_gameplayVariables.m_heavy;

            m_shieldTimeActive += Time.deltaTime;
            if (m_shieldTimeActive > vars.m_blockPoleLifeTime)
            {
                m_action2CooldownTimer = vars.m_blockPoleCooldown;
                m_currentCooldownMaxTime = vars.m_blockPoleCooldown;
                m_cooldownBar.Activate();

                m_shieldTimeActive = 0.0f;
                m_shieldIsActive = false;
            }
        }
        if (m_clone != null)
        {
            LightVariables vars = GameLoop.Instance.m_gameplayVariables.m_light;

            // Auto de-spawn.
            m_clone.m_cloneTimeAlive += Time.deltaTime;
            if (m_clone.m_cloneTimeAlive > vars.m_cloneLifetime)
            {
                RemoveClone();
            }
        }
        if (m_speedBoostIsActive)
        {
            MediumVariables vars = GameLoop.Instance.m_gameplayVariables.m_medium;

            m_speedBoostTimeActive += Time.deltaTime;
            if (m_speedBoostTimeActive > vars.m_speedBoostDuration)
            {
                m_action2CooldownTimer = vars.m_speedBoostCooldown;
                m_currentCooldownMaxTime = vars.m_speedBoostCooldown;
                m_cooldownBar.Activate();

                m_speedBoostTimeActive = 0.0f;
                m_speedBoostIsActive = false;
            }
            else
            {
                movement *= vars.m_speedBoostSpeedScale;
            }
        }
        
        if (m_isClone)
        {
            movement = -movement;
            dir      = -dir;
        }

        movement *= playerVars.m_movementAccelerationSpeed * Time.deltaTime;
        m_moveDir += new Vector3(movement.x, 0.0f, movement.y);
        m_moveDir -= m_moveDir * playerVars.m_movementDrag * Time.deltaTime;
        m_myController.SimpleMove(m_moveDir);
        
        if (dir != Vector2.zero)
        {
            transform.forward = new Vector3(dir.x, 0.0f, dir.y).normalized;
        }

        if (m_isClone) return;

        if(m_actionButtonPressed)
        {
            m_timeUntillAction1DelayTimerASFloatingPointMightAlsoPossbilyBeSetToZeroIfYouDontWantADelay += Time.deltaTime;
            if (m_timeUntillAction1DelayTimerASFloatingPointMightAlsoPossbilyBeSetToZeroIfYouDontWantADelay > playerVars.m_waveSpawnDelay)
            {
                PerformAction1();
                m_actionButtonPressed = false;
                m_timeUntillAction1DelayTimerASFloatingPointMightAlsoPossbilyBeSetToZeroIfYouDontWantADelay = 0.0f;
                ms_MadPropsForBeingAweesomeProgrammer++;
            }
        }

        m_action1CooldownTimer -= Time.deltaTime;
        if (controls.GetDown(EKeyId.EKeyId_Action1, m_controlId) && m_action1CooldownTimer <= 0.0f)
        {
            m_actionButtonPressed = true;
        }

        if (m_shieldIsActive) return;

        m_action2CooldownTimer -= Time.deltaTime;
        if(m_action2CooldownTimer < 0.0f)
        {
            if(m_cooldownBar.gameObject.activeSelf)
                m_cooldownBar.Deactivate();
        }
        else
        {
            m_cooldownBar.UpdateBar(1.0f - (m_action2CooldownTimer / m_currentCooldownMaxTime));
        }
        if (controls.GetDown(EKeyId.EKeyId_Action2, m_controlId) && m_action2CooldownTimer <= 0.0f)
        {
            PerformAction2();
        }
	}

    //////////////////////////////
    // Action 1
    //////////////////////////////
    void FireWave(GameObject a_prefab, Vector3 a_start, Vector3 a_dir)
    {
        PlayerVariables vars = GameLoop.Instance.GetPlayerVariables(m_playerType);
        
        Wave wave = Instantiate(a_prefab, a_start, Quaternion.identity).GetComponent<Wave>();

        wave.Init(a_dir * vars.m_waveSpeed, m_controlId, vars.m_waveMinWidth, vars.m_waveMaxDistance, vars.m_waveAngle, vars.m_waveMinPower, vars.m_waveMaxPower);
        m_moveDir -= a_dir * vars.m_attackAccelerationSpeed;
    }
    void PerformAction1()
    {
        GameplayVariables gameVars   = GameLoop.Instance.m_gameplayVariables;
        PlayerVariables   playerVars = GameLoop.Instance.GetPlayerVariables(m_playerType);

        switch (m_playerType)
        {
            case Defines.EPlayerType.heavy: // Drums
                FireWave(gameVars.m_wavePrefab, transform.position + transform.forward * gameVars.m_heavy.m_waveToDrumOffset, -transform.right);
                FireWave(gameVars.m_wavePrefab, transform.position + transform.forward * gameVars.m_heavy.m_waveToDrumOffset,  transform.right);
                m_action1CooldownTimer = gameVars.m_heavy.m_attackDelayTime;
                break;
            case Defines.EPlayerType.medium: // Violin
                FireWave(gameVars.m_wavePrefab, transform.position, transform.forward);
                m_action1CooldownTimer = gameVars.m_medium.m_attackDelayTime;
                break;
            case Defines.EPlayerType.light: // Flute
                FireWave(gameVars.m_wavePrefab, transform.position, transform.forward);
                if (m_clone != null)
                {
                    RemoveClone();
                }
                m_action1CooldownTimer = gameVars.m_light.m_attackDelayTime;
                break;
            case Defines.EPlayerType.strange: // Didgeridoo
                FireWave(gameVars.m_wavePrefab, transform.position + transform.forward * gameVars.m_strange.m_waveSpawnOffset, - transform.forward);
                m_action1CooldownTimer = gameVars.m_strange.m_attackDelayTime;
                break;
        }
    }

    //////////////////////////////
    // Action 2
    //////////////////////////////
    void SpawnClone()
    {
        GameplayVariables gameVars = GameLoop.Instance.m_gameplayVariables;
        LightVariables lightVars = gameVars.m_light;

        // Spawn clone.
        Vector3 splitPos = transform.position;
        PlayerControl clone = Instantiate(gameVars.m_lightPrefab, splitPos, transform.rotation).GetComponent<PlayerControl>();
        clone.m_controlId = m_controlId;
        clone.m_isClone = true;
        m_clone = clone;

        // Move players away from eachother (left, right).
        Vector3 splitOffset = lightVars.m_cloneSpawnOffset * transform.right;
        float sign = (int)(Random.value + 0.5f);
        clone.transform.position += splitOffset * sign;
        transform.position       += splitOffset * -sign;
    }
    void RemoveClone()
    {
        GameplayVariables gameVars = GameLoop.Instance.m_gameplayVariables;
        LightVariables lightVars = gameVars.m_light;

        Destroy(m_clone.gameObject);
        m_clone = null;
        m_action2CooldownTimer = lightVars.m_cloneCooldown;
        m_currentCooldownMaxTime = lightVars.m_cloneCooldown;
        m_cooldownBar.Activate();
    }
    void PerformAction2()
    {
        GameplayVariables gameVars   = GameLoop.Instance.m_gameplayVariables;
        PlayerVariables   playerVars = GameLoop.Instance.GetPlayerVariables(m_playerType);

        switch (m_playerType)
        {
            case Defines.EPlayerType.heavy: // Drums
                {
                    HeavyVariables heavyVars = gameVars.m_heavy;
                    
                    BlockPole p = Instantiate(heavyVars.m_blockPolePrefab, transform.position + transform.forward * heavyVars.m_blockPoleSpawnOffset, transform.rotation).GetComponent<BlockPole>();
                    p.Init(heavyVars.m_blockPoleLifeTime, m_controlId);

                    m_shieldIsActive = true;
                }
                break;
            case Defines.EPlayerType.medium: // Violin
                {
                    MediumVariables mediumVars = gameVars.m_medium;

                    m_speedBoostIsActive = true;

                    break;
                }
            case Defines.EPlayerType.light: // Flute
                {
                    LightVariables lightVars = gameVars.m_light;

                    if(m_clone == null)
                    {
                        SpawnClone();
                    }

                    break;
                }
            case Defines.EPlayerType.strange: // Didgeridoo
                {
                    StrangeVariables strangeVars = gameVars.m_strange;

                    Vector2 lsd = gameVars.m_controls.GetPressAsAxis(EKeyPairId.EKeyPairId_HorizontalLeft, EKeyPairId.EKeyPairId_VerticalLeft, m_controlId);
                    if (lsd.sqrMagnitude > 0)
                    {
                        Vector3 res = new Vector3(lsd.x, 0.0f, lsd.y);
                        
                        m_moveDir += strangeVars.m_dodgePower * res.normalized;
                    }
                    else
                    {
                        m_moveDir += strangeVars.m_dodgePower * transform.forward;
                    }
                    m_action2CooldownTimer = strangeVars.m_dodgeCooldown;
                    m_currentCooldownMaxTime = strangeVars.m_dodgeCooldown;
                    m_cooldownBar.Activate();

                    break;
                }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    public void GetHitByWave(Wave a_wave)
    {
        m_moveDir += a_wave.GetForce(transform.position);
    }
}
