using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class MobileUserDetectionWebgl
{
    [DllImport("__Internal")]
    private static extern int IsMobileDevice();


    
    public bool IsMobileUser()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (IsMobileDevice() == 1) return true;
        }
        return false;
    }
    
}
