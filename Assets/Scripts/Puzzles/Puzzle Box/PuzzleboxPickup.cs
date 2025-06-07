using UnityEngine;

public class PuzzleboxPickup : MonoBehaviour
{
    [SerializeField] GameObject parentPrefab;
    [SerializeField] PuzzleboxItem item;

    private void OnTriggerEnter(Collider other)
    {
        // InventoryManager inventory = other.gameObject.GetComponent<PlayerManager>();
        if (InventoryManager.instance != null)
        {
            InventoryManager.addItem(item);
            Destroy(parentPrefab);
        }
    }
}
