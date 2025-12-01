using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private PlayerCharacter _player = null;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        var composer = _camera.GetComponent<CinemachinePositionComposer>();
        if (_player._movementInput == 1)
        {
            if (composer != null)
            {
                composer.TargetOffset = new Vector3(8.0f, 0.0f, 0.0f);
            }
        }
        else if (_player._movementInput == -1)
        {
            if (composer != null)
            {
                composer.TargetOffset = new Vector3(-8.0f, 0.0f, 0.0f);
            }
        }
    }

    public void LockCamOnPlayer()
    {
        var composer = _camera.GetComponent<CinemachinePositionComposer>();

        composer.CenterOnActivate = true;
    }

}
