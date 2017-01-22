using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlayerManager : MonoBehaviour
{
    static List<Defines.EPlayerType> s_playerChoices = new List<Defines.EPlayerType>();

    public GameObject[] m_playerPrefabs = new GameObject[4];

    public void SpawnPlayers()
    {
        for(int i = 0; i < 4; i++)
        {
            s_playerChoices.Add((Defines.EPlayerType)i);
        }

        for(int i = 0; i < s_playerChoices.Count; i++)
        {
            Instantiate(m_playerPrefabs[(int)s_playerChoices[i]]);
        }
    }
}
