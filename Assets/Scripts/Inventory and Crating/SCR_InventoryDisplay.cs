using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SCR_InventoryDisplay : MonoBehaviour
{


    public SCR_Inventory inventory;

    public int xSpace, ySpace;
    public int noOfColumns;

    public static Dictionary<InventorySlots, GameObject> itemsDisplayed = new Dictionary<InventorySlots, GameObject>();

    public int xStart, yStart;
    public TextMeshProUGUI displayText, dragAndDropText;

    public static int dictionaryCount;




    private void Awake()
    {
        //itemsDisplayed = SCR_DataManager.instance.ItemsDisplayed;
        dictionaryCount = -1;

        itemsDisplayed.Clear();
    }
    private void Start()
    {
        CreateDisplay();

    }


    private void Update()
    {
        UpdateDisplay();


    }


    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            GameObject obj = Instantiate(inventory.Container[i].item.displayPrefab, Vector3.zero, inventory.Container[i].item.displayPrefab.transform.rotation, transform);

            dictionaryCount++;

            //obj.GetComponent<SCR_HoverTip>().itemID = dictionaryCount;

            obj.GetComponent<RectTransform>().localPosition = GetNewPosition(i);

            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString();

            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }

    public Vector3 GetNewPosition(int newPos)
    {
        return new Vector3(xStart + (xSpace * (newPos % noOfColumns)), (yStart + (-ySpace * (newPos / noOfColumns))), 0f);
        //updates the inventory position by using the start at the top left and then the xspace and yspace between items
        //to perfectly position each one every time a new one is added or removed
    }


    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString();

                itemsDisplayed[inventory.Container[i]].GetComponent<Image>().sprite = inventory.Container[i].item.displayImage;

                //updates the inventory data - text and sprites - if it exists in the dictionary

                if (itemsDisplayed[inventory.Container[i]].GetComponent<RectTransform>().localPosition != new Vector3(xStart, yStart, 0f))
                {

                    itemsDisplayed[inventory.Container[i]].GetComponent<RectTransform>().localPosition = GetNewPosition(i);
                    //if (!SCR_HoverTip.isItemDragging)
                    //{
                    //    

                    //    //updates the position of all inventory items only if they are not at the start of the inventory and if they are not being dragged
                    //    //So basically, every item but item 0
                    //}


                }


            }
            else
            {


                GameObject obj = Instantiate(inventory.Container[i].item.displayPrefab, Vector3.zero, inventory.Container[i].item.displayPrefab.transform.rotation, transform);
                //if an item is in the inventory container but not in the dictionary then it creates an instance of it

                dictionaryCount++;
                //dicitionary count increases for every item added

                //obj.GetComponent<SCR_HoverTip>().itemID = dictionaryCount;
                //new items grab the new dictionary count as their item id

                obj.GetComponent<RectTransform>().localPosition = GetNewPosition(i);
                //assigns position

                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString();
                //assigns amount

                //PlayerData.instance.ItemsDisplayed.Add(inventory.Container[i], obj);
                //assigns a space in the 

            }


        }

    }

}
