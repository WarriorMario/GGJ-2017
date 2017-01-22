using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    public GameObject m_FillerThingyOfTheHealthBar;
    public const float m_fullWidth = 0.75f;
    public const float m_fullHeight = 0.4f;

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        m_FillerThingyOfTheHealthBar.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
    }

    public void UpdateBar(float value)
    {
        value = Mathf.Clamp(value, 0.0f, 1.0f);
        m_FillerThingyOfTheHealthBar.transform.localScale = new Vector3(m_fullWidth * value, m_fullHeight, 1.0f);
        float xPos = -m_fullWidth * 0.5f + (m_fullWidth * 0.5f * value);
        Vector3 pos = m_FillerThingyOfTheHealthBar.transform.localPosition;
        pos.x = xPos;
        m_FillerThingyOfTheHealthBar.transform.localPosition = pos;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!gameObject.activeSelf)
            return;
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
