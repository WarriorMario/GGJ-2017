using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    int frame = 0;
    void Update()
    {
        if(frame == 54)
        {
            return;
        }
        transform.GetChild(frame).gameObject.SetActive(false);
        ++frame;
        transform.GetChild(frame).gameObject.SetActive(true);
    }
}
