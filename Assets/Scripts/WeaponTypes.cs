using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponTypes : ScriptableObject
{

    // WeaponTypes should contain only properties that define the weapon type.
    // So, properties like AmmoOnHand and AmmoInMag should be removed and managed within the Weapon class.

    public string weaponName;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public int damage;
    public int AmmoOnHand;
    public int MaxAmmoOnHand;
    public int AmmoInMag;
    public int MagCapacity;



}
