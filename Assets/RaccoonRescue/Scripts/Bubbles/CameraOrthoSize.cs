using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrthoSize : MonoBehaviour
{
    bool MaintainWidth = true;
    Vector3 CameraPos;
    float DefaultWidth;
    float DefaultHeight;
    void Start()
    {
        CameraPos = Camera.main.transform.position;

        DefaultWidth = Camera.main.orthographicSize / 1.75f;
        DefaultHeight = Camera.main.orthographicSize;
        if (MaintainWidth)
        {
            Camera.main.orthographicSize = DefaultWidth / Camera.main.aspect;
        }
        Camera.main.transform.position = new Vector3(CameraPos.x, -1 * (DefaultHeight - Camera.main.orthographicSize), CameraPos.z);
    }
}
