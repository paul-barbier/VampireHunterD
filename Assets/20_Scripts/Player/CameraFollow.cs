using UnityEngine;
using Unity.Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public CinemachinePositionComposer _camera;
    public CinemachineCamera _cinemachine;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private Cinematique _cinematique;
    private Health _health;


    [Header("Camera Settings")]
    [SerializeField] private float offsetCam = 6.0f;
    [SerializeField] private float camLerpSpeed = 6f;

    [SerializeField] private float DefaultLook = 1.0f;
    [SerializeField] private float LookUp = 1.0f;
    [SerializeField] private float LookDown = 1.0f;
    [SerializeField] private float LookRight = 1.0f;
    [SerializeField] private float LookLeft = 1.0f;

    private float CurrentLookHorizontal = 1.0f;
    private float CurrentLookVertical = 1.0f;

    [SerializeField] private float PremierPlanCamZ = 10f;
    [SerializeField] private float SecondPlanCamZ = 15f;

    private float _targetCameraDistance;

    private Vector3 _targetOffset;
    private Vector2 _targetHardLimit;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
        _health = GetComponent<Health>();
        CurrentLookVertical = DefaultLook;

        _targetCameraDistance = PremierPlanCamZ;
        if (_camera != null)
            _camera.CameraDistance = _targetCameraDistance;
    }

    private void Update()
    {
        float input = _player._movementInput;

        if (input > 0.3f)
        {
            _targetOffset = new Vector3(offsetCam + CurrentLookHorizontal, CurrentLookVertical, 0f);
            _targetHardLimit = new Vector2(-0.7f, 0f);
        }
        else if (input < -0.3f)
        {
            _targetOffset = new Vector3(-offsetCam - CurrentLookHorizontal, CurrentLookVertical, 0f);
            _targetHardLimit = new Vector2(0.7f, 0f);
        }
        else if (input == 0)
        {
            _targetOffset = new Vector3(0f, CurrentLookVertical, 0f);
            _targetHardLimit = Vector2.zero;
        }

        if (_player._isDashing && !_player.IsGrounded || _health._isDying ||_player._isCinematic)
        {
            _camera.Lookahead.Time = 0f;
        }
        else if (!_player._isDashing && _player.IsGrounded)
        {
            _camera.Lookahead.Time = 1f;
        }

        if (_camera != null && _player.transform.position.z <= 1)
        {
            _camera.CameraDistance = Mathf.Lerp(_camera.CameraDistance, PremierPlanCamZ, Time.deltaTime * camLerpSpeed);
        }
        else
        {
            _camera.CameraDistance = Mathf.Lerp(_camera.CameraDistance, SecondPlanCamZ, Time.deltaTime * camLerpSpeed);
        }

        _camera.TargetOffset = Vector3.Lerp(_camera.TargetOffset, _targetOffset, Time.deltaTime * camLerpSpeed);

        _camera.Composition.HardLimits.Offset = Vector2.Lerp(_camera.Composition.HardLimits.Offset, _targetHardLimit, Time.deltaTime * camLerpSpeed);
    }

    public void SetTargetCameraDistance(float distance)
    {
        _targetCameraDistance = distance;
    }

    public void ResetTargetCameraDistance()
    {
        _targetCameraDistance = PremierPlanCamZ;
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

    public void ChangeLook(CameraDirection direction)
    {
        //if(direction > 0)
        //{
        //    CurrentLookVertical = LookUp;
        //}
        //else if (direction < 0)
        //{
        //    CurrentLookVertical = LookDown;
        //}
        //else
        //{
        //    CurrentLookVertical = DefaultLook;
        //}
        switch (direction)
        {
            case CameraDirection.HAUT: CurrentLookVertical = LookUp; break;
            case CameraDirection.BAS: CurrentLookVertical = LookDown; break;
            case CameraDirection.DROITE: CurrentLookHorizontal = LookRight; break;
            case CameraDirection.GAUCHE: CurrentLookHorizontal = LookLeft; break;
            default:
                CurrentLookVertical = DefaultLook;
                CurrentLookHorizontal = 0;
                break;
        }
    
    }
}