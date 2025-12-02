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
    [SerializeField] private float offsetCam = 0.0f;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (_player._movementInput >= 1)
        {
            if (_camera != null)
            {
                _camera.TargetOffset = new Vector3(offsetCam, 1.0f, 0.0f);
                _camera.Composition.HardLimits.Offset = new Vector2(-1f, 0f);
            }
        }
        else if (_player._movementInput <= -1)
        {
            if (_camera != null)
            {
                _camera.TargetOffset = new Vector3(-offsetCam, 1.0f, 0.0f);
                _camera.Composition.HardLimits.Offset = new Vector2(1f, 0f);
            }
        }
        else if (_player._movementInput == 0)
        {
            if (_camera != null)
            {
                _camera.TargetOffset = new Vector3(0.0f, 1.0f, 0.0f);
            }
        }
    }

    public void LockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0f);
    }

    public void UnLockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0.45f);

    }
}
