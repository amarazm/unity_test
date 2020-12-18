using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{

public class CameraFacing 
{
    public unitFacing _facing;
    private Camera theCam;
    
    private static  CameraFacing instance = new CameraFacing();

    public static CameraFacing Instance {
        get { return instance; }
    }

    private CameraFacing()     {    }

    public unitFacing getFacing( ) 
    {
        if (!theCam) {
            theCam = Camera.main;
        }

       return  Utilities.GetFacing(Mathf.Abs(theCam.transform.eulerAngles.y));

    }
        
     public float GetAngle()
        {
            if (!theCam)
            {
                theCam = Camera.main;
            }
            return theCam.transform.eulerAngles.y;
        }


}

}