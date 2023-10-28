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
        if (col.gameObject.CompareTag("ChoosePoint")) {
            ChooseMenu.Instance.GenerateChoice(col.gameObject.GetComponent<ChooseMenuID>().id);
        }
    }

    IEnumerator updateStopI() {
        hasInteracted = true;
        yield return new WaitUntil(() => Input.GetKeyUp("i"));
        hasInteracted = false;
    }
}
