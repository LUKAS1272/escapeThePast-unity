using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleInteraction : MonoBehaviour
{
    public static PuzzleInteraction Instance;
    void Awake() { Instance = this; }

    public Item branch, redPot, bluePot, greenPot;
    public GameObject doors, openedDoors, statusUi;


    public Dictionary<int, GameObject> colliderObejcts = new Dictionary<int, GameObject>();

    public void GiveReward(int rewardIndex) {
        switch (rewardIndex) {
            case 0:
                DisplayStatus("Lockpicking is a must have skill!\n\nDon't forget to be silent, so you don't wake the guard!");
                doors.SetActive(false);
                openedDoors.SetActive(true);
                InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(branch));
                break;
            case 1:
                DisplayStatus("Hm, looks like you just destroyed a wall with your bare hands\n\nDon't ask me how, I'm not paid enough to add proper logic to this game...");
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 2:
                DisplayStatus("Nice, you just found a branch in this pile of hay!\n\nMaybe it could be handy later...");
                InventoryManager.Instance.Add(branch);
                break;
            case 3:
                DisplayStatus("Well, well, well, isn't this a strength potion?\n\nI guess our fellow guards would really appreciate it!");
                InventoryManager.Instance.Add(redPot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 4:
                DisplayStatus("This pot is making me really sleepy...\n\nPerhaps we could use it to our advantage.");
                InventoryManager.Instance.Add(bluePot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 5:
                DisplayStatus("This bottle looks a bit transparent.\n\nIt would be great, if it made you transparent too!");
                InventoryManager.Instance.Add(greenPot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 6:
                DisplayStatus("Oh no, the spear just broke!\n\nSeems like the only way to get into this chest is the proper one...");
                choiceDatabase.Instance.chest.GetComponent<ChooseMenuID>().id = 3;
                InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.spear));
                break;
            case 7:
                Debug.Log("Hidden in wardrobe");
                break;
        }
    }

    public void Interact(int index) {
        if (PuzzleGenerator.Instance.mapLoaded == index) {
            PuzzleTimer.Instance.runTimer = true;
            UIAction.Instance.puzzle.SetActive(true);
        } else {
            PuzzleGenerator.Instance.SetValues(index);
        }
    }

    public void DisplayStatus(string statusMessage) {
        statusUi.SetActive(true);
        statusUi.transform.GetChild(1).GetComponent<Text>().text = statusMessage;
    }
}