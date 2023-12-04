using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; 
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>(); // Using a List to dynamically handle weapons

    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    public GameObject weaponSlot;


    public InputActionReference reloadActionReference;

    private void Update()
    {
        //HandleInput();
    }

    //private void HandleInput()
    //{
    //    if (Input.GetButtonDown("SwitchWeaponButton")) 
    //    {
    //        SwitchWeapon();
    //    }
    //}

    public void EquipWeapon(Weapon newWeapon)
    {
        if (primaryWeapon == null)
        {
            primaryWeapon = newWeapon;
            StoreWeaponInSlot(primaryWeapon);  // Store the new primary weapon in the slot
        }
        else if (secondaryWeapon == null)
        {
            secondaryWeapon = primaryWeapon;
            primaryWeapon = newWeapon;
            StoreWeaponInSlot(primaryWeapon);  // Store the new primary weapon in the slot
        }
        else
        {
            secondaryWeapon = primaryWeapon;
            primaryWeapon = newWeapon;
            StoreWeaponInSlot(primaryWeapon);  // Store the new primary weapon in the slot
        }
    }


    // Method to get the current active weapon
    public Weapon GetCurrentWeapon()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.gameObject.activeSelf)
            {
                return weapon;
            }
        }
        return null;
    }

    public void SwitchWeapon()
    {
        if (secondaryWeapon != null)
        {
            // Swap the primary and secondary weapons
            Weapon temp = primaryWeapon;
            primaryWeapon = secondaryWeapon;
            secondaryWeapon = temp;

            // Store the new primary weapon in the slot
            StoreWeaponInSlot(primaryWeapon);
            Debug.Log("Switch button pressed");
        }
    }

    private void StoreWeaponInSlot(Weapon weaponToStore)
    {
        // Deactivate all weapons
        foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        // Activate and set the parent of the weapon we want to store
        weaponToStore.gameObject.SetActive(true);
        weaponToStore.transform.SetParent(weaponSlot.transform);
        weaponToStore.transform.localPosition = new Vector3(0, 0, 0);
        weaponToStore.transform.localRotation = Quaternion.identity;

        Rigidbody weaponRb = weaponToStore.GetComponent<Rigidbody>();
        if (weaponRb)
        {
            weaponRb.isKinematic = true; // Stops the weapon from reacting to physics
            weaponRb.velocity = Vector3.zero; // Reset any velocities
            weaponRb.angularVelocity = Vector3.zero; // Reset any angular velocities
        }
    }

    //private void OnEnable()
    //{
    //    if(switchWeaponReference!= null)
    //    {
    //        switchWeaponReference.action.Enable();
    //    }
    //    if (reloadActionReference != null)
    //    {
    //        reloadActionReference.action.performed += HandleReloadAction;
    //        reloadActionReference.action.Enable();
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (switchWeaponReference != null)
    //    {
    //        switchWeaponReference.action.Disable();
    //    }
    //    if (reloadActionReference != null)
    //    {
    //        reloadActionReference.action.performed -= HandleReloadAction;
    //        reloadActionReference.action.Disable();
    //    }
    //}

    private void HandleReloadAction(InputAction.CallbackContext context)
    {
        Weapon currentWeapon = GetCurrentWeapon();
        if (currentWeapon != null)
        {
            currentWeapon.ReloadWeapon();
        }
    }
}
