using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Test_CameraShake : BaseBehaviour
{
    [SerializeField] private CinemachineImpulseSource _cineImpulse;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _cineImpulse.GenerateImpulse();
        }
    }
}
