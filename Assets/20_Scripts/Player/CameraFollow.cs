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

    private float CurrentLook = 1.0f;

    [SerializeField] private float PremierPlanCamZ = 10f;
    [SerializeField] private float SecondPlanCamZ = 15f;

    // nouvelle cible interpolée pour la distance caméra
    private float _targetCameraDistance;

    private Vector3 _targetOffset;
    private Vector2 _targetHardLimit;

    private void Awake()
    {
        _player = GetComponent<PlayerCharacter>();
        _health = GetComponent<Health>();
        CurrentLook = DefaultLook;

        // initialiser la target distance sur le premier plan
        _targetCameraDistance = PremierPlanCamZ;
        if (_camera != null)
            _camera.CameraDistance = _targetCameraDistance;
    }

    private void Update()
    {
        float input = _player._movementInput;

        if (input > 0.3f)
        {
            _targetOffset = new Vector3(offsetCam, CurrentLook, 0f);
            _targetHardLimit = new Vector2(-0.7f, 0f);
        }
        else if (input < -0.3f)
        {
            _targetOffset = new Vector3(-offsetCam, CurrentLook, 0f);
            _targetHardLimit = new Vector2(0.7f, 0f);
        }
        else if (input == 0)
        {
            _targetOffset = new Vector3(0f, CurrentLook, 0f);
            _targetHardLimit = Vector2.zero;
        }

        if (_player._isDashing && !_player.IsGrounded || _health._isDying || _cinematique.IsCinematic)
        {
            _camera.Lookahead.Time = 0f;
        }
        else if (!_player._isDashing && _player.IsGrounded)
        {
            _camera.Lookahead.Time = 1f;
        }

        //interpolation lissée de la distance caméra
        if (_camera != null && _player.transform.position.z == 0)
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

    // méthode publique pour que des triggers (CameraChanger) modifient la cible distance
    public void SetTargetCameraDistance(float distance)
    {
        _targetCameraDistance = distance;
    }

    // remet la cible sur le plan par défaut (PremierPlanCamZ)
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

    public void ChangeLook(int direction)
    {
        if(direction > 0)
        {
            CurrentLook = LookUp;
        }
        else if (direction < 0)
        {
            CurrentLook = LookDown;
        }
        else
        {
            CurrentLook = DefaultLook;
        }
    }
}