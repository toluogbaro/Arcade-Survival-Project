using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "AI/Enemy Data")]
public class SCR_EnemyData : ScriptableObject
{
    [TextArea(15, 20)]
    public string description;

    [Header("Vitals")]
    public float maxHealth;
    public float maxStamina;

    [Header("Attack")]
    public float lightAttackDamage;
    public float heavyAttackDamage;
    public float heavyAttackStaminaCost;
    public float range;
    public float fireRate;
    [Range(0f, 100f)] public float attackRange;

    [Header("Movement")]
    public float movementSpeed;
    [Range(0f, 100f)] public float playerDetectionRange;

}
