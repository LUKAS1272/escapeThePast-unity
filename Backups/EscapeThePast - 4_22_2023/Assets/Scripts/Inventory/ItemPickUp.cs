using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    private void OnMouseDown() {
        if (!UIAction.Instance.inventory.activeInHierarchy) {
            InventoryManager.Instance.Add(item);
            Destroy(gameObject);
        } else {
            Debug.Log("Close the inventory first");
        }
    }
}
