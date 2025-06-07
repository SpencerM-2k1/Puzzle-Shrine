using UnityEngine;

public class KeyItem : InventoryItem
{
    [SerializeField] LayerMask padlockLayer;

    public override string itemID {
        get {return "keyItem";}
    }

    public override void use() {
        PlayerLook eyes = PlayerManager.instance.GetComponentInChildren<PlayerLook>();
        
        if (eyes.itemInteract(padlockLayer))
        {
            InventoryManager.removeItem(itemID, 1);
        }
    }
}
