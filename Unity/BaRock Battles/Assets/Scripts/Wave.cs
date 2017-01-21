using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wave : MonoBehaviour
{
    // Wave properties
    float m_maxLength;
    float m_minWidth;
    float m_angle;
    float m_tanAngle;
    float m_minPower;
    float m_maxPower;

    // Other properties
    Vector3   m_startPos;
    Vector3   m_dir; // Speed is included in the vector
    Rigidbody m_myRigidBody;
    List<int> m_immunePlayers;

	// Use this for initialization
	public void Init (Vector3 a_dir, int a_spawnerIdx, float a_minWidth, float a_maxLength, float a_angle, float a_minPower, float a_maxPower)
    {
        m_dir       = a_dir;
        m_maxLength = a_maxLength;
        m_minWidth  = a_minWidth;
        m_angle     = a_angle;
        m_minPower  = a_minPower;
        m_maxPower  = a_maxPower;
        m_tanAngle  = Mathf.Tan(m_angle * Mathf.Deg2Rad);
        
        // Init local containers/references
        m_myRigidBody   = GetComponent<Rigidbody>();
        m_immunePlayers = new List<int>();
        
        // Save player properties
        m_startPos = transform.position;
        m_myRigidBody.velocity = m_dir;
        m_immunePlayers.Add(a_spawnerIdx);

        transform.LookAt(transform.position + a_dir);
    }

    public void Update()
    {
        float adjacent = (transform.position - m_startPos).magnitude;
        float opposite = adjacent * m_tanAngle;
        Vector3 scale = transform.localScale;
        scale.x = opposite + m_minWidth;
        transform.localScale = scale;
        if(adjacent > m_maxLength)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerControl p = other.GetComponent < PlayerControl >();
            if (!m_immunePlayers.Exists(x => x == p.m_controlId))
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
        float powerScale       = Mathf.Max(distanceTraveled / m_maxLength, 1.0f);
        float power            = m_minPower + (m_maxPower - m_minPower) * powerScale;
        return power;
    }
}
