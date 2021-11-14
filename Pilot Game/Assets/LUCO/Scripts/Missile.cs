using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;

    public float speed;
    public float turnRate;
    private Quaternion GuideRotation;

    public bool tracking;

    public ParticleSystem explosion;

    [SerializeField]
    private float trackingDelay;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GuideMIssile();

        rb.velocity = transform.forward * speed;
        
        if(target != null && tracking == true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, GuideRotation, turnRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            //collision.gameObject.GetComponent<Health>();
            Debug.Log(other.gameObject.name + "hit by missile");
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void GuideMIssile()
    {
        if (target == null) return;
        else
        {
            Vector3 relativePosition = target.position - transform.position;
            GuideRotation = Quaternion.LookRotation(relativePosition, transform.up);
        }
    }

    public void initiateTrackingDelay()
    {
        StartCoroutine(TargetTrackingDelay());
    }

    IEnumerator TargetTrackingDelay()
    {
        yield return new WaitForSeconds(trackingDelay);
        tracking = true;
        speed = speed * 3;

    }

    IEnumerator AutoDestruction()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
