using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// raymi plz comments
public class PlayerSelection : MonoBehaviour
{
    // raymi plz comments
    public GameplayVariables m_Vars;
    // raymi plz comments
    public List<int> m_PlayerIDs;
    // raymi plz comments
    public List<int> m_LockedPlayers;
    // raymi plz comments
    public GameObject[] m_Selection;
    // raymi plz comments
    public Color[] m_PlayerColors;
    // raymi plz comments
    void Start()
    {
        // raymi plz comments
        m_PlayerIDs = new List<int>();
        // raymi plz comments
        m_PlayerIDs.Add(-1);
        // raymi plz comments
        m_PlayerIDs.Add(-1);
        // raymi plz comments
        m_PlayerIDs.Add(-1);
        // raymi plz comments
        m_PlayerIDs.Add(-1);
        // raymi plz comments
        m_LockedPlayers = new List<int>();
        // raymi plz comments
        m_LockedPlayers.Add(-1);
        // raymi plz comments
        m_LockedPlayers.Add(-1);
        // raymi plz comments
        m_LockedPlayers.Add(-1);
        // raymi plz comments
        m_LockedPlayers.Add(-1);
    }
    void Update()
    {
        // raymi plz comments
        int playerID = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Confirm);
        // raymi plz comments
        if (playerID!=-1&&m_PlayerIDs.Contains(playerID)==false)
        {
            // raymi plz comments
            m_PlayerIDs[m_PlayerIDs.IndexOf(-1)] = playerID;
            // raymi plz comments
            int playerIndex = m_PlayerIDs.IndexOf(playerID);
            // raymi plz comments
            GameObject selection = m_Selection[playerIndex];
            // raymi plz comments
            selection.SetActive(true);
            // raymi plz comments
            selection.GetComponent<Image>().color = m_PlayerColors[playerIndex];
        }
        // raymi plz comments
        playerID = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Cancel);
        // raymi plz comments
        if (playerID!=-1)
        {
            // raymi plz comments
            if (m_LockedPlayers.Contains(playerID))
            {
                // raymi plz comments
                int playerIndex = m_PlayerIDs.IndexOf(playerID);
                // raymi plz comments
                m_LockedPlayers[m_LockedPlayers.IndexOf(playerID)] = -1;
                // raymi plz comments
                GameObject selection = m_Selection[playerIndex];
                // raymi plz comments
                selection.SetActive(true);
                // raymi plz comments
                selection.GetComponent<Image>().color = m_PlayerColors[playerIndex];
            }
            else if(m_PlayerIDs.Contains(playerID))
            {
                // raymi plz comments
                int playerIndex = m_PlayerIDs.IndexOf(playerID);
                // raymi plz comments
                m_PlayerIDs[m_PlayerIDs.IndexOf(playerID)] = -1;
                // raymi plz comments
                GameObject selection = m_Selection[playerIndex];
                // raymi plz comments
                selection.SetActive(false);
            }

        }
        // raymi plz comments
        playerID = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Start);
        // raymi plz comments
        if (playerID!=-1&& m_LockedPlayers.Contains(playerID) == false)
        {
            // raymi plz comments
            m_LockedPlayers[m_LockedPlayers.IndexOf(-1)] = playerID;
            // raymi plz comments
            int playerIndex = m_PlayerIDs.IndexOf(playerID);
            // raymi plz comments
            GameObject selection = m_Selection[playerIndex];
            // raymi plz comments
            selection.SetActive(true);
            // raymi plz comments
            selection.GetComponent<Image>().color = m_PlayerColors[playerIndex] *0.8f;
            // raymi plz comments
            if (m_LockedPlayers.FindAll(x=>x.Equals(-1)).Count == m_PlayerIDs.FindAll(x => x.Equals(-1)).Count)
            {
                // raymi plz comments
                StartCoroutine(DelayedLoad());
            }
        }
    }

    // raymi plz comments
    IEnumerator DelayedLoad()
    {
        // raymi plz comments
        yield return new WaitForSeconds(1);
        // Load game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_Vars.m_ArenaSceneName);
        // thanks
    }
}
