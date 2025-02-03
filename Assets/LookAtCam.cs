using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    Camera _cam => Camera.main;
    private void Update()
    {
        transform.LookAt(_cam.transform.position);
    }
}
