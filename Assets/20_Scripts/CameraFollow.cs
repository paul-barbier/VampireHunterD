using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private PlayerCharacter _playerCam = null;
    [SerializeField] private float RunTimer;

    private void Awake()
    {
        _playerCam = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        RunTimer += Time.deltaTime;

        var composer = _camera.GetComponent<CinemachinePositionComposer>();
        if (_playerCam._movementInput == 1)
        {
            if (composer != null)
            {
                composer.TargetOffset = new Vector3(8.0f, 0.0f, 0.0f);
            }
        }
        else if (_playerCam._movementInput == -1)
        {
            if (composer != null)
            {
                composer.TargetOffset = new Vector3(-8.0f, 0.0f, 0.0f);
            }
        }
        else if (_playerCam._movementInput == 0)
        {
            RunTimer = 0.0f;
        }

    }
}
