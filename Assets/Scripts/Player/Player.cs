using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    public int diamonds;

    private Rigidbody2D _rigid;
    [SerializeField]
    private float _jumpForce = 5.0f;
    [SerializeField]
    private float _playerSpeed = 5.0f;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;
    [SerializeField]
    private float _maxStamina = 100.0f;
    [SerializeField]
    private float _staminaDrainRate = 20.0f;
    [SerializeField]
    private float _staminaRefillRate = 10.0f;
    private float _currentStamina;

    private bool _grounded;
    private bool _resetJump;
    private bool _isBoosting = false;

    [SerializeField]
    private Slider _staminaSlider;
    [SerializeField]
    private float _jumpStaminaCost = 25.0f;
    [SerializeField]
    private float _attackStaminaCost = 10.0f;

    private PlayerAnimation _playerAnim;
    private bool _doubleJumpAvailable = true;

    public int Health { get; set; }

    void Start()
    {
        InitializeComponents();
        _currentStamina = _maxStamina;
        _staminaSlider.maxValue = _maxStamina;
        _staminaSlider.value = _currentStamina;
        Health = 4;
    }
    void Update()
    {
        HandlePCInput();
        CheckGroundStatus();
        UpdateStamina();
    }

    private void HandlePCInput()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
    // Handle PC-specific controls
    float move = Input.GetAxisRaw("Horizontal") * _playerSpeed;
    MovePlayer(move);

    if (Input.GetMouseButton(1))
    {
        ActivateSpeedBoost(true);
    }
    else if (Input.GetMouseButtonUp(1))
    {
        ActivateSpeedBoost(false);
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
        PerformJump();
    }

    if (Input.GetMouseButtonDown(0))
    {
        PerformAttack();
    }
#endif
    }
    public void StartMovingLeft()
    {
        MovePlayer(-_playerSpeed, true);
    }

    public void StartMovingRight()
    {
        MovePlayer(_playerSpeed, true);
    }

    public void StopMoving()
    {
        MovePlayer(0, false);
        _isBoosting = false; // Stop boosting when the button is released
    }

    public void PerformAttack()
    {
        if (_currentStamina >= (_maxStamina * (_attackStaminaCost / 100.0f)))
        {
            _playerAnim.Attack();
            _currentStamina -= _maxStamina * (_attackStaminaCost / 100.0f);
        }
    }
    public void PerformJump()
    {
        // Check if the player has enough stamina to jump
        if (_currentStamina >= (_maxStamina * (_jumpStaminaCost / 100.0f)))
        {
            if (_grounded)
            {
                // Normal jump
                _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
                _doubleJumpAvailable = true;  // Enable double jump
            }
            else if (_doubleJumpAvailable)
            {
                // Double jump
                _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
                _doubleJumpAvailable = false; // Disable further double jumps
            }

            _currentStamina -= _maxStamina * (_jumpStaminaCost / 100.0f);
            _playerAnim.Jumping(true);
            _grounded = false;
            _resetJump = true;
            StartCoroutine(ResetJumpRoutine());
        }
    }
    private void MovePlayer(float move, bool isMoving)
    {
        // Apply speed and boost logic
        float speed = _playerSpeed;
        if (isMoving)
        {
            if (_isBoosting && _currentStamina > 0)
            {
                speed *= _speedBoostMultiplier;
            }
            _rigid.velocity = new Vector2(move * speed, _rigid.velocity.y);
            _playerAnim.Move(Mathf.Abs(move));

            // Flip the sprite based on movement direction
            Flip(move < 0);
        }
        else
        {
            _rigid.velocity = new Vector2(0, _rigid.velocity.y);
            _playerAnim.Move(0);
        }
    }
    private void ActivateSpeedBoost(bool isBoosting)
    {
        _isBoosting = isBoosting;
    }
    private void InitializeComponents()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
    }
    private void Flip(bool flipSprite)
    {
        Vector3 theScale = transform.localScale;
        theScale.x = flipSprite ? -Mathf.Abs(theScale.x) : Mathf.Abs(theScale.x);
        transform.localScale = theScale;
    }
    private void CheckGroundStatus()
    {
        Vector2 raycastOrigin = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitInfo = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.5f, 1 << 8);
        if (hitInfo.collider != null && !_resetJump)
        {
            if (!_grounded)
            {
                _grounded = true;
                _doubleJumpAvailable = true; // Reset double jump availability when grounded
            }
        }
        else
        {
            _grounded = false;
        }
        _playerAnim.Jumping(!_grounded);
    }
    private void UpdateStamina()
    {
        if (_isBoosting && _currentStamina > 0)
        {
            _currentStamina -= _staminaDrainRate * Time.deltaTime;
            if (_currentStamina <= 0)
            {
                _currentStamina = 0;
                _isBoosting = false;  // Stop boosting if stamina runs out
            }
        }
        else if (_currentStamina < _maxStamina)
        {
            _currentStamina += _staminaRefillRate * Time.deltaTime;
        }

        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        _staminaSlider.value = _currentStamina;
    }
    private IEnumerator ResetJumpRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
    public void Damage()
    {
        if (Health < 1) return;

        Debug.Log("Player::Damage()");
        Health--;
        UIManager.Instance.UpdateLives(Health);

        if (Health < 1)
        {
            Death();
        }
    }
    public void Addgems(int amount)
    {
        diamonds += amount;
        UIManager.Instance.UpdateGemCount(diamonds);
    }
    public void Death()
    {
        _playerAnim.Death();
        StartCoroutine(RestartLevelAfterDelay(2f));
    }
    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleSpeedBoost(bool boostActive)
    {
        if (boostActive && _currentStamina > 0)
        {
            _isBoosting = true;
        }
        else
        {
            _isBoosting = false;
        }
    }
}
