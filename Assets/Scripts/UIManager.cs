using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ammoOnHandText;

    [SerializeField]
    private TextMeshProUGUI ammoInMagText;

    [SerializeField]
    private TextMeshProUGUI health;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private WeaponManager weaponManager; // Reference to your WeaponManager

    [SerializeField]
    private Weapon AssaultRifle;

    // Remove the 'public Ammo ammo;' line if you're not using it, or clarify its purpose if necessary.

    void Update()
    {
        UpdateAmmo();
        UpdateHealth();
    }

    void UpdateAmmo()
    {
        // Check if the weapon manager exists and has weapons
        if (weaponManager != null)
        {
            Debug.Log("Weapon Manager is not null");

            // Get the current weapon
            Weapon currentWeapon = weaponManager.GetCurrentWeapon();

            if (AssaultRifle != null)
            {
                // Update the ammo UI with values from the current weapon
                ammoOnHandText.text = AssaultRifle.currentWeaponType.AmmoOnHand.ToString();
                ammoInMagText.text = AssaultRifle.currentWeaponType.AmmoInMag.ToString();
            }
            else
            {
                // Handle the case where the current weapon or weapon type is not available
                ammoOnHandText.text = "N/A";
                ammoInMagText.text = "N/A";
            }
        }
        else
        {
            // Handle the case where the weapon manager is not assigned or there are no weapons
            ammoOnHandText.text = "N/A";
            ammoInMagText.text = "N/A";
        }
    }

    void UpdateHealth()
    {
        if (playerController != null)
        {
            // Access the player's health value directly from the PlayerController
            int playerHealth = playerController.GetHealth();
            health.text = playerHealth.ToString() + " HP";
        }
        else
        {
            // Handle the case where the playerController is not assigned
            health.text = "Health: N/A";
        }
    }

}
