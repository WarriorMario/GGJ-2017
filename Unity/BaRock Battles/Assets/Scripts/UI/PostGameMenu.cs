using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameMenu : MonoBehaviour
{
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameLoop.Instance.m_gameplayVariables.m_ArenaSceneName);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameLoop.Instance.m_gameplayVariables.m_MainMenuSceneName);
    }
}
