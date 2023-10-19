using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private Animator _swordAnimation;  // Reference to the Sword_Arc GameObject's animator
    private GameObject _swordArc;      // Reference to the Sword_Arc GameObject itself

    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _swordArc = transform.GetChild(1).gameObject;  // Get the actual Sword_Arc GameObject
        _swordAnimation = _swordArc.GetComponent<Animator>();

        _swordArc.SetActive(false);  // Ensure the Sword_Arc GameObject starts as inactive
    }

    public void Move(float move)
    {
        _anim.SetFloat("Move", Mathf.Abs(move));
    }

    public void Jumping(bool isJumping)
    {
        _anim.SetBool("Jumping", isJumping);
    }

    public void Attack()
    {
        _anim.SetTrigger("Attack");

        // Activate the Sword_Arc GameObject and play its animation
        _swordArc.SetActive(true);
        _swordAnimation.SetTrigger("SwordAnimation");

        // Assuming the sword arc animation length is known (e.g., 0.5 seconds), 
        // you can start a coroutine to deactivate the Sword_Arc GameObject after the animation finishes.
        StartCoroutine(DeactivateSwordArcAfterAnimation(0.5f));
    }

    private IEnumerator DeactivateSwordArcAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        _swordArc.SetActive(false);
    }
}
