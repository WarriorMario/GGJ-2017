using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlayerManager : MonoBehaviour
{
    static List<Defines.EPlayerType> s_playerChoices = new List<Defines.EPlayerType>();

    public GameObject[] m_playerPrefabs = new GameObject[4];

    public void SpawnPlayers()
    {
        if(s_playerChoices.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                s_playerChoices.Add((Defines.EPlayerType)i);
            }
        }

        for(int i = 0; i < s_playerChoices.Count; i++)
        {
            PlayerControl pc = Instantiate(m_playerPrefabs[(int)s_playerChoices[i]], transform.GetChild(i).position, Quaternion.identity).GetComponent<PlayerControl>();
            pc.m_controlId = i + 1;

        }
    }
}
