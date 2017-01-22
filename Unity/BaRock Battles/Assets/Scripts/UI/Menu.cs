using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //public GameObject[] m_Menus;
    public GameObject m_CurrentScreen;
    public void ChangeScreen(GameObject newScreen)
    {
        m_CurrentScreen.SetActive(false);
        if (newScreen != null)
        {
            m_CurrentScreen = newScreen;
            m_CurrentScreen.SetActive(true);
        }
    }
}
