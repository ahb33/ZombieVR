using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MagazineSlot")
        {
            Debug.Log("Attempting to reload magazine");

            Weapon assaultRifle = other.gameObject.transform.parent.gameObject.GetComponent<Weapon>();
            assaultRifle.ReloadWeapon();
            Destroy(this.gameObject);
        }
    }
}
