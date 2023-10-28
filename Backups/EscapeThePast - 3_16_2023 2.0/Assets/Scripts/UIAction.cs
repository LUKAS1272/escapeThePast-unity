using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAction : MonoBehaviour
{
    public GameObject inventory;
    public GameObject puzzle;
    bool runUpdate = true;

    public static UIAction Instance;

    void Awake() {
        Instance = this;
    }

    void Start() {
        inventory.SetActive(false);
        puzzle.SetActive(false);
    }

    void FixedUpdate() {
        if (runUpdate) {

            if (Input.GetKey("e") && !puzzle.activeInHierarchy) {
                inventory.SetActive(!inventory.activeInHierarchy);
                StartCoroutine(updateStopE());
                InventoryManager.Instance.loadItems();
            } else if (Input.GetKey("q") && !inventory.activeInHierarchy) {
                puzzle.SetActive(!puzzle.activeInHierarchy);
                StartCoroutine(updateStopI());
            }
            
        }
    }

    IEnumerator updateStopE() {
        runUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp("e"));
        runUpdate = true;
    }

    IEnumerator updateStopI() {
        runUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp("q"));
        runUpdate = true;
    }
}
