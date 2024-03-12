using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("PLAYER STATS")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _playerPunchForce;
    [SerializeField] private float _playerPunchCooldown;
    [SerializeField] private float _playerPunchSizeDetection;
    [SerializeField] private int _playerCarryAmount;
    [SerializeField] private float _playerInputMagnitudeToMove; // Equal of the animator transitions
    [Header("Player references")]
    [SerializeField] private Transform _playerPointOfPunch;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InertiaPile _playerPile;

    private PlayerState _playerState;
    private Transform _playerTransform;
    private Vector2 _inputDirection;

    private bool _isMoving;
    private bool _canPunch;
    private bool _canGrab;
    private float _punchTimer;

    public bool IsMoving => _isMoving;

    private void Awake()
    {
        Initialize();
        GameManager.Instance.UpdateMaxCapacity(_playerCarryAmount);
    }

    private void OnEnable()
    {
        GameManager.OnMaxCapacityUpdate += UpdateCarryAmount;
    }

    private void OnDisable()
    {
        GameManager.OnMaxCapacityUpdate -= UpdateCarryAmount;
    }

    private void Update()
    {
        DoMovement();
        PunchTimer();
    }

    private void Initialize()
    {
        _playerPile.SetPlayerReference(this);
        _playerState = PlayerState.Pursuing;
        _playerTransform = this.gameObject.transform;
        _inputDirection = Vector2.zero;
        _isMoving = false;
        _canPunch = true;
        _canGrab = false;
        _punchTimer = 0f;
    }

    #region Input dependent
    public void EnableMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _inputDirection = context.ReadValue<Vector2>();

            if (_inputDirection.magnitude <= _playerInputMagnitudeToMove) return;

            _isMoving = true;
        }
        else if (context.canceled)
        {
            _isMoving = false;
            _inputDirection = Vector2.zero;
        }

        _playerAnimator.SetFloat(GameConstants.ANIMATOR_FLOAT_INPUT_STICK, _inputDirection.magnitude);
    }

    public void DoPunch(InputAction.CallbackContext context)
    {
        if (context.performed && _canPunch && (_playerState == PlayerState.Pursuing))
        {
            _canPunch = !_canPunch;
            _playerAnimator.SetTrigger(GameConstants.ANIMATOR_TRIGGER_PUNCH);

            // Get all objects in the punch area
            Collider[] colliders = Physics.OverlapSphere(_playerPointOfPunch.position, _playerPunchSizeDetection);

            foreach (Collider collider in colliders)
            {
                // Knoch down knockable objects
                if (collider.gameObject.TryGetComponent<IKnockable>(out IKnockable knockable))
                {
                    // TODO: Check why the Z value wont change during the punch
                    Vector3 punchDirection = collider.transform.localPosition - this.transform.position;
                    knockable.KnockDown(punchDirection.normalized + this.transform.forward, _playerPunchForce);
                }
            }
        }
    }

    public void DoGrab(InputAction.CallbackContext context)
    {
        if (context.performed && _canGrab && (_playerState == PlayerState.Pursuing))
        {
            _playerState = PlayerState.Carrying;
        }
    }
    #endregion

    #region Continuous
    private void DoMovement()
    {
        if (!_isMoving) return;

        // Make player model face the direction of movement
        Vector3 newPosition = new(_inputDirection.x, 0, _inputDirection.y);
        _playerTransform.forward = newPosition;
        _playerTransform.position += _playerSpeed * Time.deltaTime * newPosition;
    }

    private void PunchTimer()
    {
        if (_canPunch) return;

        _punchTimer += Time.deltaTime;

        if (_punchTimer > _playerPunchCooldown)
        {
            _canPunch = true;
            _punchTimer = 0f;
        }
    }
    #endregion

    private void UpdateCarryAmount(int amount)
    {
        _playerCarryAmount += amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TAG_RAGDOLL))
        {
            // Check if the player is carrying, so getting closer will get automatically other npcs
            if (_playerState == PlayerState.Carrying && _playerPile.GetPileCount() <= _playerCarryAmount)
            {
                // Disable the sphere collider so its not triggered again when moving to player
                other.GetComponent<SphereCollider>().enabled = false;

                GameObject parentObject = other.GetComponent<ObjectToPile>().ParentObject;
                other.transform.position = parentObject.transform.position;

                other.GetComponent<Rigidbody>().isKinematic = true;

                _playerPile.AddToThePile(parentObject);
            } 

            if (!_canGrab) _canGrab = !_canGrab;
        }
        else if (other.CompareTag(GameConstants.TAG_DELIVER))
        {
            _playerPile.RemovePile();
            GameManager.Instance.UpdateMaxCapacity(1);
            GameManager.Instance.UpdateCarrying(0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.TAG_RAGDOLL))
        {
            _canGrab = false;
        }
    }
}
