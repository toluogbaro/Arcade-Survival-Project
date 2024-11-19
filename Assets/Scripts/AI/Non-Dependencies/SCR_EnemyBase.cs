using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public enum EnemyStatus {  idle, wander, lightAttack, heavyAttack, chase, }
public class SCR_EnemyBase : SCR_Tree
{
    public SCR_EnemyData enemyData;
    public virtual void InitiateLightAttack()
    {
        //initate light attack
        Debug.Log(enemyData.lightAttackDamage);
    }

    protected override SCR_Node SetupTree()
    {
        return null;
    }
}
