using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// raymi plz comments
public class PlayerSelection : MonoBehaviour
{
    class Mapping
    {
        public int   cid;
        public int   typeIdx;
        public bool  ready;
        public float delay;
    }

    List<Mapping> m_players  = new List<Mapping>();

    public GameplayVariables m_Vars;
    public GameObject[] m_Selection;
    public Color[] m_PlayerColors;
    public List<Sprite> m_Sprites;
    
    void Update()
    {
        // Join
        int cid = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Confirm);
        if(cid != -1 && !m_players.Exists(x => x.cid == cid))
        {
            Mapping newMap;
            if (m_players.Exists(x => x.cid == -1))
            {
                newMap = m_players.Find(x => x.cid == -1);
            }
            else
            {
                newMap = new Mapping();
            }

            newMap.cid     = cid;
            newMap.typeIdx = 0;
            newMap.ready   = false;
            m_players.Add(newMap);

            int pid = m_players.FindIndex(x => x.cid == newMap.cid);

            Debug.Log("Added pid " + pid + " with cid " + cid);

            GameObject selection = m_Selection[pid];
            selection.SetActive(true);
            selection.GetComponent<Image>().color = m_PlayerColors[pid];
            selection.GetComponent<Image>().sprite = m_Sprites[newMap.typeIdx];
        }

        // Change type
        foreach(Mapping map in m_players)
        {
            if (map.cid == -1) continue;

            int pid = m_players.FindIndex(x => x.cid == map.cid);

            float dir = m_Vars.m_controls.GetPress(InputWrapper.EKeyPairId.EKeyPairId_HorizontalLeft, map.cid);
            int step = 0;
            if (dir - 0.2f > 0)
            {
                step = 1;
            }
            if (dir + 0.2f < 0)
            {
                step = -1;
            }
            if (map.delay + 0.2f < Time.time)
            {
                map.delay = Time.time;
                Debug.Log(pid);
                int newType = map.typeIdx + step;
                if (newType < 0)
                {
                    newType = 3;
                }
                map.typeIdx = (newType) % 4;
                GameObject selection = m_Selection[pid];
                selection.GetComponent<Image>().sprite = m_Sprites[map.typeIdx];
            }
        }

        // Cancel/leave
        cid = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Cancel);
        if (cid != -1)
        {
            if (m_players.Exists(x => x.cid == cid && x.ready))
            {
                Mapping map = m_players.Find(x => x.cid == cid);
                int pid = m_players.FindIndex(x => x.cid == map.cid);

                map.ready = false;

                GameObject selection = m_Selection[pid];
                selection.SetActive(true);
                selection.GetComponent<Image>().color = m_PlayerColors[pid];
            }
            else if(m_players.Exists(x => x.cid == cid))
            {
                Mapping map = m_players.Find(x => x.cid == cid);
                int pid = m_players.FindIndex(x => x.cid == map.cid);

                GameObject selection = m_Selection[pid];
                selection.SetActive(false);
                
                // Mark as free.
                map.cid = -1;
            }
        }
    
        // Start game
        cid = m_Vars.m_controls.GetDownOnAnyController(InputWrapper.EKeyId.EKeyId_Start);
        if (cid != -1 && !m_players.Exists(x => x.cid == cid && x.ready))
        {
            Mapping map = m_players.Find(x => x.cid == cid);
            int pid = m_players.FindIndex(x => x.cid == map.cid);

            map.ready = true;

            GameObject selection = m_Selection[pid];
            selection.SetActive(true);
            selection.GetComponent<Image>().color = m_PlayerColors[pid] *0.8f;
            if (!m_players.Exists(x => !x.ready))
            {
                StartCoroutine(DelayedLoad());
            }
        }
    }


    IEnumerator DelayedLoad()
    {
        foreach(Mapping map in m_players)
        {
            if(map.cid == -1)
            {
                StaticPlayerManager.s_playerChoices.Add((Defines.EPlayerType)map.typeIdx);
            }
        }

        yield return new WaitForSeconds(1);
        // Load game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_Vars.m_ArenaSceneName);
        // thanks
    }
}
