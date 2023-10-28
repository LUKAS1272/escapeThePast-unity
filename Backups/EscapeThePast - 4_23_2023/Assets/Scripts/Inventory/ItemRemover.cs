using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRemover : MonoBehaviour
{
    public static ItemRemover Instance;
    public Button removeButton;
    public int index;

    void Awake() {
        Instance = this;
    }

    public void RemoveItem() {
        InventoryManager.Instance.Remove(index);
    }
}
