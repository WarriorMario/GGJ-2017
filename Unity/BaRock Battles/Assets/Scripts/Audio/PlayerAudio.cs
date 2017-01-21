using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    public PlayerSound[] m_NormalAttacks;
    public PlayerSound m_SpecialAttack;
    public PlayerSound[] m_Deaths;
    public AudioSource[] m_AudioSources;
    [System.Serializable]
    public class PlayerSound
    {
        public AudioClip audioClip;
        public float volume;
    }
    
    public void PlayNormalAttackSound()
    {
        PlayerSound sound = m_NormalAttacks[Random.Range(0, m_NormalAttacks.Length)];
        m_AudioSources[0].clip = sound.audioClip;
        m_AudioSources[0].volume = sound.volume;
        m_AudioSources[0].Play();
    }

    public void PlaySpecialAttackSound()
    {
        m_AudioSources[1].clip = m_SpecialAttack.audioClip;
        m_AudioSources[1].volume = m_SpecialAttack.volume;
        m_AudioSources[1].Play();
    }

    public void PlayDeathSound()
    {
        PlayerSound sound = m_Deaths[Random.Range(0, m_Deaths.Length)];
        m_AudioSources[2].clip = sound.audioClip;
        m_AudioSources[2].volume = sound.volume;
        m_AudioSources[2].Play();
    }
}
