using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteraction : MonoBehaviour
{
    public static PuzzleInteraction Instance;
    void Awake() { Instance = this; }

    public Item branch, redPot, bluePot, greenPot;
    public GameObject doors, openedDoors;


    public Dictionary<int, GameObject> colliderObejcts = new Dictionary<int, GameObject>();

    public void GiveReward(int rewardIndex) {
        switch (rewardIndex) {
            case 0:
                Debug.Log("Door puzzle was solved");
                doors.SetActive(false);
                openedDoors.SetActive(true);
                break;
            case 1:
                Debug.Log("Wall puzzle was solved");
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 2:
                Debug.Log("Branch collected");
                InventoryManager.Instance.Add(branch);
                break;
            case 3:
                Debug.Log("Red potion collected");
                InventoryManager.Instance.Add(redPot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 4:
                Debug.Log("Green potion collected");
                InventoryManager.Instance.Add(bluePot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 5:
                Debug.Log("Blue potion collected");
                InventoryManager.Instance.Add(greenPot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 6:
                Debug.Log("Chest was looted");
                break;
            case 7:
                Debug.Log("Hidden in wardrobe");
                break;
        }
    }

    public void Interact(int index) {
        if (PuzzleGenerator.Instance.mapLoaded == index) {
            UIAction.Instance.puzzle.SetActive(true);
            return;
        }

        switch (index) {
            case 0: // Prison doors
                if (PuzzleInteraction.Instance.colliderObejcts.ContainsKey(2) && PuzzleInteraction.Instance.colliderObejcts[2].GetComponent<IndexSetter>().isSolved) {
                    PuzzleGenerator.Instance.SetValues(index);
                } else {
                    Debug.Log("Find a branch first");
                }
                break;
            default:
                PuzzleGenerator.Instance.SetValues(index);
                break;
        }
    }
}