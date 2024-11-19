using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dagger Weapon", menuName = "Weapons/Dagger")]
public class SCR_Dagger : SCR_BaseWeapon
{
    public void Awake()
    {
        weaponType = WeaponType.dagger;
    }
}
