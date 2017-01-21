using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public GameplayVariables m_gameplayVariables;
    
    private PlayerControl[] m_players;
    public PlayerControl[] Players
    {
        get
        {
            return m_players;
        }
    }

    private static GameLoop ms_instance;
    public static GameLoop Instance
    {
        get
        {
            if (ms_instance != null)
            {
                return ms_instance;
            }
            Debug.LogAssertion("No instance of GameLoop is placed in the editor.");
            Debug.Break();
            return null;
        }
    }

    public PlayerVariables GetPlayerVariables(Defines.EPlayerType a_type)
    {
        switch(a_type)
        {
            case Defines.EPlayerType.heavy:
                return m_gameplayVariables.m_heavy;
            case Defines.EPlayerType.medium:
                return m_gameplayVariables.m_medium;
            case Defines.EPlayerType.light:
                return m_gameplayVariables.m_light;
            case Defines.EPlayerType.strange:
                return m_gameplayVariables.m_strange;
        }
        return new PlayerVariables();
    }
    
	void Awake()
    {
        if (ms_instance != null)
        {
            Debug.LogWarning("Multiple instances of GameLoop are found.");
            return;
        }
        ms_instance = this;

        m_players = FindObjectsOfType<PlayerControl>();

        // ...
    }
	
	void Update()
    {
        // ...
    }
}
