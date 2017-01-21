using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ReversedWave : MonoBehaviour
{

    int m_spawner;
    // speed is included in the vector
    [HideInInspector]
    public Vector3 m_dir;
    Rigidbody m_myRigidBody;
    [HideInInspector]
    public float m_maxLength;
    [HideInInspector]
    public float m_minWidth;
    [HideInInspector]
    public float m_angle;
    [HideInInspector]
    public float m_tanAngle;
    [HideInInspector]
    public float m_minPower;
    [HideInInspector]
    public float m_maxPower;
    Vector3 startPos;

    // Use this for initialization
    public void Init(Vector3 a_dir, int a_spawnerIdx, float a_minWidth, float a_maxLength, float a_angle, float a_minPower, float a_maxPower, Vector3 a_spawnPos)
    {
        m_dir = a_dir;
        m_spawner = a_spawnerIdx;
        m_myRigidBody = GetComponent<Rigidbody>();
        m_myRigidBody.velocity = m_dir; ;
        transform.LookAt(transform.position + a_dir);
        m_maxLength = a_maxLength;
        m_minWidth = a_minWidth;
        m_angle = a_angle;
        m_minPower = a_minPower;
        m_maxPower = a_maxPower;
        m_tanAngle = Mathf.Tan(m_angle * Mathf.Deg2Rad);
        startPos = a_spawnPos;
    }

    public void Update()
    {
        float adjacent = m_maxLength - (transform.position - startPos).magnitude;
        float opposite = adjacent * m_tanAngle;
        Vector3 scale = transform.localScale;
        scale.x = opposite + m_minWidth;
        transform.localScale = scale;
        if (adjacent < 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerControl p = other.GetComponent<PlayerControl>();
            if (p.m_playerIdx != m_spawner)
            {
                p.GetHitByWave(this);
            }
        }
    }

    public float GetPower()
    {
        return (transform.position - startPos).magnitude / m_maxLength * (m_maxPower - m_minPower) + m_minPower;
    }
}
