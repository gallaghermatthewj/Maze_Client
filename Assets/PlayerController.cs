using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    private float movementX;
    private float movementY;
    private bool movementChanged = false;
    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            ClientSend.PlayerShoot(new Vector3(movementX,0.0f,movementY));
            //ClientSend.PlayerShoot(camTransform.forward);
        }
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
        movementChanged = true;
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        /*bool[] _inputs = new bool[]
        {
            Keyboard.current.wKey.isPressed,
            Keyboard.current.sKey.isPressed,
            Keyboard.current.aKey.isPressed,
            Keyboard.current.dKey.isPressed,
            Keyboard.current.spaceKey.isPressed
        };*/

        if (movementChanged)
        {
            ClientSend.PlayerMovement(new Vector2(movementX, movementY));
            movementChanged = false;
        }
        
    }
}