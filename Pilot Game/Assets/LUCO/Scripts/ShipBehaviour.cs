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
    public float maxSpeed = 5;
    public float maxBoostSpeed = 5;
    public float accelerationFactor = 5;
    public float BoostaccelerationFactor = 5;
    public float SpeedStraffing = 5;
    public float RotationSpeed = 1;

    public Transform Blaster;
    public GameObject Bullet;
    public float BulletSpeed;
    public float fireRate = 15f;
    public float spreadFactor;

    private float nextTimeToFire = 0f;

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
        if (Input.GetButton("Abutton"))
        {
            if (Input.GetAxis("LT") != 0 && Speed < maxBoostSpeed)
            {
                Speed += BoostaccelerationFactor * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("LT") == 0)
            {
                Speed = 0;
            }
        }
        else
        {
            if (Input.GetAxis("LT") != 0 && Speed < maxSpeed)
            {
                Speed += accelerationFactor * Time.deltaTime;
            }
            else if(Speed > maxSpeed)
            {
                Speed -= accelerationFactor * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("LT") == 0)
            {
                Speed = 0;
            }
        }

        //Switch LeftStickHorizontal between straff and rotation
        if (Input.GetButtonDown("LeftStickButton"))
        {
            if (AlternateControl == true)
                AlternateControl = false;
            else
                AlternateControl = true;
        }

        //shooting with autocannon
        if (Input.GetButton("RB") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            shootAutocannon(Blaster, BulletSpeed);
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

            Vector3 move = new Vector3();
            Vector3 acceleration = new Vector3();

            //to move the ship
            if (AlternateControl == false)
            {
                //acceleration
                acceleration = new Vector3(0, 0, Input.GetAxis("LT"));
                acceleration = acceleration.normalized * Time.deltaTime * (Speed * 100);
                rb.AddRelativeForce(acceleration, ForceMode.Acceleration);

                //straffing
                move = new Vector3(0, Input.GetAxis("RightStickVertical"), 0);
                move = move.normalized * Time.deltaTime * (SpeedStraffing * 100);
                rb.AddRelativeForce(move, ForceMode.Acceleration);

                //rotation
                Vector3 rotate = new Vector3(-Input.GetAxis("LeftStickVertical"), Input.GetAxis("RightStickHorizontal"), -Input.GetAxis("LeftStickHorizontal"));
                move = rotate.normalized * Time.deltaTime * (RotationSpeed * 10);
                rb.AddRelativeTorque(move, ForceMode.Acceleration);
            }
            else
            {
                //acceleration
                acceleration = new Vector3(0, 0, Input.GetAxis("LT"));
                acceleration = acceleration.normalized * Time.deltaTime * (Speed * 100);
                rb.AddRelativeForce(acceleration, ForceMode.Acceleration);

                //straffing
                move = new Vector3(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical"), 0);
                move = move.normalized * Time.deltaTime * (SpeedStraffing * 100);
                rb.AddRelativeForce(move, ForceMode.Acceleration);

                //rotation
                Vector3 rotate = new Vector3(-Input.GetAxis("LeftStickVertical"), 0, -Input.GetAxis("LeftStickHorizontal"));
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

    void shootAutocannon(Transform firepoint, float BulletSpeed)
    {
        /*RaycastHit hit;
        if (Physics.Raycast(Blaster.position, Blaster.forward, out hit, Mathf.Infinity, ennemiLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
        }*/

        Vector3 shootDir = firepoint.transform.forward;
        shootDir.x += Random.Range(-spreadFactor, spreadFactor);
        shootDir.y += Random.Range(-spreadFactor, spreadFactor);

        var projectileObj = Instantiate(Bullet, firepoint.position, Blaster.rotation) as GameObject;
        projectileObj.GetComponent<Rigidbody>().AddForce(shootDir * BulletSpeed, ForceMode.Impulse);
    }
}
