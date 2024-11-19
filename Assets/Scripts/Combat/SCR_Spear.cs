using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spear Weapon", menuName = "Weapons/Spear")]
public class SCR_Spear : SCR_BaseWeapon
{
    public void Awake()
    {
        weaponType = WeaponType.spear;
    }
}
