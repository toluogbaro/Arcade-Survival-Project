using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthItem", menuName = "Inventory System/Inventory/HealthItem")]
public class HealthItem : SCR_BaseInvItem
{
    public void Awake()
    {
        type = ItemType.Health;
    }
}
