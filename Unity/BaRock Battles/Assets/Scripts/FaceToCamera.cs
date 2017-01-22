using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour {
    

	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
