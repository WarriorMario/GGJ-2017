using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public Material m_Mat;
    
    List<Vector4> m_ConeData = new List<Vector4>();
    List<Vector4> m_ConeMoreData = new List<Vector4>();
    List<SoundWave> m_Cones;
    public int m_NumWaves;
	// Use this for initialization
	void Start ()
    {
        // We need a constructor for this to spawn it
        m_Cones = new List<SoundWave>();
        SoundWave cone = new SoundWave();
        cone.m_Pos = new Vector2(0.0f, 0.5f);
        cone.m_CosAngle = Mathf.Cos(22.5f * Mathf.Deg2Rad);
        cone.m_Angle = 0 * Mathf.Deg2Rad;
        cone.m_Length = 0.1f;
        cone.m_LifeTime = 5.0f;
        cone.m_Speed = 0.01f;
        m_Cones.Add(cone);
        SoundWave cone2 = new SoundWave();
        cone2.m_Pos = new Vector2(0.0f, 0.0f);
        cone2.m_CosAngle = Mathf.Cos(22.5f * Mathf.Deg2Rad);
        cone2.m_Angle = 0 * Mathf.Deg2Rad;
        cone2.m_Length = 0.1f;
        cone2.m_LifeTime = 5.0f;
        cone2.m_Speed = 0.01f;
        m_Cones.Add(cone2);
        m_NumWaves += 2;
    }

    void FixedUpdate ()
    {
        UpdateConeData();
        SetConeData();
	}

    void UpdateConeData()
    {
        for(int i = 0; i < m_NumWaves; ++i)
        {
            if(m_Cones[i].IsDead())
            {
                m_Cones[i] = m_Cones[m_NumWaves - 1];
                --m_NumWaves;
            }
            m_Cones[i].Move();
        }
    }

    void SetConeData()
    {

        // Update num
        m_Mat.SetInt("_Cones_Length", m_NumWaves);
        if(m_NumWaves == 0)
        {
            return;
        }
        m_ConeData.Clear();
        for(int i = 0; i < m_NumWaves; ++i)
        {
            m_ConeData.Add(new Vector4(m_Cones[i].m_Pos.x, m_Cones[i].m_Pos.y, m_Cones[i].m_Angle, m_Cones[i].m_CosAngle));
        }
        m_ConeMoreData.Clear();
        for (int i = 0; i < m_NumWaves; ++i)
        {
            m_ConeMoreData.Add(new Vector4(m_Cones[i].m_Length, 0,0,0));
        }
        m_Mat.SetVectorArray("_Cones", m_ConeData);
        m_Mat.SetVectorArray("_ConesMore", m_ConeMoreData);

    }

    public class SoundWave
    {
        public Vector2 m_Pos;
        public float m_Angle;
        public float m_CosAngle;
        public float m_Length;
        public float m_Speed;
        public float m_LifeTime;
        public float m_StartTime;
        public void StartWave()
        {
            m_StartTime = Time.time;
        }
        public bool IsDead()
        {
            return (m_StartTime + m_LifeTime < Time.time);
        }
        public void Move()
        {
            m_Length += m_Speed;
        }
        //public bool PointInCone(Vector2 point)
        //{
        //    // Distance check
        //    Vector2 p = point - pos;
        //    float distance = p.magnitude;
        //    if (distance > length * size && distance<length)
        //    {
        //        Vector2 pDir = p.normalized;
        //        if(Vector2.Dot(pDir,dir) > cosAngle)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
