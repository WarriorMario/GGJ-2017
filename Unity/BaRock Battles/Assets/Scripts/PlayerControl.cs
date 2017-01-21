using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputWrapper;


[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public enum PlayerType
    {
        heavy,
        medium,
        light,
        strange
    };
    public PlayerType m_playerType = PlayerType.heavy;
    public int m_playerIdx;

    // player movement variables
    public float m_accelSpeed = 2.0f;
    public float m_drag = 5.0f;
    public float m_shootWaveAccelSpeed = 50.0f;
    CharacterController m_myController;

    // bullet variables
    public float m_waveSpeed = 7.0f;
    public float m_waveMaxPower = 200.0f;
    public float m_waveMinPower = 180.0f;
    public float m_waveAngle = 5.0f;
    public float m_waveMinWidth = 0.1f;
    public float m_waveMaxLength = 10.0f;

    // input variables
    public Scheme m_publicControlScheme;
    Scheme m_controlScheme;

    // bullet spawn variables
    public GameObject m_heavyBulletPrefab;
    public float m_bulletSpawnOffset = 2.0f;

    // current velocity vector
    Vector3 m_moveDir;

    // the last known direction to shoot in
    Vector3 m_shootDir = Vector3.right;


    // player specific variables
    // player heavy
    public GameObject m_1blockPolePrefab;
    public float m_1blockPoleLifeTime = 10.0f;

    // player medium
    public float m_2attackDelayTime = 0.5f;
    float m_2attackDelayTimeSpent = 0.0f;

    // player light
    public float m_3attackDelayTime = 0.5f;
    float m_3attackDelayTimeSpent = 0.0f;

    // player strange
    PlayerControl[] m_4players;
    public GameObject m_4reversedWavePrefab;

    // Use this for initialization
    void Start ()
    {
        m_myController = GetComponent<CharacterController>();
        m_controlScheme = new Scheme(m_playerIdx);
        m_controlScheme.CopyMapping(m_publicControlScheme);
        m_4players = FindObjectsOfType<PlayerControl>();        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 movement = m_controlScheme.GetPressAsAxis(true);
        movement *= m_accelSpeed * Time.deltaTime;
        m_moveDir += new Vector3(movement.x, 0.0f, movement.y);
        m_moveDir -= m_moveDir * m_drag * Time.deltaTime;
        m_myController.SimpleMove(m_moveDir);

        Vector2 dir = m_controlScheme.GetPressAsAxis(false);
        if (dir != Vector2.zero)
        {
            dir.Normalize();
            m_shootDir = new Vector3(dir.x, 0.0f, dir.y);
        }

        switch(m_playerType)
        {
            case PlayerType.heavy:
                if (m_controlScheme.GetDown(EKeyId.EKeyId_Action1))
                {
                    Vector3 shootTemp = m_shootDir;
                    m_shootDir = new Vector3(m_shootDir.z, 0.0f, -m_shootDir.x);
                    StartAttack();
                    m_shootDir = -m_shootDir;
                    StartAttack();
                    m_shootDir = shootTemp;
                }
                if (m_controlScheme.GetDown(EKeyId.EKeyId_Action2))
                {
                    BlockPole p = Instantiate(m_1blockPolePrefab, transform.position, Quaternion.identity).GetComponent<BlockPole>();
                    p.Init(m_1blockPoleLifeTime, m_playerIdx);
                }
                break;
            case PlayerType.medium:
                m_2attackDelayTimeSpent += Time.deltaTime;
                if (m_controlScheme.GetHold(EKeyId.EKeyId_Action1) && m_2attackDelayTimeSpent > m_2attackDelayTime)
                {
                    m_2attackDelayTimeSpent = 0.0f;
                    StartAttack();
                }
                break;
            case PlayerType.light:
                m_3attackDelayTimeSpent += Time.deltaTime;
                if (m_controlScheme.GetHold(EKeyId.EKeyId_Action1) && m_3attackDelayTimeSpent > m_2attackDelayTime)
                {
                    m_3attackDelayTimeSpent = 0.0f;
                    StartAttack();
                }
                break;
            case PlayerType.strange:
                if (m_controlScheme.GetDown(EKeyId.EKeyId_Action1))
                {
                    StartReversedAttack();
                }
                break;
        }
	}

    void StartAttack()
    {
        Vector3 spawnPos = transform.position + m_shootDir * m_bulletSpawnOffset;
        Wave bullet = Instantiate(m_heavyBulletPrefab, spawnPos, Quaternion.identity).GetComponent<Wave>();
        bullet.Init(m_shootDir * m_waveSpeed, m_playerIdx, m_waveMinWidth, m_waveMaxLength, m_waveAngle, m_waveMinPower, m_waveMaxPower, spawnPos);
        m_moveDir -= m_shootDir.normalized * m_shootWaveAccelSpeed;
    }

    void StartReversedAttack()
    {
        Vector3 spawnPos = transform.position + m_shootDir * m_waveMaxLength;
        ReversedWave wave = Instantiate(m_4reversedWavePrefab, spawnPos, Quaternion.identity).GetComponent<ReversedWave>();
        wave.Init(-m_shootDir * m_waveSpeed, m_playerIdx, m_waveMinWidth, m_waveMaxLength, m_waveAngle, m_waveMinPower, m_waveMaxPower, spawnPos);
        m_moveDir += m_shootDir.normalized * m_shootWaveAccelSpeed;
    }

    public void GetHitByWave(Wave a_wave)
    {
        m_moveDir += a_wave.m_dir.normalized * a_wave.GetPower();
    }
    public void GetHitByWave(ReversedWave a_wave)
    {
        m_moveDir += a_wave.m_dir.normalized * a_wave.GetPower();
    }

    public void GetHitByDeathPlane()
    {
        Destroy(gameObject);
    }
}
