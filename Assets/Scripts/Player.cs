using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;

    [SerializeField]
    private float _jumpForce = 5.0f;

    [SerializeField]
    private float _playerSpeed = 5.0f;

    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;

    [SerializeField]
    private float _maxStamina = 100.0f;  // Max stamina amount

    [SerializeField]
    private float _staminaDrainRate = 20.0f;  // How fast the stamina decreases

    [SerializeField]
    private float _staminaRefillRate = 10.0f;  // How fast the stamina refills

    private float _currentStamina;  // Current stamina amount

    private bool _grounded;
    private bool _resetJump;

    [SerializeField]
    private Slider _staminaSlider;


    void Start()
    {
        InitializeComponents();
        _currentStamina = _maxStamina;  // Initialize current stamina to max
        _staminaSlider.maxValue = _maxStamina;
        _staminaSlider.value = _currentStamina;
    }

    void Update()
    {
        MovePlayer();
        Jump();
        CheckGroundStatus();
        UpdateStamina();
    }

    private void InitializeComponents()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void MovePlayer()
    {
        if (_grounded)  // Check if the player is grounded
        {
            float move = Input.GetAxisRaw("Horizontal") * _playerSpeed;

            if (Input.GetKey(KeyCode.J) && _currentStamina > 0)
            {
                move *= _speedBoostMultiplier;
                _currentStamina -= _staminaDrainRate * Time.deltaTime;  // Decrease stamina
            }

            _rigid.velocity = new Vector2(move, _rigid.velocity.y);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
            _grounded = false;
            _resetJump = true;
            StartCoroutine(ResetJumpRoutine());
        }
    }

    private void CheckGroundStatus()
    {
        Vector2 raycastOrigin = transform.position + new Vector3(0, -0.5f, 0);
        RaycastHit2D hitInfo = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.5f, 1 << 8);

        // This will draw the ray in the Unity Editor
        Debug.DrawRay(raycastOrigin, Vector2.down * 0.5f, Color.green);

        if (hitInfo.collider != null && !_resetJump)
        {
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }
    }

    private void UpdateStamina()
    {
        // Refill stamina when the boost key is not pressed
        if (!Input.GetKey(KeyCode.J) && _currentStamina < _maxStamina)
        {
            _currentStamina += _staminaRefillRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, _maxStamina);  // Ensure it doesn't exceed max stamina
        }

        // Update the stamina slider value
        _staminaSlider.value = _currentStamina;
    }

    private IEnumerator ResetJumpRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
}
