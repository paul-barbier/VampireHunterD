using UnityEngine;
using Unity.Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachinePositionComposer _camera;
    public CinemachineCamera _cinemachine;
    [SerializeField] private PlayerCharacter _player;
    private Health _health;


    [Header("Camera Settings")]
    [SerializeField] private float offsetCam = 6.0f;
    [SerializeField] private float camLerpSpeed = 6f;

    [SerializeField] private float PremierPlanCamZ = 10f;
    [SerializeField] private float SecondPlanCamZ = 15f;

    private Vector3 _targetOffset;
    private Vector2 _targetHardLimit;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
        _health = GetComponent<Health>();
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

        if (_player._isDashing && !_player.IsGrounded || _health._isDying)
        {
            _camera.Lookahead.Time = 0f;
        }
        else if (!_player._isDashing && _player.IsGrounded)
        {
            _camera.Lookahead.Time = 1f;
        }

        if (_player.transform.position.z == 0)
        {
            _camera.CameraDistance = 10f;
            //_camera.CameraDistance = Mathf.Lerp(10, 15, Time.deltaTime * camLerpSpeed);
        }
        if (_player.transform.position.z == 10)
        {
            _camera.CameraDistance = 15f;
            //_camera.CameraDistance = Mathf.Lerp(15, 10, Time.deltaTime * camLerpSpeed);
        }

        _camera.TargetOffset = Vector3.Lerp(_camera.TargetOffset, _targetOffset, Time.deltaTime * camLerpSpeed);

        _camera.Composition.HardLimits.Offset = Vector2.Lerp(_camera.Composition.HardLimits.Offset, _targetHardLimit, Time.deltaTime * camLerpSpeed);
    }

    public void LockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0f);
        _camera.Lookahead.IgnoreY = true;
        _camera.Lookahead.Smoothing = 6;
    }

    public void UnLockCamOnPlayer()
    {
        _camera.Composition.DeadZone.Size = new Vector2(0.6f, 0.6f);
        _camera.Lookahead.IgnoreY = true;
        _camera.Lookahead.Smoothing = 6;
    }

    public void CamFalling()
    {
        _camera.Lookahead.IgnoreY = false;
        _camera.Lookahead.Smoothing = 7;
    }
}