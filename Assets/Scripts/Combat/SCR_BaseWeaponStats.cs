using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { dagger, spear, axe, crossbow}
public class SCR_BaseWeapon : ScriptableObject
{
    public float lightDamage;
    public float heavyDamage;
    public float durability;
    public float startingDurability;
    public float lightFireRate;
    public float heavyFireRate;
    public float range;
    public int criticalHitChance;
    public WeaponType weaponType;
    [TextArea(15, 20)]
    public string description;
    public GameObject weaponPrefab;

}
