using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    private bool Moving = false;
    private bool Rotating = false;

    private bool AlternateControl = false;

    [Header ("Ship Controls Parameters")]
    public float decelerationFactor = 0.9f;
    public float Speed = 5;
    public float maxSpeed = 5;
    public float maxBoostSpeed = 5;
    public float accelerationFactor = 5;
    public float BoostaccelerationFactor = 5;
    public float SpeedStraffing = 5;
    public float RotationSpeed = 1;

    [Header ("Machineguns")]
    public Transform Blaster;
    public GameObject Bullet;
    public float BulletSpeed;
    public float fireRate = 15f;
    public float spreadFactor;
    private float nextTimeToFire = 0f;

    [Header ("Missiles")]
    public Transform MissileLaucherTransform;
    public GameObject Missile;

    [Header ("Targeting")]
    public LayerMask ennemiLayer;
    public float detectionRange = 50f;
    public List<GameObject> TargetsInSight = new List<GameObject>();
    private int whereInList = 0;
    public GameObject LockedShip;
    public GameObject lockedWidget;
    public GameObject GizmoLock;

    [Header ("if the player use a controller or not")]
    public bool controller = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //add cursor to locked ship and keep it facing Player
        if(LockedShip != null)
        {
            lockedWidget.transform.LookAt(transform);
            GizmoLock.transform.LookAt(LockedShip.transform);

            lockedWidget.transform.position = LockedShip.transform.position;
            lockedWidget.SetActive(true);
        }
        else
        {
            lockedWidget.SetActive(false);
            lockedWidget.transform.position = Vector3.zero;
        }

        //To boost Spaceship speed or let it at its default speed
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

        //lock on an object from the target list
        if (Input.GetButtonDown("Ybutton"))
        {
            LockTarget();
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
        if (Input.GetButton("Abutton") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            shootAutocannon(Bullet ,Blaster, BulletSpeed);
        }

        //shooting a missile
        if (Input.GetButtonDown("Bbutton"))
        {
            //cooldownMissile
            shootMissile(Missile, MissileLaucherTransform);
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
                float Yrot = 0;
                if (Input.GetButton("LB"))
                {
                    Yrot = -1;
                }
                else if (Input.GetButton("RB"))
                {
                    Yrot = 1;
                }
                else if (Input.GetButton("LB") && Input.GetButton("RB"))
                {
                    Yrot = 0;
                }

                Vector3 rotate = new Vector3(-Input.GetAxis("LeftStickVertical"), Yrot, -Input.GetAxis("LeftStickHorizontal"));
                move = rotate.normalized * Time.deltaTime * (RotationSpeed * 10);
                rb.AddRelativeTorque(move, ForceMode.Acceleration);
            }
            /*else
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
            }*/
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

    void shootAutocannon(GameObject bullet, Transform firepoint, float BulletSpeed)
    {
        /*RaycastHit hit;
        if (Physics.Raycast(Blaster.position, Blaster.forward, out hit, Mathf.Infinity, ennemiLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
        }*/

        Vector3 shootDir = firepoint.transform.forward;
        shootDir.x += Random.Range(-spreadFactor, spreadFactor);
        shootDir.y += Random.Range(-spreadFactor, spreadFactor);

        var projectileObj = Instantiate(bullet, firepoint.position, Blaster.rotation) as GameObject;
        projectileObj.GetComponent<Rigidbody>().AddForce(shootDir * BulletSpeed, ForceMode.Impulse);
    }

    void shootMissile(GameObject missile, Transform MissileLauncher)
    {
        var projectileObj = Instantiate(missile, MissileLauncher.position, MissileLauncher.rotation) as GameObject;
        if(LockedShip != null)
        {
            projectileObj.GetComponent<Missile>().target = LockedShip.transform;
        }
        projectileObj.GetComponent<Missile>().initiateTrackingDelay();
    }

    void LockTarget()
    {
        /*GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in TargetsInSight)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        if(bestTarget != null)
        {
            LockedShip = bestTarget;
        }*/


        //iterate trough target list
        whereInList++;
        if(TargetsInSight.Count != 0)
        {
            if (whereInList <= TargetsInSight.Count - 1)
            {
                if (TargetsInSight[whereInList] != null)
                {
                    LockedShip = TargetsInSight[whereInList];
                }
                else if (TargetsInSight[whereInList] == null)
                {
                    TargetsInSight[whereInList] = TargetsInSight[TargetsInSight.Count - 1];
                    TargetsInSight.RemoveAt(TargetsInSight.Count - 1);
                }
            }
            else if (whereInList > TargetsInSight.Count - 1)
            {
                whereInList = 0;
                if (TargetsInSight[whereInList] != null)
                {
                    LockedShip = TargetsInSight[whereInList];
                }
                else if (TargetsInSight[whereInList] == null)
                {
                    TargetsInSight[whereInList] = TargetsInSight[TargetsInSight.Count - 1];
                    TargetsInSight.RemoveAt(TargetsInSight.Count - 1);
                }
            }
        }
        
        //unlock
        if(TargetsInSight.Count == 0)
        {
            LockedShip = null;
        }
    }
}