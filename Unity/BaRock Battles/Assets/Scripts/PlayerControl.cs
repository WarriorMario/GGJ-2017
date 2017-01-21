using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputWrapper;


[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public Defines.EPlayerType m_playerType;
    public int                 m_controlId;

    // References.
    CharacterController m_myController;
    
    Vector3 m_moveDir; // current velocity vector
    Vector3 m_shootDir = Vector3.right; // the last known direction to shoot in

    // Player type specific variables
    float m_attackDelayTimeSpent;
    
    void Start()
    {
        m_myController = GetComponent<CharacterController>();
	}
	
	void Update()
    {
        // Check if player should die.
        if(transform.position.y < Defines.PLAYER_MINY)
        {
            GameLoop.Instance.NotifyPlayerDeath(this);
            Destroy(gameObject);
        }


        GameplayVariables gameVars   = GameLoop.Instance.m_gameplayVariables;
        PlayerVariables   playerVars = GameLoop.Instance.GetPlayerVariables(m_playerType);
        Scheme            controls   = gameVars.m_controls;

        Vector2 movement = controls.GetPressAsAxis(EKeyPairId.EKeyPairId_HorizontalLeft, EKeyPairId.EKeyPairId_VerticalLeft, m_controlId);
        movement *= playerVars.m_movementAccelerationSpeed * Time.deltaTime;
        m_moveDir += new Vector3(movement.x, 0.0f, movement.y);
        m_moveDir -= m_moveDir * playerVars.m_movementDrag * Time.deltaTime;
        m_myController.SimpleMove(m_moveDir);

        Vector2 dir = controls.GetPressAsAxis(EKeyPairId.EKeyPairId_HorizontalRight, EKeyPairId.EKeyPairId_VerticalRight, m_controlId);
        if (dir != Vector2.zero)
        {
            dir.Normalize();
            m_shootDir = new Vector3(dir.x, 0.0f, dir.y);
            transform.forward = m_shootDir;
        }
        
        if(m_attackDelayTimeSpent < playerVars.m_attackDelayTime)
        {
            return;
        }

        switch(m_playerType)
        {
            case Defines.EPlayerType.heavy:
                {
                    HeavyVariables heavyVars = gameVars.m_heavy;

                    if (controls.GetDown(EKeyId.EKeyId_Action1, m_controlId))
                    {
                        Vector3 shootTemp = m_shootDir;
                        m_shootDir = new Vector3(m_shootDir.z, 0.0f, -m_shootDir.x);
                        StartAttack();
                        m_shootDir = -m_shootDir;
                        StartAttack();
                        m_shootDir = shootTemp;
                    }
                    if (controls.GetDown(EKeyId.EKeyId_Action2, m_controlId))
                    {
                        BlockPole p = Instantiate(heavyVars.m_blockPolePrefab, transform.position, Quaternion.identity).GetComponent<BlockPole>();
                        p.Init(heavyVars.m_blockPoleLifeTime, m_controlId);
                    }
                }
                break;
            case Defines.EPlayerType.medium:
                {
                    MediumVariables mediumVars = gameVars.m_medium;

                    m_attackDelayTimeSpent += Time.deltaTime;
                    if (controls.GetDown(EKeyId.EKeyId_Action1, m_controlId))
                    {
                        StartAttack();
                    }
                    break;
                }
            case Defines.EPlayerType.light:
                {
                    LightVariables lightVars = gameVars.m_light;

                    m_attackDelayTimeSpent += Time.deltaTime;
                    if (controls.GetDown(EKeyId.EKeyId_Action1, m_controlId))
                    {
                        StartAttack();
                    }
                    break;
                }
            case Defines.EPlayerType.strange:
                {
                    StrangeVariables strangeVars = gameVars.m_strange;

                    if (controls.GetDown(EKeyId.EKeyId_Action1, m_controlId))
                    {
                        StartAttack();
                    }
                    break;
                }
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    void StartAttack()
    {
        GameplayVariables gameVars = GameLoop.Instance.m_gameplayVariables;
        PlayerVariables playerVars = GameLoop.Instance.GetPlayerVariables(m_playerType);
        
        Vector3 spawnPos = transform.position + m_shootDir * playerVars.m_waveSpawnOffset;

        switch (m_playerType)
        {
            case Defines.EPlayerType.heavy:
                {
                    HeavyVariables vars = gameVars.m_heavy;

                    Wave wave = Instantiate(vars.m_wavePrefab, spawnPos, Quaternion.identity).GetComponent<Wave>();
                    wave.Init(m_shootDir * vars.m_waveSpeed, m_controlId, vars.m_waveMinWidth, vars.m_waveMaxLength, vars.m_waveAngle, vars.m_waveMinPower, vars.m_waveMaxPower);
                    m_moveDir -= m_shootDir.normalized * vars.m_attackAccelerationSpeed;

                    break;
                }
            case Defines.EPlayerType.medium:
                {
                    MediumVariables vars = gameVars.m_medium;

                    Wave wave = Instantiate(vars.m_wavePrefab, spawnPos, Quaternion.identity).GetComponent<Wave>();
                    wave.Init(m_shootDir * vars.m_waveSpeed, m_controlId, vars.m_waveMinWidth, vars.m_waveMaxLength, vars.m_waveAngle, vars.m_waveMinPower, vars.m_waveMaxPower);
                    m_moveDir -= m_shootDir.normalized * vars.m_attackAccelerationSpeed;

                    break;
                }
            case Defines.EPlayerType.light:
                {
                    LightVariables vars = gameVars.m_light;

                    Wave wave = Instantiate(vars.m_wavePrefab, spawnPos, Quaternion.identity).GetComponent<Wave>();
                    wave.Init(m_shootDir * vars.m_waveSpeed, m_controlId, vars.m_waveMinWidth, vars.m_waveMaxLength, vars.m_waveAngle, vars.m_waveMinPower, vars.m_waveMaxPower);
                    m_moveDir -= m_shootDir.normalized * vars.m_attackAccelerationSpeed;

                    break;
                }
            case Defines.EPlayerType.strange:
                {
                    StrangeVariables vars = gameVars.m_strange;

                    ReversedWave wave = Instantiate(vars.m_reversedWavePrefab, spawnPos, Quaternion.identity).GetComponent<ReversedWave>();
                    wave.Init(m_shootDir * vars.m_waveSpeed, m_controlId, vars.m_waveMinWidth, vars.m_waveMaxLength, vars.m_waveAngle, vars.m_waveMinPower, vars.m_waveMaxPower);
                    m_moveDir -= m_shootDir.normalized * vars.m_attackAccelerationSpeed;

                    break;
                }
        }

        m_attackDelayTimeSpent = 0.0f;
    }

    public void GetHitByWave(Wave a_wave)
    {
        m_moveDir += a_wave.Direction * a_wave.GetPower();
    }
    public void GetHitByWave(ReversedWave a_wave)
    {
        m_moveDir += a_wave.Direction * a_wave.GetPower();
    }
}
