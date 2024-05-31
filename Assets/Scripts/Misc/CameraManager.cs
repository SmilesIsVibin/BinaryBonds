using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraManager
{
    public static CinemachineFreeLook activeCamera = null;
    public static List<CinemachineFreeLook> cameraList = new List<CinemachineFreeLook>();

    public static bool IsActiveCamera(CinemachineFreeLook freeLookCamera)
    {
        return freeLookCamera == activeCamera;
    }

    public static void SwitchCamera(CinemachineFreeLook cam)
    {
        cam.Priority = 10;
        activeCamera = cam;

        foreach(CinemachineFreeLook c in cameraList)
        {
            if(c != cam && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineFreeLook cam)
    {
        cameraList.Add(cam);
    }

    public static void UnRegister(CinemachineFreeLook cam)
    {
        cameraList.Remove(cam);
    }

}
