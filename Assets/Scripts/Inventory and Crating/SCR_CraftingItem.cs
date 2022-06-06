using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Item", menuName = "Inventory System/Inventory/Crafting Item")]
public class SCR_CraftingItem : ScriptableObject
{
    public SCR_BaseInvItem itemToCreate; //item to craft

    public List<SCR_BaseInvItem> craftingMaterials; //materials used to make it
}
