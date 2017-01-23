using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public float secondsLeft = 4.0f;
    public Sprite m_3;
    public Sprite m_2;
    public Sprite m_1;
    public Sprite m_go;
    bool m_done = false;

    public Image m_image;

	// Update is called once per frame
	void Update ()
    {
        secondsLeft -= Time.deltaTime;
        if(secondsLeft < 0.0f)
        {
            GameLoop.Instance.StartGame();
            Destroy(gameObject);
        }
        if(secondsLeft < 1.0f)
        {
            m_image.sprite = m_go;
        }
        else if(secondsLeft < 2.0f)
        {
            m_image.sprite = m_1;
        }
        else if(secondsLeft < 3.0f)
        {
            m_image.sprite = m_2;
        }
        else
        {
            m_image.sprite = m_3;
        }
	}
}
