using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToTargetList : MonoBehaviour
{
    private ShipBehaviour playerShipBehaviour;
    [SerializeField] private MeshRenderer[] shipParts;
    private bool spotted = false;

    private void Awake()
    {
        playerShipBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipBehaviour>();
    }

    private void Update()
    {
        /*foreach(MeshRenderer mr in shipParts)
        {
            if (mr.isVisible && spotted == false)
            {
                playerShipBehaviour.TargetsInSight.Add(this.gameObject);
                spotted = true;
            }
            else if(mr.isVisible == false)
            {
                playerShipBehaviour.TargetsInSight.Remove(this.gameObject);
                spotted = false;
            }
        }*/

        if(Vector3.Distance(transform.position, playerShipBehaviour.transform.position) <= playerShipBehaviour.detectionRange && spotted == false)
        {
            playerShipBehaviour.TargetsInSight.Add(this.gameObject);
            spotted = true;
        }
        else if (Vector3.Distance(transform.position, playerShipBehaviour.transform.position) > playerShipBehaviour.detectionRange)
        {
            playerShipBehaviour.TargetsInSight.Remove(this.gameObject);
            spotted = false;
        }
    }
}
