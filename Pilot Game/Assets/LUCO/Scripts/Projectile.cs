using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public float Lifetime;

    public LayerMask ennemisLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Speed * Time.deltaTime);

        Lifetime -= Time.deltaTime;
        if(Lifetime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == ennemisLayer)
        {
            //collision.gameObject.GetComponent<Health>();
            Debug.Log(collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
