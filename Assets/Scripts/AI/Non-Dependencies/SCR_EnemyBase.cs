using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus {  idle, wander, lightAttack, heavyAttack, chase, }
public class SCR_EnemyBase : MonoBehaviour
{
    public SCR_EnemyData enemyData;


    public virtual void InitiateLightAttack()
    {
        //initate light attack
        Debug.Log(enemyData.lightAttackDamage);
    }
}
