using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponItem", menuName = "Inventory System/Inventory/Weapon Item")]
public class WeaponItem : SCR_BaseInvItem
{
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
