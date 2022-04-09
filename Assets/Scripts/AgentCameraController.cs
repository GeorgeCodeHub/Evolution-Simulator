using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCameraController : MonoBehaviour
{
    public void OnMouseDown()
    {
        CameraController.instance.followTransform = transform;
    }
}
