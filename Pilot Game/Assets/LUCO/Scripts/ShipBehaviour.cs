using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    public bool Moving = false;
    public bool Rotating = false;

    public float decelerationFactor = 0.9f;
    public float Speed = 5;
    public float RotationSpeed = 1;

    public bool controller = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller == false)
        {
            /////USING A KEYBOARD/////

            //acceleration
            if (Input.GetKey(KeyCode.Z))
            {
                rb.AddForce(transform.forward * Speed, ForceMode.Acceleration);
                Moving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(transform.forward * -Speed, ForceMode.Acceleration);
                Moving = true;
            }
            else
            {
                Moving = false;
            }

            //straffing left right
            if (Input.GetKey(KeyCode.Q))
            {
                rb.AddForce(transform.right * -Speed, ForceMode.Acceleration);
                Moving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(transform.right * Speed, ForceMode.Acceleration);
                Moving = true;
            }
            else
            {
                Moving = false;
            }

            //straffing up down
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(transform.up * Speed, ForceMode.Acceleration);
                Moving = true;
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                rb.AddForce(transform.up * -Speed, ForceMode.Acceleration);
                Moving = true;
            }
            else
            {
                Moving = false;
            }

            //Rotation x
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddTorque(transform.right * RotationSpeed, ForceMode.Acceleration);
                Moving = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddTorque(transform.right * -RotationSpeed, ForceMode.Acceleration);
                Moving = true;
            }
            else
            {
                Moving = false;
            }

            //rotation z
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddTorque(transform.forward * -RotationSpeed, ForceMode.Acceleration);
                Moving = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddTorque(transform.forward * RotationSpeed, ForceMode.Acceleration);
                Moving = true;
            }
            else
            {
                Moving = false;
            }

            //rotation y
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(transform.up * -RotationSpeed, ForceMode.Acceleration);
                Moving = true;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                rb.AddTorque(transform.up * RotationSpeed, ForceMode.Acceleration);
                Moving = true;
            }
            else
            {
                Moving = false;
            }
        }
        else
        {
            /////USING A CONTROLLER/////
        }
        
    }

    private void FixedUpdate()
    {
        if(Moving == false)
        {
            rb.velocity = rb.velocity * decelerationFactor;
        }

        if(Rotating == false)
        {
            rb.angularVelocity = rb.angularVelocity * decelerationFactor;
        }
    }
}
