using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerWeapon : MonoBehaviour
{
    public static SCR_PlayerWeapon _instance;

    public SCR_BaseWeapon currentWeapon;
    public GameObject weaponHolder, currentWeaponPrefab, rightHand;
    public LayerMask enemyMask;
    private float lastAttack = 0f;
    public Vector3 attackPositionOne, attackPositionTwo;
    public Vector3 attackRotationOne;
    private bool hitDetection;
    private Collider[] hitColliders;
    public void Awake()
    {
        _instance = this;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(currentWeaponPrefab.GetComponent<SCR_WorldWeapon>().weaponTip.transform.position, currentWeapon.range);
    }

    public void Update()
    {
        if (Time.time > currentWeapon.lightFireRate + lastAttack && InputManager._instance.GetKeyDown("LightAttack") && currentWeapon != null) CalculateWeaponStatsLightAttack();
        if (Input.GetKeyDown(KeyCode.R)) currentWeaponPrefab.transform.eulerAngles = new Vector3(0, 0, 0);

        if(hitDetection) hitColliders = Physics.OverlapSphere(currentWeaponPrefab.GetComponent<SCR_WorldWeapon>().weaponTip.transform.position, currentWeapon.range, enemyMask);
    }

    public void CalculateWeaponStatsLightAttack()
    {
        //Collider[] hitColliders = Physics.OverlapSphere(currentWeaponPrefab.GetComponent<SCR_WorldWeapon>().weaponTip.transform.position, currentWeapon.range, enemyMask);
        lastAttack = Time.time;
        StartCoroutine(PlaceHolderAttackDagger());

        //foreach (Collider hitCollider in hitColliders)
        //{
        //    Debug.Log("hit");
        //    hitCollider.gameObject.GetComponent<SCR_EnemyHealth>().TakeDamage(currentWeapon.lightDamage);
        //    currentWeapon.durability -= 1;
        //}
    }

    IEnumerator PlaceHolderAttackDagger()
    {
        

        Vector3 lastPos = rightHand.transform.localPosition;
        Vector3 lastRot = rightHand.transform.eulerAngles;

        rightHand.LeanMoveLocal(attackPositionOne, 0.1f).setEaseInOutQuint();
        rightHand.LeanRotate(attackRotationOne, 0.1f).setEaseInOutQuint();

        hitDetection = true;

        yield return new WaitForSeconds(0.1f);

        rightHand.LeanMoveLocal(attackPositionTwo, 0.25f).setEaseInOutQuint();

        foreach (Collider hitCollider in hitColliders)
        {
            Debug.Log("hit");
            hitCollider.gameObject.GetComponent<SCR_EnemyHealth>().TakeDamage(currentWeapon.lightDamage);
            currentWeapon.durability -= 1;
        }

        yield return new WaitForSeconds(0.25f);

        rightHand.LeanMoveLocal(lastPos, 0.25f).setEaseInOutQuint();
        rightHand.LeanRotate(new Vector3(0, 0, 0), 0.25f).setEaseInOutQuint();

        hitDetection = false;

        yield return null;
    }
}
