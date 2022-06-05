using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SCR_EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public GameObject damageTextPrefab;
    public float textShakeStrength;


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(DamagePopup(damage));
    }

    public IEnumerator DamagePopup(float damageToShow)
    {
        GameObject newDamageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, transform.GetChild(0));
        newDamageText.LeanMoveY(7.5f, 1f).setEaseInOutExpo();
        //newDamageText.transform.LookAt(FindObjectOfType<SCR_CameraMovement>().transform);
        newDamageText.GetComponent<TextMeshProUGUI>().text = damageToShow.ToString();
        newDamageText.GetComponent<SCR_ObjectShake>().ShakeObject();

        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(UITools._instance.UnscaledDeltaFadeText(newDamageText.GetComponent<TextMeshProUGUI>(), false, 1.1f));

        yield return new WaitForSecondsRealtime(1f);

        Destroy(newDamageText);
    }
}
