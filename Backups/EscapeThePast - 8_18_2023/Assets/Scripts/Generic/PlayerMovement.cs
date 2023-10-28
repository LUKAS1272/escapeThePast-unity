using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    Rigidbody2D rb;
    public GameObject uis;
    float speed = 150f;
    int uiActive = -1;
    public GameObject interactText;

    void Awake() {
        Instance = this;
        uis.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        interactText.SetActive(false); // Interaction text toggle

        uiActive = -1;
        for (int i = 0; i < uis.transform.childCount; i++) { // Checks, whether there is some toggled UI
            if (uis.transform.GetChild(i).gameObject.activeInHierarchy) {
                uiActive = i;
                break;
            }
        }

        if (uiActive == -1) { // Player movement
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime * speed;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    public IndexSetter currentColider;
    public bool hasInteracted = false;
    void OnTriggerStay2D(Collider2D col) {
        if (Input.GetKey("i") && !hasInteracted && uiActive == -1) { // If 'i' is pressed
            if (col.gameObject.CompareTag("Item")) {
                col.gameObject.GetComponent<ItemPickUp>().PickUp();
            } else if (col.gameObject.CompareTag("PuzzlePoint") && !col.gameObject.GetComponent<IndexSetter>().isSolved) {
                if (!PuzzleInteraction.Instance.colliderObejcts.ContainsKey(col.gameObject.GetComponent<IndexSetter>().index)) {
                    PuzzleInteraction.Instance.colliderObejcts.Add(col.gameObject.GetComponent<IndexSetter>().index, col.gameObject);
                }

                currentColider = col.gameObject.GetComponent<IndexSetter>();
                PuzzleInteraction.Instance.Interact(col.gameObject.GetComponent<IndexSetter>().index);
            }

            StartCoroutine(updateStopI());
        }

        if (col.gameObject.CompareTag("Item") || col.gameObject.CompareTag("PuzzlePoint") && !col.gameObject.GetComponent<IndexSetter>().isSolved) { 
            interactText.SetActive(true); // Interaction text toggle
        }
    }

    void OnCollisionEnter2D(Collision2D col) { // Generates choose menu
        uiActive = -1;
        for (int i = 0; i < uis.transform.childCount; i++) { // Checks, whether there is some toggled UI
            if (uis.transform.GetChild(i).gameObject.activeInHierarchy) {
                uiActive = i;
                break;
            }
        }

        if (col.gameObject.CompareTag("ChoosePoint") && uiActive == -1) {
            ChooseMenu.Instance.GenerateChoice(col.gameObject.GetComponent<ChooseMenuID>().id, col.gameObject.GetComponent<ChooseMenuID>().canClose); // Generates choice menu

            if (col.gameObject.GetComponent<ChooseMenuID>().id == 4) { // If guards see the player
                foreach (GameObject guard in choiceDatabase.Instance.guards) {
                    PuzzleInteraction.Instance.DisplayStatus("Oh no, the guards caught you!\n\nThe only thing you can do is try fighting back with some strength potion.\n\nOtherwise you have to give up...");
                    guard.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255); // Changes guard color to red
                    guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.exclamationMark; // Changes status to angry
                    guard.transform.GetChild(1).gameObject.GetComponent<PolygonCollider2D>().isTrigger = true; // Sets view field to trigger
                    guard.transform.GetChild(1).transform.localScale = new Vector3((float)0.5, (float)0.35, (float)0.1); // Makes the view field bigger
                    guard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 100); // Changes the view field color
                }
            }
        }
    }

    public bool secondPortalTriggered = false;
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject == choiceDatabase.Instance.returnPortal && choiceDatabase.Instance.returnPortal.GetComponent<BoxCollider2D>().isTrigger && !secondPortalTriggered) {
            choiceDatabase.Instance.returnPortal.GetComponent<BoxCollider2D>().isTrigger = false;
            PuzzleInteraction.Instance.DisplayStatus("If you would come back to the secret room, someone could see you.\n\nSo, I would not realy recommend that...");
            GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.circle; // Makes player visible
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); // Makes player visible

            if (choiceDatabase.Instance.guards[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite != choiceDatabase.Instance.skull) {
                foreach (GameObject guard in choiceDatabase.Instance.guards) {
                    guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null; // Removes status
                    guard.transform.GetChild(1).gameObject.SetActive(true); // Adds vision field
                    guard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(0, 132, 255, 100); // Changes vision field color
                    guard.transform.GetChild(1).GetComponent<PolygonCollider2D>().isTrigger = false; // Turns vision field into collider (so player can't walk through it)
                    guard.transform.GetComponent<SpriteRenderer>().color = new Color32(0, 132, 255, 255); // Changes color
                }
            }

        } else if (col.gameObject == choiceDatabase.Instance.secondPortal) {
            secondPortalTriggered = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == choiceDatabase.Instance.secondPortal) {
            secondPortalTriggered = true;
        }
    }

    IEnumerator updateStopI() {
        hasInteracted = true;
        yield return new WaitUntil(() => Input.GetKeyUp("i"));
        hasInteracted = false;
    }
}
