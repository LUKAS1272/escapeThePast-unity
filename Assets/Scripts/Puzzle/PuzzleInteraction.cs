using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleInteraction : MonoBehaviour
{
    public static PuzzleInteraction Instance;
    void Awake() { Instance = this; }

    public Item branch, redPot, bluePot, greenPot;
    public GameObject doors, openedDoors, window, statusUi, openUi, returnStatusUi;


    public Dictionary<int, GameObject> colliderObejcts = new Dictionary<int, GameObject>();

    public void GiveReward(int rewardIndex) {
        switch (rewardIndex) {
            case 0: // Haybale
                DisplayStatus("Nice, you just found a branch in this pile of hay!\n\nMaybe it could be handy later...");
                InventoryManager.Instance.Add(branch);
                break;
            case 1: // Lockpick
                DisplayStatus("Lockpicking is a must have skill!\n\nDon't forget to be silent, so you don't wake the guard!");
                doors.SetActive(false);
                openedDoors.SetActive(true);
                InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(branch));
                break;
            case 2: // Fake wall
                DisplayStatus("Hm, looks like you just destroyed a wall with your bare hands\n\nDon't ask me how, I'm not paid enough to add proper logic to this game...");
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 3: // Sleeping guard
                InventoryManager.Instance.Add(choiceDatabase.Instance.keys);
                DisplayStatus("Let's see, what do we have here...\n\nOh, keys! I'm wondering what could these open.");
                break;
            case 4: // Strength potion
                DisplayStatus("Well, well, well, isn't this a strength potion?\n\nI guess our fellow guards would really appreciate it!");
                InventoryManager.Instance.Add(redPot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 5: // Sleeping potion
                DisplayStatus("This pot is making me really sleepy...\n\nPerhaps we could use it to our advantage.");
                InventoryManager.Instance.Add(bluePot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 6: // Invisibility potion
                DisplayStatus("This bottle looks a bit transparent.\n\nIt would be great, if it made you transparent too!");
                InventoryManager.Instance.Add(greenPot);
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 7: // Axe
                InventoryManager.Instance.Add(choiceDatabase.Instance.axe);
                DisplayStatus("Huh, an axe. Too bad I don't see any wood anywhere near...");
                break;
            case 8: // Spear
                InventoryManager.Instance.Add(choiceDatabase.Instance.spear);
                DisplayStatus("A spear! I heard those things are good for getting into narrow areas!");
                break;
            case 9: // Sword
                InventoryManager.Instance.Add(choiceDatabase.Instance.sword);
                DisplayStatus("Oi! A golden sword!\n\nHopefuly it won't break after the first use.");
                break;
            case 10: // Breaking into chest
                DisplayStatus("Oh no, the spear just broke!\n\nSeems like the only way to get into this chest is the proper one...");
                choiceDatabase.Instance.chest.GetComponent<ChooseMenuID>().id = 3;
                InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.spear));
                break;
            case 11: // Unlocking chest
                DisplayReturnStatus("So this is where the king stored your time machine.\n\nThe good thing is you keep some coins on top of it!\n\nThe bad thing is, you can't escape through window anymore...");
                InventoryManager.Instance.Add(choiceDatabase.Instance.timeMachine);
                InventoryManager.Instance.Add(choiceDatabase.Instance.coins);
                InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.keys));
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(2).gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(2).gameObject.GetComponent<StatusText>().doRun = false;
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(3).gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(3).gameObject.GetComponent<StatusText>().doRun = true;
                choiceDatabase.Instance.guards[2].SetActive(true);
                choiceDatabase.Instance.guards[3].SetActive(true);
                Destroy(window.GetComponent<IndexSetter>());
                break;
            case 12: // Breaking a window
                Destroy(window);
                Destroy(choiceDatabase.Instance.chest.GetComponent<ChooseMenuID>());
                choiceDatabase.Instance.chest.tag = "Untagged";
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(1).gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(1).gameObject.GetComponent<StatusText>().doRun = true;
                DisplayStatus("Oh, you broke the window!\n\nNow, get away as soon as possible!");
                break;
            case 13:
                DisplayReturnStatus("Phew, you managed to escape. However, you don't have the time machine...\n\nSo I guess, you're staying in medieval forever.");
                AchievementManager.Instance.OpenAchievement(1);
                break;
            case 14:
                choiceDatabase.Instance.mazeUi.transform.GetChild(0).gameObject.SetActive(false);
                choiceDatabase.Instance.mazeSolution.SetActive(true);
                GenericMethods.Instance.PlayerToMazeStart();
                DisplayStatus("Well, you have the right path highlighted, so don't mess it up!");
                break;
        }
    }

    public void Interact(int index) {
        if (PuzzleGenerator.Instance.mapLoaded == index) {
            openUi.SetActive(true);
        } else {
            PuzzleGenerator.Instance.SetValues(index);
        }
    }

    public void Interact2(bool doContinue) {
        if (doContinue) {
            PuzzleTimer.Instance.runTimer = true;
            UIAction.Instance.puzzle.SetActive(true);
        } else {
            PuzzleGenerator.Instance.SetValues(PuzzleGenerator.Instance.mapLoaded);
        }
    }

    public void DisplayStatus(string statusMessage) {
        statusUi.SetActive(true);
        statusUi.transform.GetChild(2).GetComponent<Text>().text = statusMessage;
    }

    public void DisplayReturnStatus(string statusMessage) {
        GameObject returnStatus = Instantiate(returnStatusUi, transform.position, transform.rotation);
        returnStatus.transform.GetChild(1).GetComponent<Text>().text = statusMessage;
        returnStatus.transform.SetParent(openUi.transform.parent);
    }
}