using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPath : MonoBehaviour
{
    // speed and damage will depend on type of weapon

    public float speed = 20f;
    private int damage = 0;  // Set to 0 by default
    public Rigidbody firedBulletBody;

    public GameObject bulletEffect;

    [SerializeField]
    private float destroyDelayAfterHit = 0.05f; // delay in seconds

    

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        firedBulletBody = GetComponent<Rigidbody>();
        if (firedBulletBody)
            firedBulletBody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie")
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if(zombie)
            {
                zombie.TakeDamage(damage);
                Debug.Log("Damage dealt");
            }

        }


        // Instantiate an impact effect
        if (bulletEffect != null)
        {
            Instantiate(bulletEffect, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        // Destroy the bullet after a short delay
        Destroy(gameObject, destroyDelayAfterHit);

    }
}
