using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachinePositionComposer _camera;
    [SerializeField] private PlayerCharacter _player = null;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (_player._movementInput == 1)
        {
            if (_camera != null)
            {
                _camera.TargetOffset = new Vector3(8.0f, 0.0f, 0.0f);
            }
        }
        else if (_player._movementInput == -1)
        {
            if (_camera != null)
            {
                _camera.TargetOffset = new Vector3(-8.0f, 0.0f, 0.0f);
            }
        }
    }

    public void LockCamOnPlayer()
    {
        _camera.CenterOnActivate = true;
    }

    public void UnLockCamOnPlayer()
    {
        _camera.CenterOnActivate = false;
    }
}
