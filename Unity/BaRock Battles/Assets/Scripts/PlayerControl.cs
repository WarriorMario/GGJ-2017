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
            transform.forward = new Vector3(dir.x, 0.0f, dir.y).normalized;
        }

        m_attackDelayTimeSpent += Time.deltaTime;
        if (controls.GetDown(EKeyId.EKeyId_Action1, m_controlId) && m_attackDelayTimeSpent >= playerVars.m_attackDelayTime)
        {
            PerformAction1();

            m_attackDelayTimeSpent = 0.0f;
        }
        if (controls.GetDown(EKeyId.EKeyId_Action2, m_controlId))
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
            case Defines.EPlayerType.heavy:
                FireWave(gameVars.m_wavePrefab, transform.position, -transform.right);
                FireWave(gameVars.m_wavePrefab, transform.position,  transform.right);
                break;
            case Defines.EPlayerType.medium:
            case Defines.EPlayerType.light:
                FireWave(gameVars.m_wavePrefab, transform.position, transform.forward);
                break;
            case Defines.EPlayerType.strange: // A.k.a didgeridoo guy
                FireWave(gameVars.m_wavePrefab, transform.position + transform.forward * gameVars.m_strange.m_waveSpawnOffset, - transform.forward);
                break;
        }
    }

    //////////////////////////////
    // Action 2
    //////////////////////////////
    void PerformAction2()
    {
        GameplayVariables gameVars   = GameLoop.Instance.m_gameplayVariables;
        PlayerVariables   playerVars = GameLoop.Instance.GetPlayerVariables(m_playerType);

        switch (m_playerType)
        {
            case Defines.EPlayerType.heavy:
                {
                    HeavyVariables heavyVars = gameVars.m_heavy;
                    
                    BlockPole p = Instantiate(heavyVars.m_blockPolePrefab, transform.position + transform.forward * heavyVars.m_blockPoleSpawnOffset, transform.rotation).GetComponent<BlockPole>();
                    p.Init(heavyVars.m_blockPoleLifeTime, m_controlId);
                }
                break;
            case Defines.EPlayerType.medium:
                {
                    MediumVariables mediumVars = gameVars.m_medium;

                    // ...

                    break;
                }
            case Defines.EPlayerType.light:
                {
                    LightVariables lightVars = gameVars.m_light;

                    // ...

                    break;
                }
            case Defines.EPlayerType.strange: // A.k.a didgeridoo guy
                {
                    StrangeVariables strangeVars = gameVars.m_strange;
                    
                    // ...

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
        m_moveDir += a_wave.Direction * a_wave.GetPower();
    }
}
