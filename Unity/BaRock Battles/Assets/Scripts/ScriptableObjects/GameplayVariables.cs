using UnityEngine;
using InputWrapper;


[System.Serializable]
public class PlayerVariables
{
    public float m_movementAccelerationSpeed;
    public float m_movementDrag;

    public float m_attackDelayTime;
    public float m_attackAccelerationSpeed;

    public float m_waveSpeed;
    public float m_waveMaxDistance;
    public float m_waveMinPower;
    public float m_waveMaxPower;
    [Range(0.0f, 360.0f)] public float m_waveAngle;
    public float m_waveMinWidth;
}

[System.Serializable]
public class HeavyVariables : PlayerVariables
{
    public GameObject m_blockPolePrefab;
    public float      m_blockPoleLifeTime;
    public float      m_blockPoleSpawnOffset;
    public float      m_blockPoleCooldown;
}
[System.Serializable]
public class MediumVariables : PlayerVariables
{
    // ...
}
[System.Serializable]
public class LightVariables : PlayerVariables
{
    public float m_cloneSpawnOffset;
    public float m_cloneLifetime;
    public float m_cloneCooldown;
}
[System.Serializable]
public class StrangeVariables : PlayerVariables
{
    public float m_waveSpawnOffset;
    public float m_dodgePower;
    public float m_dodgeCooldown;
}


public class GameplayVariables : ScriptableObject
{
    public string m_ArenaSceneName;
    public string m_MainMenuSceneName;

    [Space()]

    [Header("General settings")]
    public Scheme     m_controls;
    public GameObject m_wavePrefab;
    public GameObject m_heavyPrefab;
    public GameObject m_mediumPrefab;
    public GameObject m_lightPrefab;
    public GameObject m_strangePrefab;

    [Space()]

    [Header("Player type specific")]
    public HeavyVariables   m_heavy;
    public MediumVariables  m_medium;
    public LightVariables   m_light;
    public StrangeVariables m_strange;
}
