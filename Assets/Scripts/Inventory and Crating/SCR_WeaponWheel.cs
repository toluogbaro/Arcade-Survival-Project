using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SCR_WeaponWheel : MonoBehaviour
{
    public static SCR_WeaponWheel _instance;

    public static int dictionaryCount;

    public SCR_Inventory inventory;

    public List<GameObject> wheelItems;

    public List<SCR_BaseInvItem> itemsToCraftWith;

    public List<GameObject> potentialItemsToDiscard;

    public SCR_CraftingItem itemToCraft;


    public static Dictionary<InventorySlots, GameObject> itemsDisplayed = new Dictionary<InventorySlots, GameObject>();

    private bool isCrafting;

    public void Awake()
    {
        _instance = this;
    }
    public void Start()
    {
        CreateDisplay();
    }

    public void Update()
    {
        if (!isCrafting) UpdateDisplay();

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(OpenWheels());
            SCR_GameManager._instance.SetState(GameState.PauseMenu);
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(CloseWheels());
            SCR_GameManager._instance.SetState(GameState.Gameplay);
        }

    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            GameObject obj = Instantiate(inventory.Container[i].item.displayPrefab, wheelItems[i].GetComponent<RectTransform>().transform.localPosition, inventory.Container[i].item.displayPrefab.transform.rotation, wheelItems[i].transform);

            itemsDisplayed.Add(inventory.Container[i], obj);

            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;

            wheelItems[i].GetComponent<SCR_Wheel>().item = inventory.Container[i].item;
            wheelItems[i].GetComponent<SCR_Wheel>().displayImage = obj;
            wheelItems[i].GetComponent<SCR_Wheel>().itemID = dictionaryCount;

            dictionaryCount++;

        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                //itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString();

                //itemsDisplayed[inventory.Container[i]].GetComponent<Image>().sprite = inventory.Container[i].item.displayImage;

                //updates the inventory data - text and sprites - if it exists in the dictionary

                // itemsDisplayed[inventory.Container[i]].GetComponent<RectTransform>().localPosition = wheelItems[i].GetComponent<RectTransform>().transform.localPosition;



            }
            else
            {


                GameObject obj = Instantiate(inventory.Container[i].item.displayPrefab, wheelItems[i].GetComponent<RectTransform>().transform.localPosition, inventory.Container[i].item.displayPrefab.transform.rotation, wheelItems[i].transform);

                itemsDisplayed.Add(inventory.Container[i], obj);

                obj.GetComponent<RectTransform>().localPosition = Vector3.zero;

                wheelItems[i].GetComponent<SCR_Wheel>().item = inventory.Container[i].item;
                wheelItems[i].GetComponent<SCR_Wheel>().displayImage = obj;
                wheelItems[i].GetComponent<SCR_Wheel>().itemID = dictionaryCount;

                dictionaryCount++;

                //creates wheel item if it doesnt exist

            }


        }

    }

    public IEnumerator OpenWheels()
    {
        for (int i = 0; i < wheelItems.Count; i++)
        {
            wheelItems[i].LeanScale(Vector3.one, 0.5f).setEaseInOutBack().setIgnoreTimeScale(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public IEnumerator CloseWheels()
    {
        for (int i = wheelItems.Count - 1; i > -1; i--)
        {
            wheelItems[i].LeanScale(Vector3.zero, 0.5f).setEaseInOutBack().setIgnoreTimeScale(true);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void AddToCraftList(SCR_BaseInvItem item)
    {
        itemsToCraftWith.Add(item);
    }

    public bool Craft()
    {

        for (int i = 0; i < inventory.CraftDatabase.Length; i++)
        {
            foreach (SCR_BaseInvItem craftingItem in itemsToCraftWith)
            {
                if (!inventory.CraftDatabase[i].craftingMaterials.Contains(craftingItem))
                {
                   
                    return false;
                }
                else
                {
                    itemToCraft = inventory.CraftDatabase[i];
                    

                }

                //loops through all crafting items and checks if the current items to craft with are contained in them
                //returns false if not and ends the check
            }
        }

        if (itemToCraft != null)
        {
            //if (itemToCraft.craftingMaterials.Count == itemsToCraftWith.Count) return true;
            //else return false;

            return true ? itemToCraft.craftingMaterials.Count == itemsToCraftWith.Count : itemToCraft.craftingMaterials.Count != itemsToCraftWith.Count;
            //returns true if the current number of crafting items matches the number of items required to craft the item you want
        }
        return false;



    }

    public void CraftButton()
    {
        isCrafting = true;

        if (Craft()) //if the boolean check is true when you press the craft button
        {



            List<int> potentialItemIntegers = new List<int>();

            //potential items to discard are added when you select an item

            foreach (GameObject item in potentialItemsToDiscard)
            {
                potentialItemIntegers.Add(item.GetComponentInParent<SCR_Wheel>().itemID);
                //adds the item id number of the inventory item
            }


            for (int i = 0; i < potentialItemsToDiscard.Count; i++)

            {

                itemsDisplayed.Remove(inventory.Container[potentialItemsToDiscard[i].GetComponentInParent<SCR_Wheel>().itemID]);

                dictionaryCount--;


            }


            for (int i = 0; i < potentialItemsToDiscard.Count; i++)

            {
                int highestNum = Mathf.Max(potentialItemIntegers.ToArray());

                //calculating the highest number of all the inventory items you want to craft
                // the higher the number the lowest in the list
                //I do this to remove items from the inventory from the bottom up to avoid ID mishmashes

                Debug.Log(highestNum);

                inventory.Container.Remove(inventory.Container[highestNum]);

                potentialItemIntegers.Remove(highestNum);

                Destroy(potentialItemsToDiscard[i].gameObject);
            }

            inventory.AddItem(itemToCraft.itemToCreate, 1);
            //adds crafted item to inventory

            itemToCraft = null;

            itemsToCraftWith.Clear();

            potentialItemsToDiscard.Clear();

            //removes crafting items from inventory



        }

        isCrafting = false;
    }

  

}
