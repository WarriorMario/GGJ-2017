using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{ 
    public GameplayVariables m_Vars;
    public List<int> m_PlayerIDs;
    public List<int> m_LockedPlayers;
    public GameObject[] m_Selection;
    public Color[] m_PlayerColors;
    void Start()
    {
        m_PlayerIDs = new List<int>();
        m_PlayerIDs.Add(-1);
        m_PlayerIDs.Add(-1);
        m_PlayerIDs.Add(-1);
        m_PlayerIDs.Add(-1);
        m_LockedPlayers = new List<int>();
        m_LockedPlayers.Add(-1);
        m_LockedPlayers.Add(-1);
        m_LockedPlayers.Add(-1);
        m_LockedPlayers.Add(-1);
    }
    void Update()
    {
        int playerID = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Confirm);
        if (playerID!=-1&&m_PlayerIDs.Contains(playerID)==false)
        {
            m_PlayerIDs[m_PlayerIDs.IndexOf(-1)]=(playerID);
            int playerIndex = m_PlayerIDs.IndexOf(playerID);
            GameObject selection = m_Selection[playerIndex];
            selection.SetActive(true);
            selection.GetComponent<Image>().color = m_PlayerColors[playerIndex];
        }
        playerID = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Cancel);
        if(playerID!=-1)
        {
            if(m_LockedPlayers.Contains(playerID))
            {
                int playerIndex = m_PlayerIDs.IndexOf(playerID);
                m_LockedPlayers[m_LockedPlayers.IndexOf(playerID)] = -1;
                GameObject selection = m_Selection[playerIndex];
                selection.SetActive(true);
                selection.GetComponent<Image>().color = m_PlayerColors[playerIndex];
            }
            else if(m_PlayerIDs.Contains(playerID))
            {
                int playerIndex = m_PlayerIDs.IndexOf(playerID);
                m_PlayerIDs[m_PlayerIDs.IndexOf(playerID)] = -1;
                GameObject selection = m_Selection[playerIndex];
                selection.SetActive(false);
            }
            
        }
        playerID = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Start);
        if(playerID!=-1&& m_LockedPlayers.Contains(playerID) == false)
        {
            m_LockedPlayers[m_LockedPlayers.IndexOf(-1)] = playerID;
            int playerIndex = m_PlayerIDs.IndexOf(playerID);
            GameObject selection = m_Selection[playerIndex];
            selection.SetActive(true);
            selection.GetComponent<Image>().color = m_PlayerColors[playerIndex] *0.8f;
            if(m_LockedPlayers.FindAll(x=>x.Equals(-1)).Count == m_PlayerIDs.FindAll(x => x.Equals(-1)).Count)
            {
                StartCoroutine(DelayedLoad());
            }
        }
    }

    IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(1);
        // Load game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_Vars.m_ArenaSceneName);
    }
}
