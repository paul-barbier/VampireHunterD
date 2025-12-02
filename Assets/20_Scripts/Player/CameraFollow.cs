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
    [SerializeField] private float offsetCam = 2.0f;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (_camera == null || _player == null)
            return;

        float velocityX = _player._rigidbody.linearVelocity.x;

        if (Mathf.Abs(velocityX) > 0.1f)
        {
            float lookDir = Mathf.Sign(velocityX);
            _camera.TargetOffset = new Vector3(lookDir * offsetCam, 1f, 0f);
        }
        else
        {
            _camera.TargetOffset = new Vector3(0f, 1f, 0f);
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
