using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSystem : Singleton<CameraSystem>
{
    private Camera mainCam;

    protected override void Awake()
    {
        base.Awake();

        mainCam = GetComponent<Camera>();
    }

    /// <summary>
    /// Returns this camera's position.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCamPosition()
    {
        return transform.position;
    }
}
