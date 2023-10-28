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

        if (FirstActiveUI.Instance.Run() == -1) { // Player movement
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime * speed;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    public IndexSetter currentColider;
    public bool hasInteracted = false;
    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag("Item") || col.gameObject.CompareTag("PuzzlePoint") || col.gameObject.CompareTag("Status")) {
            if (Input.GetKey("i") && !hasInteracted && uiActive == -1) { // If 'i' is pressed
                if (col.gameObject.CompareTag("Item")) {
                    col.gameObject.GetComponent<ItemPickUp>().PickUp();
                } else if (col.gameObject.CompareTag("PuzzlePoint") && !col.gameObject.GetComponent<IndexSetter>().isSolved) {
                    if (!PuzzleInteraction.Instance.colliderObejcts.ContainsKey(col.gameObject.GetComponent<IndexSetter>().index)) {
                        PuzzleInteraction.Instance.colliderObejcts.Add(col.gameObject.GetComponent<IndexSetter>().index, col.gameObject);
                    }

                    currentColider = col.gameObject.GetComponent<IndexSetter>();
                    PuzzleInteraction.Instance.Interact(col.gameObject.GetComponent<IndexSetter>().index);
                } else if (col.gameObject.GetComponent<StatusText>() != null && col.gameObject.GetComponent<StatusText>().doRun) {
                    PuzzleInteraction.Instance.DisplayStatus(col.gameObject.GetComponent<StatusText>().text);
                }

                StartCoroutine(WaitForRelease(KeyCode.I));
            }

            if (col.gameObject.GetComponent<StatusText>() != null && col.gameObject.GetComponent<StatusText>().doRun) { 
                interactText.SetActive(true); // Interaction text toggle
            }
        }

        
    }

    void OnCollisionEnter2D(Collision2D col) { // Generates choose menu
        if (col.gameObject.CompareTag("ChoosePoint") && FirstActiveUI.Instance.Run() == -1) {
            ChooseMenu.Instance.GenerateChoice(col.gameObject.GetComponent<ChooseMenuID>().id, col.gameObject.GetComponent<ChooseMenuID>().canClose); // Generates choice menu
        }
    }

    IEnumerator WaitForRelease(KeyCode key) {
        hasInteracted = true;
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        hasInteracted = false;
    }
}
