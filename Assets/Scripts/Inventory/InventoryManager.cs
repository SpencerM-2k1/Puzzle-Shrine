using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Singleton for player character. Used to find player in scene, particularly
    //when persistent objects are involved
    //(Should be automatically set to null if stored singleton is destroyed)
    public static InventoryManager instance;
    
    public int selectedIndex = 0;
    [SerializeField] List<InventoryItem> items = new List<InventoryItem>();

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("InventoryManager initialized!");
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // OnDestroy, clear the singleton
    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;        
        }
    }

    void Start()
    {
        InventoryItem[] instantiatedItems = GetComponentsInChildren<InventoryItem>();
        foreach(InventoryItem item in instantiatedItems)
        {
            items.Add(item);
        }
        updateItemHUD();
    }

    //Inventory management
    public static void addItem(InventoryItem newItem)
    {
        InventoryItem duplicateItem = null;
        foreach (InventoryItem item in instance.items)
        {
            if (item.itemID == newItem.itemID)
                duplicateItem = item;
        }
        if (duplicateItem != null)
        {
            duplicateItem.stackSize += newItem.stackSize;
        }
        else
        {
            InventoryItem instantiatedItem = Instantiate<InventoryItem>(newItem, instance.transform);
            instance.items.Add(instantiatedItem);
        }
        instance.updateItemHUD();
    }

    public static void removeItem(string queryID, int count)
    {
        InventoryItem targetItem = null;
        int targetItemIndex = -1;
        for (int i = 0; i < instance.items.Count; i++)
        {
            if (instance.items[i].itemID == queryID)
            {
                targetItem = instance.items[i];
                targetItemIndex = i;
                break;
            }
        }
        if (targetItem == null)
        {
            Debug.LogWarning("Attempted to subtract item not held by player!");
        }
        targetItem.stackSize -= count;
        
        if (targetItem.stackSize < 1)
        {
            instance.items.RemoveAt(targetItemIndex);
            Destroy(targetItem.gameObject);
            prevItem();
        }
        instance.updateItemHUD();
    }

    public static void nextItem()
    {
        if (instance.items.Count == 0)
            return;
        instance.selectedIndex++;
        if (instance.selectedIndex >= instance.items.Count)
        {
            instance.selectedIndex = 0;
        }
        instance.updateItemHUD();
    }

    public static void prevItem()
    {
        if (instance.items.Count == 0)
            return;
        instance.selectedIndex--;
        if (instance.selectedIndex < 0)
        {
            instance.selectedIndex = instance.items.Count - 1;
            if (instance.selectedIndex < 0) instance.selectedIndex = 0;
        }
        instance.updateItemHUD();
    }

    public static void useCurrentItem()
    {
        if (instance.items.Count == 0)
            return;
        instance.items[instance.selectedIndex].use();
    }

    void updateItemHUD()
    {
        bool showPrevNext = false;
        if (items.Count > 1)
            showPrevNext = true;
        
        if (items.Count > 0)
        {
            HUDManager.inventoryBarActive(true);
            HUDManager.setItemDisplay(items[selectedIndex], showPrevNext);
        }
        else
        {
            HUDManager.inventoryBarActive(false);
        }
    }

    //Clean deletion (call this instead of destroying the gameObject directly)
    public void destroyInventory()
    {
        Destroy(this.gameObject);
    }
}
