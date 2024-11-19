using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TEMP_CameraSwitch : MonoBehaviour
{
    public List<GameObject> cameras;


    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ResetAllCameras(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ResetAllCameras(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ResetAllCameras(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            ResetAllCameras(3);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            ResetAllCameras(4);

    }


    public void ResetAllCameras(int camera)
    {
        for(int i = 0; i < cameras.Count; i++)
        {
            cameras[i].SetActive(false);
        }

        cameras[camera].SetActive(true);
    }
}
