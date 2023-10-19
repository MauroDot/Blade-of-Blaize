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

    [SerializeField]
    private float _jumpStaminaCost = 25.0f;  // Percentage of stamina used when jumping

    private PlayerAnimation _playerAnim;


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

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))  // 0 is for left mouse button
        {
            Attack();
        }
    }

    private void Attack()
    {
        _playerAnim.Attack();
    }

    private void InitializeComponents()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
    }

    private void MovePlayer()
    {
        if (_grounded)  // Check if the player is grounded
        {
            float move = Input.GetAxisRaw("Horizontal") * _playerSpeed;

            // Check if the player is moving left or right and flip the sprite accordingly
            if (move > 0) // Moving right
            {
                Flip(false);  // false indicates not flipped (right facing)
            }
            else if (move < 0) // Moving left
            {
                Flip(true);  // true indicates flipped (left facing)
            }

            if (Input.GetKey(KeyCode.Mouse1) && _currentStamina > 0)
            {
                move *= _speedBoostMultiplier;
                _currentStamina -= _staminaDrainRate * Time.deltaTime;  // Decrease stamina
            }

            _rigid.velocity = new Vector2(move, _rigid.velocity.y);
            _playerAnim.Move(move);
        }
    }

    private void Flip(bool flipSprite)
    {
        // Get the local scale of the GameObject
        Vector3 theScale = transform.localScale;

        // If flipSprite is true, set the x-component to its absolute value (facing left)
        // Otherwise, set it to its negative absolute value (facing right)
        theScale.x = flipSprite ? Mathf.Abs(theScale.x) * -1 : Mathf.Abs(theScale.x);

        // Apply the new local scale to flip the sprite
        transform.localScale = theScale;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            // Check if the player has enough stamina to jump
            if (_currentStamina >= (_maxStamina * (_jumpStaminaCost / 100.0f)))
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
                _grounded = false;
                _resetJump = true;
                StartCoroutine(ResetJumpRoutine());

                // Deduct stamina cost for jumping
                _currentStamina -= _maxStamina * (_jumpStaminaCost / 100.0f);

                // Start the jump animation
                _playerAnim.Jumping(true);
            }
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

        if (hitInfo.collider != null && !_resetJump)
        {
            _grounded = true;
            _playerAnim.Jumping(false); // End the jump animation when grounded
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
