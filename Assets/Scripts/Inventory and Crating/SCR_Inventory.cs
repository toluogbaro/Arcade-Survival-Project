using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory/ItemInventory")]
public class SCR_Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    //private InventoryDataBase database;

    public int maxAmount = 12;

    public string savePath;

    public List<InventorySlots> Container = new List<InventorySlots>();

    [HideInInspector]
    public bool hasItem;

    public SCR_CraftingItem[] CraftDatabase;

    private void OnEnable()
    {
        //#if UNITY_EDITOR
        //      database = (InventoryDataBase)AssetDatabase.LoadAssetAtPath("Assets/Resources/Item Inventory Database.asset", typeof(InventoryDataBase));
        //#else
        //      database = Resources.Load<InventoryDataBase>("Item Inventory Database");
        //#endif
    }

    public void AddItem(SCR_BaseInvItem _item, int _amount)
    {

        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                return;
            }
        }

        //database.GetID[item]
        Container.Add(new InventorySlots(_item, _amount));

    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        formatter.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(formatter.Deserialize(file).ToString(), this);
            file.Close();
        }

    }

    public void OnAfterDeserialize()
    {
        //for (int i = 0; i < Container.Count; i++)
        //{
        //    Container[i].item = database.GetItem[Container[i].ID];
        //}


    }

    public void OnBeforeSerialize()
    {

    }

    public void RemoveItem(SCR_BaseInvItem _item, int _amount)
    {


        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].ReduceAmount(_amount);

                hasItem = true;

                break;
            }



        }



    }
}




[System.Serializable]
public class InventorySlots
{
    public int ID;
    public SCR_BaseInvItem item;
    public int amount;


    public InventorySlots(SCR_BaseInvItem _item, int _amount)
    {
        //ID = _id;
        item = _item;
        amount = _amount;

    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void ReduceAmount(int value)
    {
        amount -= value;
    }
}
