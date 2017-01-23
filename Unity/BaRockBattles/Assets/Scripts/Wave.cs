using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wave : MonoBehaviour
{
    // Wave properties
    float m_distance;
    float m_maxDistance;
    float m_startWidth;
    float m_widthSign;
    float m_angle;
    float m_tanAngle;
    float m_minPower;
    float m_maxPower;

    // Other properties
    Vector3   m_previousPos;
    Vector3   m_startPos;
    Vector3   m_dir; // Speed is included in the vector
    Rigidbody m_myRigidBody;
    List<int> m_immunePlayers;

	// Use this for initialization
	public void Init (Vector3 a_dir, int a_spawnerId, float a_startWidth, float a_widthSign, float a_maxDistance, float a_angle, float a_minPower, float a_maxPower)
    {
        m_dir       = a_dir;
        m_maxDistance = a_maxDistance;
        m_startWidth = a_startWidth;
        m_widthSign = a_widthSign;
        m_angle     = a_angle;
        m_minPower  = a_minPower;
        m_maxPower  = a_maxPower;
        m_tanAngle  = Mathf.Tan(m_angle * Mathf.Deg2Rad);
        
        // Init local containers/references
        m_myRigidBody   = GetComponent<Rigidbody>();
        m_immunePlayers = new List<int>();

        // Save player properties
        m_startPos = transform.position;
        m_previousPos = transform.position;
        m_myRigidBody.velocity = m_dir;
        m_immunePlayers.Add(a_spawnerId);

        transform.LookAt(transform.position + a_dir);
        Vector3 scale = transform.localScale;
        scale.x = m_startWidth;
        transform.localScale = scale;



        WaveManager wm = FindObjectOfType<WaveManager>();
        
        float speed = a_dir.magnitude;
        float radius = m_maxDistance;
        float duration = radius / speed;
        wm.AddWave(transform.position, duration, radius * (1.0f/4.5f), true, Direction, 25.0f);
    }

    public void Update()
    {
        m_distance += (transform.position - m_previousPos).magnitude;
        m_previousPos = transform.position;
        if (m_distance > m_maxDistance)
        {
            Destroy(gameObject);
        }

        float opposite = m_distance * m_tanAngle;
        Vector3 scale = transform.localScale;
        scale.x = m_startWidth + opposite * m_widthSign;
        transform.localScale = scale;
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
    public void Redirect(Vector3 a_newDirection)
    {
        m_distance = 0.0f;
        m_startPos = transform.position;
        transform.forward = a_newDirection;
        m_myRigidBody.velocity = a_newDirection * m_myRigidBody.velocity.magnitude;
        m_immunePlayers.Clear();

        WaveManager wm = FindObjectOfType<WaveManager>();

        float speed = m_myRigidBody.velocity.magnitude;
        float radius = m_maxDistance;
        float duration = radius / speed;
        wm.AddWave(transform.position, duration, radius * (1.0f / 4.5f), true, Direction, 25.0f);
    }
    public Vector3 GetForce()
    {
        float scale = Mathf.Clamp(m_distance / m_maxDistance, 0.0f, 1.0f);
        float sign = Mathf.Sign(m_maxPower - m_minPower);
        float power = m_minPower + sign * Mathf.Abs(m_maxPower - m_minPower) * scale;

        Vector3 force = (transform.position - m_startPos).normalized * power;
        return force;
    }
    public Vector3 GetForce(Vector3 a_position)
    {
        float scale = Mathf.Clamp(m_distance / m_maxDistance, 0.0f, 1.0f);
        float sign  = Mathf.Sign(m_maxPower - m_minPower);
        float power = m_minPower + sign * Mathf.Abs(m_maxPower - m_minPower) * scale;
        
        Vector3 force = (a_position - m_startPos).normalized * power;
        return force;
    }
}
