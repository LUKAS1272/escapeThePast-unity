using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAction : MonoBehaviour
{
    public GameObject inventory, puzzle, mainUis;
    bool runUpdate = true;

    public static UIAction Instance;
    void Awake() { Instance = this; }

    void Start() {
        for (int i = 0; i < mainUis.transform.childCount; i++) {
            mainUis.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {
        if (runUpdate && Input.GetKey("e")) {
            if (FirstActiveUI.Instance.Run() == -1) {
                inventory.SetActive(true);
                InventoryManager.Instance.loadItems();
            } else if (FirstActiveUI.Instance.Run() == 0) {
                inventory.SetActive(false);
            }

            StartCoroutine(WaitForRelease(KeyCode.E));
        }
    }

    IEnumerator WaitForRelease(KeyCode key) {
        runUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        runUpdate = true;
    }
}
