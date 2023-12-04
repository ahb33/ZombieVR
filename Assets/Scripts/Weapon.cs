using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;


public class Weapon : MonoBehaviour
{

    public Transform bulletSpawnPoint;
    public Transform casingEjectPoint;



    public WeaponTypes currentWeaponType;

    public WeaponManager weaponManager;

    public AudioClip gunshotSound;

    public AudioSource audioSource;

    public float gunshotHearingRadius = 50f; // Defines how far the gunshot can be heard.

    public XRBaseInteractor interactor;

    public Transform barrelSocket;

    public GameObject muzzleFlash;

    public GameObject bloodFX;




    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // For demonstration purposes, we'll default to the first weapon in the list.
        // Later on, we can create a mechanism to switch weapons.

        //currentWeaponType = weaponTypes[0];

        currentWeaponType.AmmoOnHand = currentWeaponType.MaxAmmoOnHand; // we will start with the max Ammo On Hand
        currentWeaponType.AmmoInMag = currentWeaponType.MagCapacity;

    }

    public void FireWeapon()
    {
        // check if there is ammo in magazine
        if (currentWeaponType.AmmoOnHand > 0)
        {
            if (audioSource && gunshotSound)
            {
                audioSource.PlayOneShot(gunshotSound);
            }

            currentWeaponType.AmmoOnHand--;
            Debug.Log(currentWeaponType.AmmoOnHand);

            // Eject bullet casing
            GameObject casing = Instantiate(currentWeaponType.casingPrefab, casingEjectPoint.position, casingEjectPoint.rotation);
            BulletCasing bulletCasing = casing.GetComponent<BulletCasing>();
            if (bulletCasing)
            {
                bulletCasing.Eject(casingEjectPoint.right);
              
            }

            Collider[] zombiesInRange = Physics.OverlapSphere(transform.position, gunshotHearingRadius, LayerMask.GetMask("Zombie")); // Assuming your zombies are on a layer named "Zombie".
            foreach (Collider col in zombiesInRange)
            {
                Zombie zombie = col.GetComponent<Zombie>();
                if (zombie)
                {
                    zombie.HeardGunshot(transform.position);
                }
            }

            // Instantiate Bullet effect
            Instantiate(muzzleFlash, barrelSocket.position, barrelSocket.rotation);

            RaycastHit hit;
            Debug.DrawLine(barrelSocket.position, barrelSocket.position + transform.forward * 10, Color.red);
            if(Physics.Raycast(barrelSocket.position, transform.forward, out hit, 10))
            {
                if(hit.collider.gameObject.tag == "Zombie")
                {
                    Instantiate(bloodFX, hit.point, Quaternion.identity);
                    hit.collider.gameObject.GetComponent<Zombie>().TakeDamage(currentWeaponType.damage);
                }
            }

        }

    }

    public void ReloadWeapon()
    {
        Debug.Log("Reload weapon in Weapon.cs called");
        // Calculate the ammo needed to fill the magazine.
        int neededAmmo = currentWeaponType.MagCapacity - currentWeaponType.AmmoInMag;

        // Check if there is enough or some ammo on hand to reload.
        if (currentWeaponType.AmmoOnHand >= neededAmmo)
        {
            currentWeaponType.AmmoInMag = currentWeaponType.MagCapacity;
            currentWeaponType.AmmoOnHand -= neededAmmo;
        }
        else
        {
            currentWeaponType.AmmoInMag += currentWeaponType.AmmoOnHand;
            currentWeaponType.AmmoOnHand = 0;
        }
    }

    public string GetCurrentAmmo()
    {
        return currentWeaponType.AmmoOnHand + "/" + currentWeaponType.AmmoInMag;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "WeaponSlot")
        {
            interactor.enableInteractions = false;

            this.transform.SetParent(other.gameObject.transform);


            this.transform.localPosition = new Vector3(0, 0, 0);

            interactor.enableInteractions = true;

            this.GetComponent<Rigidbody>().useGravity = false;

            weaponManager.EquipWeapon(this);
            
        }
    }



    public void HandReference(SelectEnterEventArgs interactorRef)
    {
        interactor = interactorRef.interactorObject.transform.gameObject.GetComponent<XRBaseInteractor>(); // look up syntax
    }
}
