using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] Sprite _icon;
    public Sprite icon {
        get {return _icon;}
    }

    public int stackSize = 1;
    //Hand model?
    
    public virtual string itemID {
        get {return "DEFAULT";}
    }

    public virtual void use()
    {
        Debug.LogError("InventoryItem.onUse() virtual function called! Check the code.");
    }
}
