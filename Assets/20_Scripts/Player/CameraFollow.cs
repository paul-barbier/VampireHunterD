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

    [SerializeField] private float horizontalOffset = 4f;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetOffsetX = 4f;
    private float currentOffsetX = 10f;
    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (_player._movementInput > 0.1f)
            targetOffsetX = horizontalOffset;
        else if (_player._movementInput < -0.1f)
            targetOffsetX = -horizontalOffset;
        else
            targetOffsetX = 0f;

        currentOffsetX = Mathf.Lerp(currentOffsetX, targetOffsetX, Time.deltaTime * smoothSpeed);

        _camera.TargetOffset = new Vector3(currentOffsetX, _camera.TargetOffset.y, 0);
    }

    public void LockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0f);
        _camera.Composition.HardLimits.Size = new Vector2(1f, 0.46f);
        _camera.Lookahead.IgnoreY = true;
    }

    public void UnLockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0.45f);
        _camera.Lookahead.IgnoreY = false;
    }
}
