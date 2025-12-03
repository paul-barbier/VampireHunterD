using UnityEngine;
using Unity.Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachinePositionComposer _camera;
    [SerializeField] private PlayerCharacter _player;

    [Header("Camera Settings")]
    [SerializeField] private float offsetCam = 6.0f;
    [SerializeField] private float camLerpSpeed = 6f;

    private Vector3 _targetOffset;
    private Vector2 _targetHardLimit;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        float input = _player._movementInput;

        if (input > 0.3f)
        {
            _targetOffset = new Vector3(offsetCam, 1f, 0f);
            _targetHardLimit = new Vector2(-0.7f, 0f);
        }
        else if (input < -0.3f)
        {
            _targetOffset = new Vector3(-offsetCam, 1f, 0f);
            _targetHardLimit = new Vector2(0.7f, 0f);
        }
        else if (input == 0)
        {
            _targetOffset = new Vector3(0f, 1f, 0f);
            _targetHardLimit = Vector2.zero;
        }

        _camera.TargetOffset = Vector3.Lerp(_camera.TargetOffset, _targetOffset, Time.deltaTime * camLerpSpeed);

        _camera.Composition.HardLimits.Offset = Vector2.Lerp(_camera.Composition.HardLimits.Offset, _targetHardLimit, Time.deltaTime * camLerpSpeed);
    }

    public void LockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0f);
    }

    public void UnLockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0.6f);
    }
}