using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseButton : MonoBehaviour
{
    public bool isAvailable;
    public int menuId;
    public GameObject uiParent;

    public void ChoiceMade() {
        int choiceIndex = gameObject.transform.GetSiblingIndex();
        Debug.Log($"You chose index {choiceIndex}, which availability is {isAvailable}");
        
        if (!isAvailable) {
            StartCoroutine(writeError(gameObject.transform.parent.transform.parent.GetChild(1).GetChild(gameObject.transform.GetSiblingIndex()).transform.GetComponent<Text>().text)); // Gets the label of pressed button
        } else {
            switch (menuId) {
                case 0: // Prison doors
                    PuzzleInteraction.Instance.Interact(1);
                    break;
                case 1: // Pots choice
                    
                    choiceDatabase.Instance.noGoingBackBarriers.GetChild(0).gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                    Destroy(choiceDatabase.Instance.potsBarrier);
                    switch (choiceIndex) {
                        case 0: // redPot
                            InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.redPot)); // Uses redPot
                            InventoryManager.Instance.Add(choiceDatabase.Instance.armor); // Adds armor to inventory
                            InventoryManager.Instance.Add(choiceDatabase.Instance.spear); // Adds spear to inventory
                            // Make guards * dead *

                            foreach (GameObject guard in choiceDatabase.Instance.guards) {
                                guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.skull; // Updates status to dead
                                guard.transform.GetChild(1).gameObject.SetActive(false); // Removes vision field
                                guard.transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255); // Changes color
                            }

                            PuzzleInteraction.Instance.DisplayStatus("Oh, something really serious happened here.\n\nYou better get out of there as soon as possible...\n\nAt least you got yourself some equipment, don't forget to check the inventory.");
                            break;
                        case 1: // bluePot
                            InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.bluePot)); // Uses bluePot
                            InventoryManager.Instance.Add(choiceDatabase.Instance.armor); // Adds armor to inventory
                            InventoryManager.Instance.Add(choiceDatabase.Instance.spear); // Adds spear to inventory
                            // Make guards * asleep *

                            foreach (GameObject guard in choiceDatabase.Instance.guards) {
                                guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.sleep; // Updates status to sleeping
                                guard.transform.GetChild(1).gameObject.SetActive(false); // Removes vision field
                                guard.transform.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 0, 255); // Changes color
                            }

                            PuzzleInteraction.Instance.DisplayStatus("Aww, look at these sleeping babies!\n\nSweet and not even knowing you just stole their stuff into your inventory!");
                            break;
                        case 2: // greenPot
                            InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.greenPot)); // Uses greenPot
                            choiceDatabase.Instance.player.GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.outlineCircle; // Makes player invisible
                            choiceDatabase.Instance.player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50); // Makes player invisible

                            foreach (GameObject guard in choiceDatabase.Instance.guards) {
                                guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.questionMark; // Updates status to clueless
                                guard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 100); // Changes vision field color
                                guard.transform.GetChild(1).GetComponent<PolygonCollider2D>().isTrigger = true; // Turns vision field into trigger (so player can walk through it)
                                guard.transform.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255); // Changes color
                            }

                            PuzzleInteraction.Instance.DisplayStatus("You are invisible now. That's kinda it...\n\nIf you weren't such a chicken, you maybe could get some stuff to yourself too...\n\nWell, maybe next time...");
                            break;
                    }

                    break;
                case 2: // King's room chest

                    switch (choiceIndex) {
                        case 0:
                            PuzzleGenerator.Instance.SetValues(10);
                            // Spear
                            break;
                        case 1:
                            PuzzleGenerator.Instance.SetValues(11);
                            // Key
                            break;
                    }

                    break;
                case 3: // King's room chest
                    PuzzleGenerator.Instance.SetValues(11);
                    // Key
                    break;
                case 4: // Caught by guards
                    Debug.Log("Activated!");

                    switch (choiceIndex) {
                        case 0:
                            InventoryManager.Instance.Remove(InventoryManager.Instance.Items.IndexOf(choiceDatabase.Instance.redPot)); // Uses redPot
                            InventoryManager.Instance.Add(choiceDatabase.Instance.armor); // Adds armor to inventory
                            InventoryManager.Instance.Add(choiceDatabase.Instance.spear); // Adds spear to inventory
                            // Make guards * dead *

                            foreach (GameObject guard in choiceDatabase.Instance.guards) {
                                guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.skull; // Updates status to dead
                                guard.transform.GetChild(1).gameObject.SetActive(false); // Removes vision field
                                guard.transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255); // Changes color
                            }

                            PuzzleInteraction.Instance.DisplayStatus("Phew, that was a close one!\n\nYou better get out of there as soon as possible...\n\nAt least you got yourself some equipment, don't forget to check the inventory.");
                            break;
                        case 1:
                            PuzzleInteraction.Instance.DisplayReturnStatus("Well, they caught you...\n\nI guess you made everything just worse by trying to escape.\n\nMaybe in a next life...");
                            AchievementManager.Instance.OpenAchievement(0);
                            break;
                    }

                    break;
                case 5: // Destroying fence
                    PuzzleGenerator.Instance.SetValues(14);
                    // Using axe
                    break;
            }

            Destroy(this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject);
        }
    }

    int ongoingErrorMessages = 0;
    IEnumerator writeError(string inputValue) {
        ChooseMenu.Instance.errorText.text = $"\"{inputValue}\" option is not available at this moment...";

        ongoingErrorMessages++;
        yield return new WaitForSeconds(5);
        ongoingErrorMessages--;

        if (ongoingErrorMessages == 0) {
            ChooseMenu.Instance.errorText.text = "";
        }
    }
}
