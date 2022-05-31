using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ItemType
{
    Health,
    SpecialHealth,
    Food,
    Default
}

public abstract class SCR_BaseInvItem : ScriptableObject
{
    public GameObject displayPrefab;
    public Sprite displayImage;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;


}
