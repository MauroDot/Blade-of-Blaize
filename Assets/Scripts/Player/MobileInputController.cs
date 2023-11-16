using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool jumpPressed;
    public static bool attackPressed;
    public static float horizontalMove;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.name == "JumpButton")
            jumpPressed = true;
        else if (gameObject.name == "AttackButton")
            attackPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (gameObject.name == "JumpButton")
            jumpPressed = false;
        else if (gameObject.name == "AttackButton")
            attackPressed = false;
    }

    // Call this method from your joystick script or event
    public static void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }
}
