using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int maxHandgunAmmo = 20;
    public int maxRifleAmmo = 100;
    public int currentHandgunAmmo = 0;
    public int currentRifleAmmo = 0;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("HandgunAmmo"))
        //{
        //    AddAmmo("Handgun", 5); // Add 5 bullets when you pick up handgun ammo
        //    Destroy(other.gameObject);
        //}
        //else if (other.CompareTag("RifleAmmo"))
        //{
        //    AddAmmo("Rifle", 30); // Add 30 bullets when you pick up rifle ammo
        //    Destroy(other.gameObject);
        //}
    }

    public void AddAmmo(string weaponName, int amount)
    {
        switch (weaponName)
        {
            case "Handgun":
                currentHandgunAmmo = Mathf.Min(currentHandgunAmmo + amount, maxHandgunAmmo);
                break;
            case "Rifle":
                currentRifleAmmo = Mathf.Min(currentRifleAmmo + amount, maxRifleAmmo);
                break;
            default:
                Debug.LogError("Invalid weapon type");
                break;
        }
    }

    public void UseAmmo(string weaponName, int amount)
    {
        switch (weaponName)
        {
            case "Handgun":
                currentHandgunAmmo = Mathf.Max(currentHandgunAmmo - amount, 0);
                break;
            case "Rifle":
                currentRifleAmmo = Mathf.Max(currentRifleAmmo - amount, 0);
                break;
            default:
                Debug.LogError("Invalid weapon type");
                break;
        }
    }

    public int GetCurrentAmmo(string weaponName)
    {
        switch (weaponName)
        {
            case "Handgun":
                return currentHandgunAmmo;
            case "Rifle":
                return currentRifleAmmo;
            default:
                Debug.LogError("Invalid weapon type");
                return 0;
        }
    }
}
