using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera vCamera;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vCamera.Priority = 100;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vCamera.Priority = 0;
        }
    }
}
