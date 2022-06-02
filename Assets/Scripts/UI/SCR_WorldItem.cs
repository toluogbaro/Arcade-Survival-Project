using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WorldItem : MonoBehaviour
{
    public SCR_BaseInvItem item;
    public SCR_Inventory inventory;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inventory.AddItem(item, 1);
            Destroy(gameObject);
        }
    }
}
