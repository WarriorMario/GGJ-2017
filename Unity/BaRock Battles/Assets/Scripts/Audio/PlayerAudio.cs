using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    public PlayerSound[] m_NormalAttacks;
    public PlayerSound m_SpecialAttack;
    [System.Serializable]
    public class PlayerSound
    {
        public AudioClip audioClip;
        public float volume;
    }
}
