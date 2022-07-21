using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoRotate : MonoBehaviour
{
    public float RotationSpeed = 2f;
    public float rotationScalar = 1f;
    public Rigidbody rb;
    public float torque = 1f;
    float turnH = 1.1f;
    float turnV =1.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turnH = Random.Range(-2, 2);
        turnV = Random.Range(-2, 2);
        rb.AddTorque(transform.right * torque * turnH);
        rb.AddTorque(transform.up * torque * turnV);
        transform.Rotate(Vector3.up * (RotationSpeed * rotationScalar * Time.deltaTime));
    }
}
