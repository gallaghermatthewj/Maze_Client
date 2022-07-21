using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isDead;
    public Transform camTransform;
    public Transform mouseTransform;
    public Transform mouseForwardTransform;
    private float movementX;
    private float movementY;
    private bool movementChanged = false;
    public Animator animator;
    private void Update()
    {
                if (Mouse.current.leftButton.isPressed)
        {
            /*float maxRange = 5;
            RaycastHit hit;

            //if (Vector3.Distance(transform.position, player.position) < maxRange)
            //{
            
            
            Debug.DrawRay(mouseTransform.position, (mouseForwardTransform.position - mouseTransform.position), Color.green, 2, false);
            if (Physics.Raycast(mouseTransform.position, (mouseForwardTransform.position - mouseTransform.position), out hit, 100))
            {
                if (hit.transform.tag == "Player")
                {

                    Debug.Log("Hit!");
                    // In Range and i can see you!
                }
            }
            //}


            //ClientSend.PlayerShoot(new Vector3(movementX,0.0f,movementY));
            //ClientSend.PlayerShoot(camTransform.forward);*/
        }
    }

    private void OnMove(InputValue movementValue)
    {
        
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        movementX = movementVector.x;
        movementY = movementVector.y;
        animator.SetFloat("IsMoving", Mathf.Abs(movementX) + Mathf.Abs(movementY));
        movementChanged = true;
        
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            SendInputToServer();
        }
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