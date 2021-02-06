using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegacyPlayerController : MonoBehaviour
{
    public float speed = 0;
    public bool wonkyX = false;
    public bool wonkyY = false;
    public bool wonkySwitch = false;
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        if (wonkyX) { movementX *= -1; }
        if (wonkyY) { movementY *= -1; }
        if (wonkySwitch)
        {
            float wonkyMoveX = movementX;
            float wonkyMoveY = movementY;
            movementX = wonkyMoveY;
            movementY = wonkyMoveX;
        }
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        
        rb.AddForce(movement * speed);
    }

}