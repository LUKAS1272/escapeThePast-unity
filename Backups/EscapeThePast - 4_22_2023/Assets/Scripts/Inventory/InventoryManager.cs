using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public GameObject slot;
    public GameObject slotParent;

    public List<Button> closeButtons = new List<Button>();
    public Toggle removeItems;

    private void Awake() {
        Instance = this;
    }

    public void Add(Item item) {
        Items.Add(item);
        loadItems();
    }

    public void Remove(int index) {
        Items.RemoveAt(index);
        loadItems();
    }

    public void loadItems() {
        for (int i = slotParent.transform.childCount - 1; i >= 0; i--) {
            Destroy(slotParent.transform.GetChild(i).gameObject);
        }
        closeButtons.Clear();

        for (int i = 0; i < Items.Count; i++) {
            GameObject newSlot = Instantiate(slot, transform.position, transform.rotation); // Creates a slot
            newSlot.transform.SetParent(slotParent.transform); // Sets slot to parent

            if (removeItems.isOn) {
                newSlot.transform.GetChild(2).gameObject.SetActive(true); // Shows closeButton
            } else {
                newSlot.transform.GetChild(2).gameObject.SetActive(false);
            }

            closeButtons.Add(newSlot.transform.GetChild(2).gameObject.GetComponent<Button>()); // Adds closeButton object to list

            newSlot.transform.GetChild(1).gameObject.SetActive(true); // Shows background
            newSlot.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Items[i].icon; // Changes sprite icon
            
            ItemRemover.Instance.index = i;
        }

        for (int i = Items.Count; i < 20; i++) {
            GameObject newSlot = Instantiate(slot, transform.position, transform.rotation); // Creates a slot
            newSlot.transform.SetParent(slotParent.transform); // Sets slot to parent

            ItemRemover.Instance.index = i;
        }
    }
}
