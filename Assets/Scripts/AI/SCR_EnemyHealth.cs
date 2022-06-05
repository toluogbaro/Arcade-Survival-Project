using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SCR_EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public GameObject damageTextPrefab;
    public Color normalDamageColour, critDamageColour;
    public float textShakeStrength;

    private SCR_PlayerWeapon playerWeapon;


    public void Start()
    {
        playerWeapon = FindObjectOfType<SCR_PlayerWeapon>();
    }

    public void Update()
    {

    }


    public void TakeDamage(float damage)
    {
        StartCoroutine(CalculateKnockBackAndCritical(damage));
        //currentHealth -= damage;
        //StartCoroutine(DamagePopup(damage));
       
    }

    public IEnumerator DamagePopup(float damageToShow, bool isCrit)
    {
        GameObject newDamageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, transform.GetChild(0));;
        newDamageText.GetComponent<TextMeshProUGUI>().color = normalDamageColour;
        newDamageText.LeanMoveY(7.5f, 1f).setEaseInOutExpo();
        newDamageText.GetComponent<TextMeshProUGUI>().text = damageToShow.ToString();
        

        if (isCrit)
        {

            newDamageText.GetComponent<TextMeshProUGUI>().color = critDamageColour;
            yield return new WaitForSecondsRealtime(0.5f);
            newDamageText.GetComponent<SCR_ObjectShake>().ShakeObject();
            
        }

        StartCoroutine(TextLookAtPlayer(newDamageText, playerWeapon.directionBearer));

        yield return new WaitForSecondsRealtime(1f);
     
        StartCoroutine(UITools._instance.UnscaledDeltaFadeText(newDamageText.GetComponent<TextMeshProUGUI>(), false, 1.1f));

        yield return new WaitForSecondsRealtime(1f);
        Destroy(newDamageText);
    }

    public IEnumerator TextLookAtPlayer(GameObject textObj, GameObject objectToLookAt)
    {

        for (int i = 0; i < 10000; i++)
        {

            if (textObj != null) textObj.transform.rotation = Quaternion.LookRotation(textObj.transform.position - objectToLookAt.transform.position);
            yield return i;
        }


    }



    public IEnumerator CalculateKnockBackAndCritical(float damage)
    {
        switch (playerWeapon.currentWeapon.weaponType)
        {
            case WeaponType.dagger:
                currentHealth -= damage;
                StartCoroutine(DamagePopup(damage, false));
                break;

            case WeaponType.spear:
                if(CritChanceSuccessful(playerWeapon.currentWeapon.criticalHitChance))
                {
                    damage += CritChanceValues(1, 3);
                    currentHealth -= damage;
                    StartCoroutine(DamagePopup(damage, true));
                    break;
                }
                currentHealth -= damage;
                StartCoroutine(DamagePopup(damage, false));
                //transform.position = Vector3.Lerp(transform.position, -playerWeapon.currentWeaponPrefab.transform.forward, (5f * Time.deltaTime));
                break;

            case WeaponType.axe:
                if (CritChanceSuccessful(playerWeapon.currentWeapon.criticalHitChance))
                {
                    damage += CritChanceValues(2, 4);
                    currentHealth -= damage;
                    StartCoroutine(DamagePopup(damage, true));
                    break;
                }
                currentHealth -= damage;
                StartCoroutine(DamagePopup(damage, false));
                //transform.position = Vector3.Lerp(transform.position, -playerWeapon.currentWeaponPrefab.transform.forward, (10f * Time.deltaTime));
                break;

            case WeaponType.crossbow:
                if (CritChanceSuccessful(playerWeapon.currentWeapon.criticalHitChance))
                {
                    
                    damage += CritChanceValues(2, 4);
                    currentHealth -= damage;
                    StartCoroutine(DamagePopup(damage, true));
                    break;
                }
                currentHealth -= damage;
                StartCoroutine(DamagePopup(damage, false));
                //transform.position = Vector3.Lerp(transform.position, -playerWeapon.currentWeaponPrefab.transform.forward, (2f * Time.deltaTime));
                break;
        }

        yield return null;
    }

    public int CritChanceValues(int firstNum, int secondNum)
    {
        return Random.Range(firstNum, secondNum);
    }

    public bool CritChanceSuccessful(int weaponCritChance)
    {
        int randomChanceNum = Random.Range(0, 100);

        if (weaponCritChance >= randomChanceNum)
            return true;

        return false;
    }
}
