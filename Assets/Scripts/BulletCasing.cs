using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    [SerializeField]
    private float bulletForce = 5f;

    [SerializeField]
    private float destroyDelay = 2f; 

    private Rigidbody casingBody;

    private void Awake()
    {
        casingBody = GetComponent<Rigidbody>();
    }

    public void Eject(Vector3 direction)
    {
        casingBody.AddForce(direction * bulletForce + Vector3.up * 2f, ForceMode.Impulse);
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, destroyDelay);
    }
}
