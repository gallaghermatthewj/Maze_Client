using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    float lockPos = 0;
    public bool isLocalPlayer;
    public Quaternion currentOrientation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            //transform.rotation = currentOrientation;            
            //transform.parent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            transform.SetPositionAndRotation(transform.position, currentOrientation);
        }
    }
    public void rotateMouse(Vector3 _playerVelocity)
    {
        float angle = Mathf.Atan2(_playerVelocity.x, _playerVelocity.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.rotation = Quaternion.Euler(lockPos, lockPos, transform.rotation.eulerAngles.z);
        //transform.Rotate(Vector3.left, 1.5708f, Space.World);
        transform.RotateAroundLocal(Vector3.left, 1.5708f);

        if (!isLocalPlayer)
        {
            //transform.parent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            currentOrientation = transform.rotation;
        }
        //transform.rotation = Quaternion.AngleAxis(1.5708f, Vector3.right);
    }
}
