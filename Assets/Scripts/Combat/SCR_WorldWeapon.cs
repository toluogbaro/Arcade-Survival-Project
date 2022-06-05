using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WorldWeapon : MonoBehaviour
{
    public SCR_BaseWeapon weapon;
    public SCR_PlayerWeapon playerWeapon;
    public GameObject weaponTip;

    public void Awake()
    {
        playerWeapon = FindObjectOfType<SCR_PlayerWeapon>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (playerWeapon.currentWeaponPrefab != null)

            {
                Destroy(playerWeapon.currentWeaponPrefab);
                playerWeapon.currentWeapon = weapon;
                playerWeapon.currentWeaponPrefab = Instantiate(weapon.weaponPrefab, playerWeapon.weaponHolder.transform.localPosition, Quaternion.identity);
                playerWeapon.currentWeaponPrefab.GetComponent<BoxCollider>().enabled = false;
                playerWeapon.transform.eulerAngles = Vector3.zero;
                playerWeapon.currentWeaponPrefab.transform.SetParent(playerWeapon.weaponHolder.transform);
                playerWeapon.currentWeaponPrefab.transform.localPosition = Vector3.zero;
                Destroy(gameObject);

            }
            else
            {
                playerWeapon.currentWeapon = weapon;
                playerWeapon.currentWeaponPrefab = Instantiate(weapon.weaponPrefab, Vector3.zero, Quaternion.identity);
                playerWeapon.currentWeaponPrefab.GetComponent<BoxCollider>().enabled = false;
                playerWeapon.transform.eulerAngles = Vector3.zero;
                playerWeapon.currentWeaponPrefab.transform.SetParent(playerWeapon.weaponHolder.transform);
                //playerWeapon.currentWeaponPrefab.transform.position = Vector3.zero;
                Destroy(gameObject);
            }
        }
        
    }

    public void OnApplicationQuit()
    {
        weapon.durability = weapon.startingDurability;
    }
}
