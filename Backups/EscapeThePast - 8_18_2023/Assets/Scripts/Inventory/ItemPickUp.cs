using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public static ItemPickUp Instance;
    void Awake() { Instance = this; }

    public Item item;

    public void PickUp() {
        if (!UIAction.Instance.inventory.activeInHierarchy) {
            InventoryManager.Instance.Add(item);
            Destroy(gameObject);
        } else {
            Debug.Log("Close the inventory first");
        }
    }
}
