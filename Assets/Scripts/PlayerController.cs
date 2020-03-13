using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 velocity;

    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position+ velocity * Time.fixedDeltaTime);
    }
    
    public void Move(Vector3 moveVelocity)
    {
        velocity = moveVelocity;
    }

    public void LookAt(Vector3 point)
    {
        Vector3 pointAtPlayerHeight = new Vector3(point.x,transform.position.y,point.z);
        transform.LookAt(pointAtPlayerHeight);
    }
}
