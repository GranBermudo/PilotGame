using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    public bool Moving = false;
    public bool Rotating = false;

    public bool AlternateControl = false;

    public float decelerationFactor = 0.9f;
    public float Speed = 5;
    public float RotationSpeed = 1;

    public Transform Blaster;

    public LayerMask ennemiLayer;

    public bool controller = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Switch LeftStickHorizontal between straff and rotation
        if (Input.GetButtonDown("LeftStickButton"))
        {
            if (AlternateControl == true)
                AlternateControl = false;
            else
                AlternateControl = true;
        }

        if (Input.GetButtonDown("RB"))
        {
            shoot();
        }
    }

    private void FixedUpdate()
    {
        if (controller == false)
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

            //to move the ship
            if (AlternateControl == false)
            {
                //straffing
                Vector3 move = new Vector3(0, -Input.GetAxis("LeftStickVertical"), Input.GetAxis("LT"));
                move = move.normalized * Time.deltaTime * (Speed * 100);
                rb.AddRelativeForce(move, ForceMode.Acceleration);

                //rotation
                Vector3 rotate = new Vector3(Input.GetAxis("RightStickVertical"), Input.GetAxis("LeftStickHorizontal"), -Input.GetAxis("RightStickHorizontal"));
                move = rotate.normalized * Time.deltaTime * (RotationSpeed * 10);
                rb.AddRelativeTorque(move, ForceMode.Acceleration);
            }
            else
            {
                //straffing
                Vector3 move = new Vector3(Input.GetAxis("LeftStickHorizontal"), -Input.GetAxis("LeftStickVertical"), Input.GetAxis("LT"));
                move = move.normalized * Time.deltaTime * (Speed * 100);
                rb.AddRelativeForce(move, ForceMode.Acceleration);

                //rotation
                Vector3 rotate = new Vector3(Input.GetAxis("RightStickVertical"), 0, -Input.GetAxis("RightStickHorizontal"));
                move = rotate.normalized * Time.deltaTime * (RotationSpeed * 10);
                rb.AddRelativeTorque(move, ForceMode.Acceleration);
            }
        }

        if (Moving == false)
        {
            rb.velocity = rb.velocity * decelerationFactor;
        }

        if(Rotating == false)
        {
            rb.angularVelocity = rb.angularVelocity * decelerationFactor;
        }
    }

    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Blaster.position, Blaster.forward, out hit, Mathf.Infinity, ennemiLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
