using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryItem", order = 1)]
public class InventoryItemSO : ScriptableObject
{
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] string _id;
    public string id {
        get {return _id;}
    }
}
