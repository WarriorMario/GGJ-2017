using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ReversedWave : MonoBehaviour
{
    // Wave properties
    public float m_maxLength;
    public float m_minWidth;
    public float m_angle;
    public float m_tanAngle;
    public float m_minPower;
    public float m_maxPower;

    // Other properties
    Vector3 m_startPos;
    Vector3 m_dir; // Speed is included in the vector
    Rigidbody m_myRigidBody;
    List<int> m_immunePlayers;

    // Use this for initialization
    public void Init(Vector3 a_dir, int a_spawnerIdx, float a_minWidth, float a_maxLength, float a_angle, float a_minPower, float a_maxPower)
    {
        m_dir       = a_dir;
        m_maxLength = a_maxLength;
        m_minWidth  = a_minWidth;
        m_angle     = a_angle;
        m_minPower  = a_minPower;
        m_maxPower  = a_maxPower;
        m_tanAngle  = Mathf.Tan(m_angle * Mathf.Deg2Rad);

        // Init local containers/references
        m_myRigidBody = GetComponent<Rigidbody>();
        m_immunePlayers = new List<int>();

        // Save player properties
        m_startPos             = transform.position;
        m_myRigidBody.velocity = m_dir;
        m_immunePlayers.Add(a_spawnerIdx);

        transform.LookAt(transform.position + a_dir);

        m_myRigidBody = GetComponent<Rigidbody>();
        m_myRigidBody.velocity = m_dir;
        transform.LookAt(transform.position + a_dir);
    }

    public void Update()
    {
        float adjacent = m_maxLength - (transform.position - m_startPos).magnitude;
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
            if(!m_immunePlayers.Exists(x => x == p.m_controlId))
            {
                p.GetHitByWave(this);
                m_immunePlayers.Add(p.m_controlId);
            }
        }
    }

    public Vector3 Direction
    {
        get { return m_myRigidBody.velocity.normalized; }
    }
    public void Bounce()
    {
        m_myRigidBody.velocity = -m_myRigidBody.velocity;
        m_immunePlayers.Clear();
    }
    public float GetPower()
    {
        float distanceTraveled = (transform.position - m_startPos).magnitude;
        float scale = Mathf.Max(distanceTraveled / m_maxLength, 1.0f);
        float sign  = Mathf.Sign(m_maxPower - m_minPower);
        float power = m_minPower + sign * Mathf.Abs(m_maxPower - m_minPower) * scale;
        return power;
    }
}
