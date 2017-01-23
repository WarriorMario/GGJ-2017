using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    public PlayerSound[] m_NormalAttacks;
    public PlayerSound   m_SpecialAttack;
    public PlayerSound[] m_Deaths;
    
    List<AudioSource> m_channels = new List<AudioSource>();

    [System.Serializable]
    public class PlayerSound
    {
        public AudioClip audioClip;
        public float     volume;
    }

    bool m_isDetached;
    
    void Awake()
    {
        for (int i = 0; i < Defines.AUDIO_NUMRESERVEDCHANNELS; i++)
        {
            AudioSource newChannel = gameObject.AddComponent<AudioSource>();
            m_channels.Add(newChannel);
        }
    }
    void Update()
    {
        if(m_isDetached)
        {
            // Keep the object as long as there are channels that are playing.
            if(!m_channels.Exists(x => x.isPlaying))
            {
                Destroy(gameObject);
            }
        }
    }
    public void Detach()
    {
        m_isDetached = true;

        transform.parent = new GameObject().transform;
    }
    
    public void PlayNormalAttackSound()
    {
        PlayerSound sound = m_NormalAttacks[Random.Range(0, m_NormalAttacks.Length)];
        PlayAtFirstFreeChannel(sound);
    }

    public void PlaySpecialAttackSound()
    {
        PlayAtFirstFreeChannel(m_SpecialAttack);
    }

    public void PlayDeathSound()
    {
        PlayerSound sound = m_Deaths[Random.Range(0, m_Deaths.Length)];
        PlayAtFirstFreeChannel(sound);
    }

    void PlayAtFirstFreeChannel(PlayerSound a_sound)
    {
        if(m_channels.Exists(x => !x.isPlaying))
        {
            AudioSource channel = m_channels.Find(x => !x.isPlaying);
            channel.clip   = a_sound.audioClip;
            channel.volume = a_sound.volume;
            channel.Play();
        }
    }
}
