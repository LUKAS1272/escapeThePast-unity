using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAction : MonoBehaviour
{
    public GameObject inventory;
    public GameObject puzzle;
    public GameObject choose;
    bool runUpdate = true;

    public static UIAction Instance;

    void Awake() { Instance = this; }

    void Start() {
        inventory.SetActive(false);
        puzzle.SetActive(false);
        choose.SetActive(false);
    }

    void FixedUpdate() {
        if (runUpdate) {
            if (Input.GetKey("e") && !choose.activeInHierarchy && !puzzle.activeInHierarchy) {
                inventory.SetActive(!inventory.activeInHierarchy);
                StartCoroutine(updateStopE());
                InventoryManager.Instance.loadItems();
            }
        }
    }

    IEnumerator updateStopE() {
        runUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp("e"));
        runUpdate = true;
    }
}
