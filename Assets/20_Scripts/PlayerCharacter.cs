using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerCharacter;
using static UnityEngine.EventSystems.EventTrigger;

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
    [SerializeField] private Transform _mesh = null;
    [SerializeField] private Health _health;
    [SerializeField] public GameObject ChauveSouris;
    private CameraFollow cameraFollow;
    #endregion EditorVariables

    #region Variables

    //Components
    public Rigidbody2D _rigidbody = null;
    [SerializeField] public Animator _DAnimation;

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
    public bool IsGrounded { get; private set; } = true;

    //Air
    private float _airTime = 0.0f;
    private float _dashAirTime = 0.0f;
    private bool _isInCoyoteTime = false;

    //Jump
    private Vector2 _currentJumpForce = Vector2.zero;
    [SerializeField] private bool _isJumping = false;
    private float _jumpTime = 0.0f;
    private float _startJumpTime = 0.0f;
    private bool _bufferJump = false;
    private bool _hasBounce = false;


    //Event appel� quand on touche ou quitte le sol
    public event Action<PhysicState> OnPhysicStateChanged;

    //Dash
    public Vector2 _currentDashForce = Vector2.zero;
    private Vector2 _dashMovementInput = Vector2.zero;
    [SerializeField] public bool _canDash = true;
    public bool _isDashing = false;
    private float _startDashTime = 0.0f;
    private bool _bufferDash = false;
    [SerializeField] public bool _hittingDash = false;
    private float _dashAnimTime;

    [SerializeField] private Vector2 enemyBounceForce;

    //KnockBack
    private Collider2D _enemyCollider;
    private Vector3 targetKnockback = Vector3.zero;

    [SerializeField] private float BouncingTime;

    //Checkpoint
    public CheckPoints checkpoint;

    //Sprite
    private Vector3 _currentMeshRotation = Vector3.zero;
    private float rotationSpeed = 8000f;
    [SerializeField] private bool _lockedRotation = false;

    //Disable movement
    public bool MovementDisabled = false;

    [SerializeField] private Attack _attack;

    [SerializeField] private CapsuleCollider2D _capsuleBox;
    private Vector2 _sizeCapsule;
    private Vector2 _offsetCapsule;

    [SerializeField] public BoxCollider2D attackHitbox;
    [SerializeField] public BoxCollider2D dashHitbox;
    private Vector2 _sizeDashHitbox;
    private Vector2 _offsetDashHitbox;

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
        RotateMesh();
    }

    private void RotateMesh()
    {
        if (_attack.isAttacking || _isDashing)
            return;

        float targetRotation = _movementInput >= 0.01 ? 0f : _movementInput <= -0.01 ? 180f : _currentMeshRotation.y;

        _currentMeshRotation.y = Mathf.MoveTowards(_currentMeshRotation.y, targetRotation, rotationSpeed * Time.deltaTime);

        _mesh.rotation = Quaternion.Euler(0, _currentMeshRotation.y, 0);
    }

    #endregion Visual

    #region Update
    private void FixedUpdate()
    {
        DisableMovement();
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

    public void DisableMovement()
    {
        if (MovementDisabled == true)
        {
            return;
        }

        else
            //On reset la force � ajouter cette boucle de fixed update
            _forceToAdd = Vector2.zero;
        _prePhysicPosition = _rigidbody.position;

        //Fonction qui d�tecte si on touche le sol ou non
        //Et appelle les events associ�s
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

    private void GroundDetection()
    {
        //On utilise le filtre qui contient l'inclinaison du sol pour savoir si le rigidbody touche le sol ou non
        ContactFilter2D filter = _groundContactFilter;
        bool isTouchingGround = _rigidbody.IsTouching(filter);

        //Si le rigidbody touche le sol mais on a en m�moire qu'il ne le touche pas, on est sur la frame o� il touche le sol
        if (isTouchingGround && !IsGrounded)
        {
            if (!_canDash)
            {
                _canDash = true;
                ChauveSouris.gameObject.SetActive(true);
            }

            IsGrounded = true;
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
            cameraFollow.UnLockCamOnPlayer();

        }
    }

    private void ManageAirTime()
    {
        if (!IsGrounded)
            _airTime += Time.fixedDeltaTime;
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
        if (_isDashing)
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
            _capsuleBox.size = _sizeCapsule * new Vector2(2, 1);
            _capsuleBox.offset = new Vector2(1, 0.3f);
        }
        else if (_movementInput <= -0.01 && IsGrounded)
        {
            _DAnimation.SetBool("IsRunning", true);
            _capsuleBox.size = _sizeCapsule * new Vector2(2, 1);
            _capsuleBox.offset = new Vector2(-1, 0.3f);

        }
        else if (_movementInput == 0 && IsGrounded || _movementInput != 0 && !IsGrounded)
        {
            _DAnimation.SetBool("IsRunning", false);
            _capsuleBox.size = _sizeCapsule;
            _capsuleBox.offset = _offsetCapsule;
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

    #endregion HorizontalMovement

    #region Gravity

    private void Gravity()
    {
        if (IsGrounded || _isJumping || _isDashing || _hittingDash)
            return;

        float coyoteTimeRatio = Mathf.Clamp01(_airTime / _gravityParameters.CoyoteTime);
        float coyoteTimeFactor = _isInCoyoteTime ? _gravityParameters.GravityRemapFromCoyoteTime.Evaluate(coyoteTimeRatio) : 1.0f;
        float acceleration = _jumpParameters.Deceleration * coyoteTimeFactor * Time.fixedDeltaTime;

        float maxGravityForce = _gravityParameters.MaxForce;
        _currentGravity = Mathf.MoveTowards(_currentGravity, maxGravityForce, acceleration);

        float velocityDelta = _currentGravity - _rigidbody.linearVelocity.y;

        velocityDelta = Mathf.Clamp(velocityDelta, -_gravityParameters.MaxAcceleration, 0.0f);

        if (!_isDashing)
        {
            _DAnimation.SetBool("IsFalling", true);
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
        }
    }

    #endregion Gravity

    #region Jump

    public void StartJump()
    {
        if ((!IsGrounded && !_isInCoyoteTime) || _isJumping)
        {
            _bufferJump = true;
            Invoke(nameof(StopJumpBuffer), _jumpParameters.BufferTime);
            return;
        }
        _DAnimation.SetBool("IsJumping", true);

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
        if (!_isJumping)
            return;
        _capsuleBox.offset = new Vector2(0, 1.5f);

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

    //public float GravityValue(float baseWalkSpeed, float maxHeight, float maxLength) // v0 = -2 * h / t_h^2 || -2 * h * v^2 / L_h^2
    //{

    //    return -2 * maxHeight * baseWalkSpeed * baseWalkSpeed / ((maxLength * 0.5f) * (maxLength * 0.5f));
    //}

    //public float InitSpeed(float baseWalkSpeed, float maxHeight, float maxLength) // v0 = 2 * h / t_h || 2 * h * v / L_h
    //{
    //    return 2 * maxHeight * baseWalkSpeed / ((maxLength * 0.5f));
    //}

    #endregion Jump

    #region Dash
    public void GetDashInput(Vector2 Dashinput)
    {
        float scalaire = Vector2.Dot(Vector2.up, Dashinput);
        /*On multiplie un ensemble de valeur par le nombre de marche,
         * qu'on arrondis à l'inferieur ensuite,
         * puis on redivise par le nombre de marche pour obtenir un ensemble restreint de valeur (0, 0,5, 1),
         * Cet ensemble a un décalage arbitraire de 0,25 (marche de manoeuvre joystick)*/
        float step = Mathf.Floor((MathF.Abs(scalaire) + 0.25f) * 2) / 2f;
        /* Déplacement horizontal si le step > 0,5 dans ce cas déplacement vertical strict, donc pas horizontal, sinon on * le signe du déplacement (-1 ou 1),
         * par l'inverse du déplacement vertical (1 - step) qui donne soit 1 (déplcamenet horizontal strict ou 0,5 déplacement diagonal)*/
        float Mx = step > 0.5f ? 0 : Mathf.Sign(Dashinput.x) * (1 - step);
        _dashMovementInput = (new Vector2(Mx, step * Mathf.Sign(scalaire))).normalized;
        Debug.Log(_dashMovementInput);
    }

    public void StartDash()
    {
        if (!_isInCoyoteTime && _isDashing || !_canDash)
        {
            _bufferDash = true;
            Invoke(nameof(StopDashBuffer), _dashParameters.DashBufferTime);
            return;
        }

        if (_canDash)
        {
            _isDashing = true;
            _canDash = false;

            if (_dashMovementInput.y == 1)
            {
                _DAnimation.SetBool("IsDashingUp", true);
            }
            else if (_dashMovementInput.y != 0 && _dashMovementInput.x != 0)
            {
                _lockedRotation = true;

                float angle = Mathf.Atan2(_dashMovementInput.y, _dashMovementInput.x) * Mathf.Rad2Deg;

                _mesh.rotation = Quaternion.Euler(0, 0, angle);

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
            ChauveSouris.SetActive(false);
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

            _lockedRotation = false;

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
        ChauveSouris.gameObject.SetActive(true);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemyCollider = collision;

        if (collision.CompareTag("AttackZone") && !_hittingDash && collision != attackHitbox)
        {
            _health.TakeDamage(25);
            Knockback(collision);
            return;
        }
        if (collision.CompareTag("Dash") && _isDashing && collision != dashHitbox)
        {
            StopDashOnEnemy(collision);
            BounceOnEnemy();
            ChauveSouris.gameObject.SetActive(true);
            _canDash = true;
            collision.gameObject.SetActive(false);
        }
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
    public void Knockback(Collider2D enemy)
    {
        StopDashOnEnemy(enemy);
        _knockbackValues._knockbackDirection.x = (transform.position.x - _enemyCollider.transform.position.x);
        _knockbackValues._knockbackDirection.y = (transform.position.y - _enemyCollider.transform.position.y);
        targetKnockback = new Vector3(Mathf.Ceil(_knockbackValues._knockbackDirection.x), Mathf.Ceil(_knockbackValues._knockbackDirection.y), 0).normalized;

        Debug.Log(_knockbackValues._knockbackDirection.x);

        _rigidbody.AddForce(targetKnockback * _knockbackValues._knockbackForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        if (checkpoint)
        {
            transform.position = checkpoint.transform.position;
            _rigidbody.linearVelocity = Vector3.zero;
            _health.GetHeal(_health.GetMaxHealth());
            _health.UpdateBar();
            _isDashing = false;
            _isJumping = false;
        }
    }
}