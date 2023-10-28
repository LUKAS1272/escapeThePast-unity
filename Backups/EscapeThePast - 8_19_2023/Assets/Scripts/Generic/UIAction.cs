using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAction : MonoBehaviour
{
    public GameObject inventory, puzzle, choose, mainUis;
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
            int uiActive = -1;
            for (int i = mainUis.transform.childCount - 1; i >= 0; i--) {
                if (mainUis.transform.GetChild(i).gameObject.activeInHierarchy) {
                    uiActive = i;
                    break;
                }
            }

            if (uiActive == -1) {
                inventory.SetActive(true);
                InventoryManager.Instance.loadItems();
            } else if (uiActive == 0) {
                inventory.SetActive(false);
            }

            StartCoroutine(updateStopE());
        }
    }

    IEnumerator updateStopE() {
        runUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp("e"));
        runUpdate = true;
    }
}
