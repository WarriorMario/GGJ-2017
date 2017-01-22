using UnityEngine;
using System.Collections.Generic;
using InputWrapper;


public class GameLoop : MonoBehaviour
{
    public GameplayVariables m_gameplayVariables;
    
    private List<PlayerControl> m_players = new List<PlayerControl>();
    public List<PlayerControl> Players
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

        m_players.AddRange(FindObjectsOfType<PlayerControl>());

        // ...
    }
	
	void Update()
    {
        Scheme controls = m_gameplayVariables.m_controls;

        int controllerId = controls.GetDownOnAnyController(EKeyId.EKeyId_Confirm);
        if (controllerId != -1)
        {
            Debug.Log("Controller " + controllerId + " registered");
        }

        // ...
    }

    void OnGUI()
    {
        Rect rect = new Rect(0.0f, 25.0f, 250.0f, 50.0f);
        rect.x = Screen.width / 2.0f - rect.width / 2.0f;

        if (m_players.Count == 1)
        {
            GUI.TextArea(rect, "Player " + m_players[0].m_playerType.ToString() + " won the game");

            // End game
        }
        else
        {
            GUI.TextArea(rect, m_players.Count + " players left");
        }
    }

    public void NotifyPlayerDeath(PlayerControl a_player)
    {
        if (m_players.Exists(x => x.m_controlId == a_player.m_controlId))
        {
            m_players.Remove(a_player);
        }
        if(m_players.Count==1)// Last man standing
        {
            GameObject menu = GameObject.Find("PostGameMenu");
            if(menu!=null)
            {
                menu.SetActive(true);
            }
            else
            {
                // Reload scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(m_gameplayVariables.m_ArenaSceneName);
            }
        }
    }
}
