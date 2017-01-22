using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlayerManager : MonoBehaviour
{
    public static List<Defines.EPlayerType> s_playerChoices = new List<Defines.EPlayerType>();
    public static List<Color> s_playerColors = new List<Color>();

    public GameObject[] m_playerPrefabs = new GameObject[4];

    public void SpawnPlayers()
    {
        for(int i = 0; i < s_playerChoices.Count; i++)
        {
            Instantiate(m_playerPrefabs[(int)s_playerChoices[i]]);
        }
    }
}
