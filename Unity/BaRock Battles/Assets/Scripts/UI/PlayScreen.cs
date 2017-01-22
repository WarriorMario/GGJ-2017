using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScreen : MonoBehaviour
{
    public GameplayVariables m_Vars;
    public GameObject m_PlayerSelectionScreen;
    public Menu m_Menu;
	// Update is called once per frame
	void Update()
    {
		if(m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Confirm)!=-1)
        {
            m_Menu.ChangeScreen(m_PlayerSelectionScreen);
        }
	}
}
