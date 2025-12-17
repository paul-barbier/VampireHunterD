using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerCharacter;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour
{

    #region DataStructure

    public enum PhysicState
    {
        Ground,
        Air
    }

    [Serializable]
    private struct MovementValues
    {
        public float MaxSpeed;
        public float Acceleration;
        public float MaxAcceleration;
        [Tooltip("Range [-1, 1]")] public AnimationCurve AccelerationRemapFromVelocityDot;
    }

    [Serializable]
    private struct GravityValues
    {
        public float MaxForce;
        public float Acceleration;
        public float MaxAcceleration;
        public float CoyoteTime;
        [Tooltip("Range [0, 1]")] public AnimationCurve GravityRemapFromCoyoteTime;
    }

    [Serializable]
    private struct JumpValues
    {
        public float ImpulseForce;
        public float Deceleration;
        public float MaxDeceleration;
        [Tooltip("Range [0, 1]")] public AnimationCurve DecelerationFromAirTime;
        public float Height;
        public float Length;
        public float BufferTime;
        public float Bounciness;
        [HideInInspector] public float InitValue;
        [HideInInspector] public float GravityValue;
    }

    [Serializable]
    private struct DashValues
    {
        public float DashImpulseForce;
        public float DashDuration;
        public float DashBufferTime;
    }

    [Serializable]
    private struct KnockBackValues
    {
        public Vector3 _knockbackDirection;
        public float _knockbackForce;
        public float _knockbackDeceleration;
        [Tooltip("Range [0, 1]")] public AnimationCurve DecelerationFromKnockBack;
        public float durationKnockback;
    }

    #endregion DataStructure

    #region EditorVariables

    [Header("Gameplay")]
    [SerializeField] private MovementValues _groundPhysic = new MovementValues();
    [SerializeField] private MovementValues _airPhysic = new MovementValues();
    [SerializeField] private GravityValues _gravityParameters = new GravityValues();
    [SerializeField] private JumpValues _jumpParameters = new JumpValues();
    [SerializeField] private DashValues _dashParameters = new DashValues();
    [SerializeField] private KnockBackValues _knockbackValues = new KnockBackValues();
    [SerializeField] private ContactFilter2D _groundContactFilter = new ContactFilter2D();
    [SerializeField] private ContactFilter2D _ceilingContactFilter = new ContactFilter2D();

    [Header("Setup")]
    public Transform _mesh = null;
    [SerializeField] public GameObject ChauveSourisD;
    public Rigidbody2D _rigidbody = null;
    [SerializeField] public Animator _DAnimation;
    [SerializeField] private UnityEvent PlaySound;
    [SerializeField] private UnityEvent PlayMobDeath;

    [Header("Dash Cursor (visual only)")]
    [SerializeField] private GameObject _dashCursor = null;
    [SerializeField] private float _cursorDistance = 1.5f;
    [SerializeField] private float _cursorDeadzone = 0.2f;
    [SerializeField] private float _cursorRotationOffset = -90f; // ajustable pour orienter correctement votre sprite flèche
    [SerializeField] private float _cursorLerpSpeed = 12f; // vitesse d'interpolation (ajustable)
    [SerializeField] private ParticleSystem Land;

    #endregion EditorVariables

    #region Variables

    [Header("Ref Script")]
    [SerializeField] private Attack _attack;
    [SerializeField] private Health _health;
    [SerializeField] private CameraFollow cameraFollow;

    //Force
    public Vector2 _forceToAdd = Vector2.zero;
    private Vector2 _prePhysicPosition = Vector2.zero;

    //Horizontal movement
    public Vector2 _currentHorizontalVelocity = Vector2.zero;
    public float _movementInput = 0.0f;
    private MovementValues _horizontalPhysic = new MovementValues();

    //Gravity
    private float _currentGravity = 0.0f;

    //Ground
    public bool IsGrounded = true;

    //Air
    private float _airTime = 0.0f;
    private float _dashAirTime = 0.0f;
    private bool _isInCoyoteTime = false;
    private float _chuteTime = 0.0f;
    public bool _isFalling = false;

    [Header("Jump")]
    private Vector2 _currentJumpForce = Vector2.zero;
    public bool _isJumping = false;
    private float _jumpTime = 0.0f;
    private float _startJumpTime = 0.0f;
    private bool _bufferJump = false;
    private bool _hasBounce = false;

    //Event appel� quand on touche ou quitte le sol
    public event Action<PhysicState> OnPhysicStateChanged;

    [Header("Dash")]
    public Vector2 _currentDashForce = Vector2.zero;
    private Vector2 _dashMovementInput = Vector2.zero;
    [SerializeField] public bool _canDash = true;
    public bool _isDashing = false;
    private float _startDashTime = 0.0f;
    private bool _bufferDash = false;
    [SerializeField] public bool _hittingDash = false;

    // last meaningful direction for the visual cursor (does not affect dash logic)
    private Vector2 _lastDashDirection = Vector2.zero;
    // current interpolated direction used to draw the cursor smoothly
    private Vector2 _cursorCurrentDirection = Vector2.right;

    [Header("Bounce")]
    [SerializeField] private Vector2 enemyBounceForce;
    [SerializeField] private float BouncingTime;

    [Header("Knockback")]
    [SerializeField] private Vector3 targetKnockback = Vector3.zero;
    private Collider2D _enemyCollider;
    [SerializeField] private bool _isKnockBacked = false;

    //Sprite
    public Vector3 _currentMeshRotation = Vector3.zero;
    private float rotationSpeed = 8000f;
    [SerializeField] public bool _rotatePlayer = false;

    //Disable movement
    public bool _movementDisabled = false;

    //COLLIDER
    //PLAYER
    [SerializeField] private CapsuleCollider2D _capsuleBox;
    private Vector2 _sizeCapsule;
    private Vector2 _offsetCapsule;
    //ATTACK
    [SerializeField] public BoxCollider2D attackHitbox;
    //DASH
    [SerializeField] public BoxCollider2D dashHitbox;
    private Vector2 _sizeDashHitbox;
    private Vector2 _offsetDashHitbox;

    //CINEMATIQUE
    public bool _isCinematic = false;

    #endregion Variables

    #region Initialization

    private void Awake()
    {
        _attack = GetComponent<Attack>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleBox = GetComponent<CapsuleCollider2D>();
        _sizeCapsule = _capsuleBox.size;
        _offsetCapsule = _capsuleBox.offset;

        _horizontalPhysic = _groundPhysic;
        CalculateJumpTime();

        //On enregistre le changement de physic � l'event qui detecte le changement d'�tat du sol
        OnPhysicStateChanged += ChangePhysic;
        OnPhysicStateChanged += ResetGravity;
        OnPhysicStateChanged += CancelJump;
        OnPhysicStateChanged += TryJumpBuffer;
        OnPhysicStateChanged += TryDashBuffer;

        dashHitbox.gameObject.SetActive(false);
        _isKnockBacked = false;
        _isCinematic = false;
        // activer le curseur visuel si assigné
        if (_dashCursor != null)
            _dashCursor.SetActive(true);

        // initialiser la direction courante du curseur (évite jump dynamiques à la 1re frame)
        if (_lastDashDirection.sqrMagnitude > 0.0001f)
            _cursorCurrentDirection = _lastDashDirection.normalized;
        else
        {
            bool facingRight = true;
            if (_mesh != null)
                facingRight = (_mesh.localScale.x >= 0f);
            _cursorCurrentDirection = facingRight ? Vector2.right : Vector2.left;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        CalculateJumpTime();
    }
#endif

    private void CalculateJumpTime()
    {
        _jumpTime = _jumpParameters.Height / _jumpParameters.ImpulseForce;

        //_maxJumpParameters.InitValue = InitSpeed(_groundPhysic.MaxSpeed, _maxJumpParameters.Height, _maxJumpParameters.Length);
        //_maxJumpParameters.GravityValue = GravityValue(_groundPhysic.MaxSpeed, _maxJumpParameters.Height, _maxJumpParameters.Length);

        //_jumpParameters.InitValue = InitSpeed(_groundPhysic.MaxSpeed, _jumpParameters.Height, _jumpParameters.Length);
        //_jumpParameters.GravityValue = GravityValue(_groundPhysic.MaxSpeed, _jumpParameters.Height, _jumpParameters.Length);
    }

    #endregion Initialization

    #region Visual

    private void Update()
    {
        UpdateDashCursor();
        RotateMesh();
    }

    private void RotateMesh()
    {
        if (!_rotatePlayer || _isDashing || _movementDisabled)
            return;

        float targetRotation = _movementInput >= 0.01 ? 0f : _movementInput <= -0.01 ? 180f : _currentMeshRotation.y;

        _currentMeshRotation.y = Mathf.MoveTowards(_currentMeshRotation.y, targetRotation, rotationSpeed * Time.deltaTime);

        _mesh.rotation = Quaternion.Euler(0, _currentMeshRotation.y, 0);
    }

    private void UpdateDashCursor()
    {
        if (_dashCursor == null)
            return;

        // Toujours visible 
        if (!_dashCursor.activeSelf)
            _dashCursor.SetActive(true);

        // Déterminer direction cible : priorité remappée (_dashMovementInput), sinon la dernière direction du stick brut
        Vector2 targetDir = Vector2.zero;

        if (_dashMovementInput.sqrMagnitude > 0.0001f && _dashMovementInput.magnitude >= _cursorDeadzone)
        {
            targetDir = _dashMovementInput.normalized;
        }
        else if (_lastDashDirection.sqrMagnitude > 0.0001f)
        {
            targetDir = _lastDashDirection.normalized;
        }
        else
        {
            // fallback : facing via localScale.x (plus courant pour flip sprite)
            bool facingRight = true;
            if (_mesh != null)
                facingRight = (_mesh.localScale.x >= 0f);
            targetDir = facingRight ? Vector2.right : Vector2.left;
        }

        // Interpolation exponentielle (frame-rate indépendant) pour un feeling fluide
        float t = 1f - Mathf.Exp(-_cursorLerpSpeed * Time.deltaTime);
        _cursorCurrentDirection = Vector2.Lerp(_cursorCurrentDirection, targetDir, t);
        if (_cursorCurrentDirection.sqrMagnitude > 0.000001f)
            _cursorCurrentDirection.Normalize();

        // positionner et orienter le curseur
        Vector3 targetPos = transform.position + (Vector3)(_cursorCurrentDirection * _cursorDistance);
        _dashCursor.transform.position = targetPos;

        float angle = Mathf.Atan2(_cursorCurrentDirection.y, _cursorCurrentDirection.x) * Mathf.Rad2Deg;
        _dashCursor.transform.rotation = Quaternion.Euler(0, 0, angle + _cursorRotationOffset);
    }

    #endregion Visual

    #region Update
    private void FixedUpdate()
    {
        //On reset la force à ajouter cette boucle de fixed update
        _forceToAdd = Vector2.zero;
        _prePhysicPosition = _rigidbody.position;

        //Fonction qui détecte si on touche le sol ou non
        //Et appelle les events associés
        GroundDetection();
        ManageAirTime();
        ManageCoyoteTime();

        //On effectue tous les calculs physiques
        Movement();

        Gravity();
        JumpForce();
        Dash();


        //On ajoute la force au rigidbody
        _rigidbody.linearVelocity += _forceToAdd;
    }

    private void LateUpdate()
    {
        if (_prePhysicPosition == _rigidbody.position && _forceToAdd != Vector2.zero)
        {
            _rigidbody.linearVelocity = new Vector2(0.0f, _rigidbody.linearVelocity.y);
            _currentHorizontalVelocity.x = 0.0f;
        }
    }
    #endregion Update

    #region PhysicState

    private void GroundDetection()
    {
        //On utilise le filtre qui contient l'inclinaison du sol pour savoir si le rigidbody touche le sol ou non
        ContactFilter2D filter = _groundContactFilter;
        bool isTouchingGround = _rigidbody.IsTouching(filter);

        //Si le rigidbody touche le sol mais on a en m�moire qu'il ne le touche pas, on est sur la frame o� il touche le sol
        if (isTouchingGround && !IsGrounded)
        {

            Instantiate(Land, new Vector3(_mesh.transform.position.x, _mesh.transform.position.y - 7, _mesh.transform.position.z), Quaternion.identity);
            if (!_canDash)
            {
                _canDash = true;
                ChauveSourisD.gameObject.SetActive(true);
                StartCoroutine(StopAuraAfterDelay());
            }

            IsGrounded = true;
            _isFalling = false;

            //On invoque l'event en passant true pour signifier que le joueur arrive au sol
            OnPhysicStateChanged.Invoke(PhysicState.Ground);
            cameraFollow.LockCamOnPlayer();
        }
        //Si le rigidbody ne touche pas le sol mais on a en m�moire qu'il le touche, on est sur la frame o� il quitte le sol
        else if (!isTouchingGround && IsGrounded)
        {
            IsGrounded = false;
            if (!_isJumping || !_isDashing)
                _isInCoyoteTime = true;
            //On invoque l'event en passant false pour signifier que le joueur quitte au sol
            OnPhysicStateChanged.Invoke(PhysicState.Air);

            if (!_isDashing || !_isFalling)
            {
                cameraFollow.UnLockCamOnPlayer();
            }
        }
    }

    private void ManageAirTime()
    {
        if (!IsGrounded)
        {
            _airTime += Time.fixedDeltaTime;
        }

        if (_isDashing)
            _dashAirTime += Time.fixedDeltaTime;
    }

    private void ManageCoyoteTime()
    {
        if (_airTime > _gravityParameters.CoyoteTime)
            _isInCoyoteTime = false;
    }

    private void ChangePhysic(PhysicState groundState)
    {
        //On change la physique en fonction de si le joueur est au sol ou non
        if (groundState == PhysicState.Ground)
            _horizontalPhysic = _groundPhysic;
        else if (groundState == PhysicState.Air)
            _horizontalPhysic = _airPhysic;
    }

    #endregion PhysicState

    #region HorizontalMovement

    public void GetMovementInput(float input)
    {
        _movementInput = input;
    }

    private void Movement()
    {
        if (_movementDisabled || _isDashing || _health._isDying)
            return;

        //Vector2 maxSpeed = new Vector2(_horizontalPhysic.MaxSpeed * _movementInput, 0.0f);
        Vector2 maxSpeed = SnapToGround(_movementInput);
        float velocityDot = Mathf.Clamp(Vector2.Dot(_rigidbody.linearVelocity, maxSpeed), -1.0f, 1.0f);
        velocityDot = _horizontalPhysic.AccelerationRemapFromVelocityDot.Evaluate(velocityDot);
        float acceleration = _horizontalPhysic.Acceleration * velocityDot * Time.fixedDeltaTime;

        //On fait avancer notre vitesse actuelle vers la max speed en fonction de l'acceleration
        _currentHorizontalVelocity = Vector2.MoveTowards(_currentHorizontalVelocity, maxSpeed, acceleration);

        //On calcul l'�cart entre la velocit� actuelle du rigidbody et la v�locit� cible
        Vector2 velocityDelta = _currentHorizontalVelocity - _rigidbody.linearVelocity;
        if (_currentHorizontalVelocity.y == 0.0f)
            velocityDelta.y = 0.0f;

        //On clamp le delta de velocite avec l'acceleration maximum en negatif et positif pour eviter des bugs dans la physic
        velocityDelta = Vector2.ClampMagnitude(velocityDelta, _horizontalPhysic.MaxAcceleration);

        //On a ajoute le delta de v�locit� � la force � donn� ce tour de boucle au rigidbody
        _forceToAdd += velocityDelta;

        if (_movementInput >= 0.01 && IsGrounded)
        {
            _DAnimation.SetBool("IsRunning", true);
            //_capsuleBox.offset = new Vector2(1, 0.3f);
        }
        else if (_movementInput <= -0.01 && IsGrounded)
        {
            _DAnimation.SetBool("IsRunning", true);
            //_capsuleBox.offset = new Vector2(-1, 0.3f);

        }
        else if (_movementInput == 0 && IsGrounded || _movementInput != 0 && !IsGrounded)
        {
            _DAnimation.SetBool("IsRunning", false);
            //_capsuleBox.offset = _offsetCapsule;
        }
    }

    private Vector2 SnapToGround(float input)
    {
        //Fix : on passait ici la premi�re frame du saut, ce qui appliquait la force vers le bas pour snap � la pente
        if (!IsGrounded || _isJumping)
            return new Vector2(input * _horizontalPhysic.MaxSpeed, 0.0f);

        //Fix : Filtrer les points de contacts avec le sol uniquement
        ContactPoint2D[] contactPointArray = new ContactPoint2D[1];
        ContactFilter2D filter = _groundContactFilter;
        _rigidbody.GetContacts(filter, contactPointArray);
        Vector2 normal = contactPointArray.Length > 0 ? contactPointArray[0].normal : Vector2.zero;

        //Si on est en l'air ou sur un sol plat, on revoit la force normalement
        if (normal == Vector2.zero || (normal == Vector2.up) || (normal == Vector2.down) || input == 0.0f)
            return new Vector2(input * _horizontalPhysic.MaxSpeed, 0.0f);

        Vector3 force = Vector3.zero;

        //On effectue un cross product avec la normal et la 3�me dimension pour obtenir une force parall�le au sol
        if (input > 0.0f)
            force = Vector3.Cross(normal, Vector3.forward);
        else
            force = Vector3.Cross(normal, Vector3.back);

        return _horizontalPhysic.MaxSpeed * force;
    }
    public void DisableAllMovement()
    {
        _movementDisabled = true;
        _isDashing = false;
        _isJumping = false;
        _rotatePlayer = false;
        _currentDashForce = Vector2.zero;
        _currentJumpForce = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;
        _forceToAdd = Vector2.zero;
    }


    #endregion HorizontalMovement

    #region Gravity

    private void Gravity()
    {
        if (IsGrounded || _isJumping || _isDashing || _hittingDash)
            return;

        _chuteTime += Time.deltaTime;

        float coyoteTimeRatio = Mathf.Clamp01(_airTime / _gravityParameters.CoyoteTime);
        float coyoteTimeFactor = _isInCoyoteTime ? _gravityParameters.GravityRemapFromCoyoteTime.Evaluate(coyoteTimeRatio) : 1.0f;
        float acceleration = _jumpParameters.Deceleration * coyoteTimeFactor * Time.fixedDeltaTime;

        float maxGravityForce = _gravityParameters.MaxForce;
        _currentGravity = Mathf.MoveTowards(_currentGravity, maxGravityForce, acceleration);

        float velocityDelta = _currentGravity - _rigidbody.linearVelocity.y;

        velocityDelta = Mathf.Clamp(velocityDelta, -_gravityParameters.MaxAcceleration, 0.0f);

        if (!_isDashing || !_isKnockBacked)
        {
            _DAnimation.SetBool("IsFalling", true);
        }

        if (_chuteTime >= 0.3f && !_isDashing)
        {
            cameraFollow.CamFalling();
            _isFalling = true;
        }

        _forceToAdd.y += velocityDelta;
    }

    private void ResetGravity(PhysicState physicState)
    {
        if (physicState != PhysicState.Air)
        {
            _currentGravity = 0.0f;
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0.0f);
            _airTime = 0.0f;
            _dashAirTime = 0.0f;
            _chuteTime = 0.0f;
        }
    }

    #endregion Gravity

    #region Jump

    public void StartJump()
    {

        if (_movementDisabled) 
            return;

        if ((!IsGrounded && !_isInCoyoteTime) || _isJumping)
        {
            _bufferJump = true;
            Invoke(nameof(StopJumpBuffer), _jumpParameters.BufferTime);
            return;
        }
        cameraFollow.LockCamOnPlayer();
        _DAnimation.SetBool("IsJumping", true);
        PlaySound.Invoke();
        //_currentJumpForce.y = _jumpParameters.InitValue;
        _currentJumpForce.y = _jumpParameters.ImpulseForce;
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _currentJumpForce.y);
        _currentHorizontalVelocity.y = 0.0f;
        _isJumping = true;
        _isInCoyoteTime = false;
        _startJumpTime = _airTime;
        _hasBounce = false;
    }

    private void StopJumpBuffer()
    {
        _bufferJump = false;
    }

    private void JumpForce()
    {
        if (!_isJumping || _health._isDying)
            return;
        _capsuleBox.offset = new Vector2(0, 0.5f);

        ContactPoint2D[] contactPointArray = new ContactPoint2D[1];
        ContactFilter2D filter = _ceilingContactFilter;
        _rigidbody.GetContacts(filter, contactPointArray);
        Vector2 normal = contactPointArray.Length > 0 ? contactPointArray[0].normal : Vector2.zero;

        _currentJumpForce = GetBounceForce(_currentJumpForce, normal, _jumpParameters.Bounciness, ref _hasBounce);

        float jumpTimeRatio = Mathf.Clamp01((_airTime - _startJumpTime) / _jumpTime);
        float deceleration = _jumpParameters.Deceleration * _jumpParameters.DecelerationFromAirTime.Evaluate(jumpTimeRatio) * Time.fixedDeltaTime;
        //float deceleration = _jumpParameters.GravityValue * Time.fixedDeltaTime;

        _currentJumpForce = Vector2.MoveTowards(_currentJumpForce, Vector2.zero, deceleration);
        //_currentJumpForce += Vector2.down * _jumpParameters.GravityValue * Time.fixedDeltaTime;

        Vector2 velocityDelta = _currentJumpForce - _rigidbody.linearVelocity;
        if (_currentJumpForce.x == 0.0f)
            velocityDelta.x = 0.0f;
        velocityDelta = Vector2.ClampMagnitude(velocityDelta, _jumpParameters.MaxDeceleration);

        _forceToAdd += velocityDelta;

        if (jumpTimeRatio >= 1.0f)
        {
            _isJumping = false;
            _DAnimation.SetBool("IsJumping", false);

            _capsuleBox.offset = _offsetCapsule;

            _currentJumpForce = Vector2.zero;
        }
        if (jumpTimeRatio <= 1.0f && _isDashing)
        {
            _isJumping = false;
            _DAnimation.SetBool("IsJumping", false);

            _capsuleBox.offset = _offsetCapsule;

            _currentJumpForce = _currentDashForce;
        }
    }

    private Vector2 GetBounceForce(Vector2 initialForce, Vector2 normal, float bouciness, ref bool hasBounce)
    {
        if (!hasBounce && normal != Vector2.zero && normal.y < 0)
        {
            float dot = Vector2.Dot(initialForce, normal);
            Vector2 projectedVector = -2 * dot * normal;
            Vector2 bounceForce = bouciness * (projectedVector + initialForce);
            hasBounce = true;
            return bounceForce;
        }
        return initialForce;
    }

    private void CancelJump(PhysicState state)
    {
        if (state != PhysicState.Air)
        {
            _isJumping = false;
            _currentJumpForce = Vector2.zero;
            _DAnimation.SetBool("IsJumping", false);
            _DAnimation.SetBool("IsFalling", false);
        }
    }

    private void TryJumpBuffer(PhysicState state)
    {
        if (state != PhysicState.Air && _bufferJump)
        {
            StartJump();
            _bufferJump = false;
            CancelInvoke(nameof(StopJumpBuffer));
        }
    }

    #endregion Jump

    #region Dash
    public void GetDashInput(Vector2 Dashinput)
    {
        float scalaire = Vector2.Dot(Vector2.up, Dashinput);
        float step = Mathf.Floor((Mathf.Abs(scalaire) + 0.25f) * 2) / 2f;
        float Mx = step > 0.5f ? 0 : Mathf.Sign(Dashinput.x) * (1 - step);
        _dashMovementInput = (new Vector2(Mx, step * Mathf.Sign(scalaire))).normalized;

        if (Dashinput.sqrMagnitude >= (_cursorDeadzone * _cursorDeadzone) && Dashinput.sqrMagnitude > 0.0001f)
        {
            _lastDashDirection = Dashinput.normalized;
        }
    }

    public void StartDash()
    {
        if (!_isInCoyoteTime && _isDashing || !_canDash || _health._isDying)
        {
            _bufferDash = true;
            Invoke(nameof(StopDashBuffer), _dashParameters.DashBufferTime);
            return;
        }

        if (_canDash || _health._isDying)
        {
            _isDashing = true;
            _canDash = false;
            cameraFollow._camera.Lookahead.IgnoreY = true;
            cameraFollow.LockCamOnPlayer();

            _chuteTime = 0.0f;

            if (_dashMovementInput.y == 1)
            {
                _DAnimation.SetBool("IsDashingUp", true);
            }
            else if (_dashMovementInput.y != 0 && _dashMovementInput.x != 0)
            {
                _rotatePlayer = false;

                float angle = (Mathf.Atan2(_dashMovementInput.y, _dashMovementInput.x) * Mathf.Rad2Deg) % 90;

                _mesh.rotation = Quaternion.Euler(0, _mesh.localEulerAngles.y, angle);

                _DAnimation.SetBool("IsDashing", true);
            }
            else if (_dashMovementInput.x != 0)
            {
                _DAnimation.SetBool("IsDashing", true);
            }
            else if (_dashMovementInput.y == -1)
            {
                _DAnimation.SetBool("IsDashingDown", true);
            }

            _currentDashForce = _dashMovementInput.normalized * _dashParameters.DashImpulseForce;

            _rigidbody.linearVelocity = Vector2.zero;
            _currentJumpForce = Vector2.zero;
            _forceToAdd = Vector2.zero;

            _startDashTime = Time.time;
            ChauveSourisD.SetActive(false);
            SoundManager.PlaySound(SoundType.Dash, 7.0f);
        }
    }

    public void Dash()
    {
        if (!_isDashing && !_canDash)
            return;

        float elapsed = Time.time - _startDashTime;

        if (elapsed < _dashParameters.DashDuration)
        {
            _forceToAdd += _currentDashForce;
            dashHitbox.gameObject.SetActive(true);
        }
        else
        {
            dashHitbox.gameObject.SetActive(false);

            _isDashing = false;
            _currentDashForce = Vector2.zero;
            _DAnimation.SetBool("IsDashing", false);
            _DAnimation.SetBool("IsDashingUp", false);
            _DAnimation.SetBool("IsDashingDown", false);

            _rotatePlayer = true;
            //_movementDisabled = false;

            if (IsGrounded && !_canDash)
            {
                StartCoroutine(CdDash());
            }
        }
    }
    private void StopDashBuffer()
    {
        _bufferDash = false;
    }

    private void TryDashBuffer(PhysicState state)
    {
        if (_bufferDash)
        {
            StartDash();
            _bufferDash = false;
            CancelInvoke(nameof(StopDashBuffer));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isDashing = false;
        _currentJumpForce = Vector2.zero;
        _DAnimation.SetBool("IsDashing", false);
        _DAnimation.SetBool("IsDashingUp", false);
        _DAnimation.SetBool("IsDashingDown", false);
    }

    IEnumerator CdDash()
    {
        yield return new WaitForSeconds(1.0f);
        _canDash = true;
        ChauveSourisD.gameObject.SetActive(true);
        _aura.Play();
        yield return new WaitForSeconds(0.5f);
        _aura.Stop();
    }

    public void BounceOnEnemy()
    {
        StartCoroutine(BounceTime());
    }

    public void StopDashOnEnemy(Collider2D enemy)
    {
        _currentDashForce = Vector2.zero;
        _currentHorizontalVelocity = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;
        _forceToAdd = Vector2.zero;
        _currentGravity = 0.0f;
        _DAnimation.SetBool("IsDashing", false);
        _DAnimation.SetBool("IsDashingUp", false);
        _DAnimation.SetBool("IsDashingDown", false);
    }

    IEnumerator BounceTime()
    {
        _hittingDash = true;
        _rigidbody.AddForce(enemyBounceForce, ForceMode2D.Impulse);
        _isJumping = false;
        _currentGravity = 0f;
        yield return new WaitForSeconds(BouncingTime);
        _hittingDash = false;
    }

    #endregion Dash

    #region Damage/Die
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemyCollider = collision;

        //Attack ennemi
        if (collision.CompareTag("AttackZone") && !_hittingDash && !_attack.isAttacking)
        {
            if (_health._isInvincible ||_isDashing)
                return;

            _health.TakeDamage(25);
            Knockback(collision);
            return;
        }
        //Dash sur mob
        if (collision.CompareTag("Dash") && _isDashing && collision != dashHitbox)
        {
            StopDashOnEnemy(collision);
            BounceOnEnemy();
            ChauveSourisD.gameObject.SetActive(true);
            _canDash = true;
            PlayMobDeath.Invoke();
            KillingEnemy(collision);
            StartCoroutine(StopAuraAfterDelay());
            return;
        }
        //Dash sur cadavre
        if (collision.CompareTag("Cadavre") && _isDashing && collision != dashHitbox)
        {
            StopDashOnEnemy(collision);
            BounceOnEnemy();
            if (_currentMeshRotation.y >= 0f && _currentMeshRotation.y != 180.0f)
            {
                Instantiate(DaBloodyBlood, collision.transform.position, Quaternion.Euler(0, 0, 0));
            }
            if (_currentMeshRotation.y >= 180.0f)
            {
                Instantiate(DaBloodyBlood180, collision.transform.position, Quaternion.Euler(0, 0, 0));
            }
            //Time.timeScale = 0.2f;
            ChauveSourisD.gameObject.SetActive(true);
            StartCoroutine(StopAuraAfterDelay());
            _canDash = true;
            collision.enabled = false;
            StartCoroutine(CadavreHitboxCD(collision));
            return;
        }
    }

    IEnumerator CadavreHitboxCD(Collider2D collision)
    {
        yield return new WaitForSeconds(1);
        collision.enabled = true;
    }

    public void Knockback(Collider2D enemy)
    {
        _isKnockBacked = true;
        _DAnimation.SetBool("IsKnockbacked", true);

        StopDashOnEnemy(enemy);
        _movementDisabled = true;

        float sign = transform.position.x < enemy.transform.position.x ? -1f : 1f;

        if (_mesh.localEulerAngles.y < 90)
            sign = -Mathf.Abs(sign);
        else
            sign = Mathf.Abs(sign);

        targetKnockback = new Vector2(_knockbackValues._knockbackDirection.x * sign, _knockbackValues._knockbackDirection.y).normalized;

        StartCoroutine(KnockBackTime());
    }

    IEnumerator KnockBackTime()
    {
        float t = 0f;
        float duration = _knockbackValues.durationKnockback;

        _currentHorizontalVelocity = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;

        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = t / duration;

            float curveValue = _knockbackValues.DecelerationFromKnockBack.Evaluate(ratio);

            float deceleration = _knockbackValues._knockbackDeceleration * _jumpParameters.DecelerationFromAirTime.Evaluate(ratio) * Time.fixedDeltaTime;
            targetKnockback = Vector2.MoveTowards(targetKnockback, Vector2.zero, deceleration);

            _rigidbody.AddForce(targetKnockback * _knockbackValues._knockbackForce, ForceMode2D.Impulse);

            yield return null;
        }

        _movementDisabled = false;

        _isKnockBacked = false;
        _DAnimation.SetBool("IsKnockbacked", false);
    }

 

    private static readonly int _dissolveID = Shader.PropertyToID("_Dissolve");
    [SerializeField] private ParticleSystem DaBloodyBlood;
    [SerializeField] private ParticleSystem DaBloodyBlood180;
    private IEnumerator DissolveAndDisable(SpriteRenderer sprite, Collider2D dash, Collider2D attackEnnemi, RespawnManager rm, float duration = 1f)
    {
        if (sprite == null)
            yield break;

        // Utiliser sprite.material pour s'assurer d'instancier le matériau si nécessaire
        Material mat = sprite.material;
        mat.SetFloat(_dissolveID, 0f);
        sprite.enabled = true;
        if (dash != null) dash.enabled = false;
        if (attackEnnemi != null) attackEnnemi.enabled = false;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float value = Mathf.Clamp01(elapsed / duration);
            mat.SetFloat(_dissolveID, value);
            yield return null;
        }

        mat.SetFloat(_dissolveID, 1f);

        // Désactiver visuel et hitboxes après la dissolution
        sprite.enabled = false;

        if (rm != null)
            rm.RespawnFonction();
    }
    [SerializeField] private ParticleSystem _aura;
    private IEnumerator StopAuraAfterDelay()
    {
        _aura.Play();
        yield return new WaitForSeconds(0.5f);
        _aura.Stop();
    }
    public void KillingEnemy(Collider2D collision)
    {
        RespawnManager rm = collision.transform.root.GetComponentInChildren<RespawnManager>(true);
        SpriteRenderer sprite = collision.GetComponent<SpriteRenderer>();
        Collider2D dash = collision.GetComponent<Collider2D>();
        Collider2D attackEnnemi = collision.transform.Find("AttackZone").GetComponent<Collider2D>();
        Debug.Log("RespawnManager trouvé = " + (rm != null));

        if (_currentMeshRotation.y >= 0f && _currentMeshRotation.y != 180.0f)
        {
            Instantiate(DaBloodyBlood, collision.transform.position, Quaternion.Euler(0, 0, 0));
        }
        if (_currentMeshRotation.y >= 180.0f)
        {
            Instantiate(DaBloodyBlood180, collision.transform.position, Quaternion.Euler(0, 0, 0));
        }

        StartCoroutine(DissolveAndDisable(sprite, dash, attackEnnemi, rm, 0.5f));
    }
    #endregion Damage/Die

    public void EnterCinematicMode()
    {
        _movementDisabled = true;

        _isDashing = false;
        _isJumping = false;
        _hittingDash = false;
        _attack.isAttacking = false;

        _rotatePlayer = false;

        _isCinematic = true;

        _currentDashForce = Vector2.zero;
        _currentJumpForce = Vector2.zero;
        _currentHorizontalVelocity = Vector2.zero;
        _forceToAdd = Vector2.zero;

        _rigidbody.linearVelocity = Vector2.zero;

        // --- Animator : tout couper ---
        _DAnimation.SetBool("IsRunning", false);
        _DAnimation.SetBool("IsJumping", false);
        _DAnimation.SetBool("IsFalling", false);
        _DAnimation.SetBool("IsAttacking", false);
        _DAnimation.SetBool("IsDashing", false);
        _DAnimation.SetBool("IsDashingUp", false);
        _DAnimation.SetBool("IsDashingDown", false);

        // --- Camera ---
        if (cameraFollow != null)
            cameraFollow.LockCamOnPlayer();
    }

    public void ExitCinematicMode()
    {
        _movementDisabled = false;
        _rotatePlayer = true;
        _isCinematic = false;

        if (_attack != null)
            _attack.ResetAttackState();

        if (cameraFollow != null)
            cameraFollow.UnLockCamOnPlayer();
    }

}