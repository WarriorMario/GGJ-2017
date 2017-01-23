using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlayerManager : MonoBehaviour
{
    public static List<PlayerSelection.Mapping> mappings;

    public GameObject[] m_playerPrefabs = new GameObject[4];

    public void SpawnPlayers()
    {
        for (int i = 0; i < mappings.Count; i++)
        {
            Vector3 spawn = transform.GetChild(i).transform.position;
            PlayerControl pc = Instantiate(m_playerPrefabs[mappings[i].typeIdx], spawn, Quaternion.identity).GetComponent<PlayerControl>();
            pc.m_controlId = mappings[i].cid;
        }
    }
}
