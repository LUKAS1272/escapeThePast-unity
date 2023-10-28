using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    Rigidbody2D rb;
    public GameObject uis;
    float speed = 150f;
    public GameObject interactText;

    void Awake() {
        Instance = this;
        uis.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
    }

    // Player movement
    void FixedUpdate() {
        interactText.SetActive(false);

        bool doUpdate = true;

        for (int i = 0; i < uis.transform.childCount; i++) {
            if (uis.transform.GetChild(i).gameObject.activeInHierarchy) {
                doUpdate = false;
                break;
            }
        }

        if (doUpdate) {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime * speed;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    public IndexSetter currentColider;
    public bool hasInteracted = false;
    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag("Item") && Input.GetKey("i") && !hasInteracted) {

            Debug.Log("Item pickUp");

            col.gameObject.GetComponent<ItemPickUp>().PickUp();
            StartCoroutine(updateStopI());
        } else if (col.gameObject.CompareTag("PuzzlePoint") && Input.GetKey("i") && !hasInteracted && !col.gameObject.GetComponent<IndexSetter>().isSolved) {
            if (!PuzzleInteraction.Instance.colliderObejcts.ContainsKey(col.gameObject.GetComponent<IndexSetter>().index)) {
                PuzzleInteraction.Instance.colliderObejcts.Add(col.gameObject.GetComponent<IndexSetter>().index, col.gameObject);
            }

            currentColider = col.gameObject.GetComponent<IndexSetter>();
            
            Debug.Log("Interacted");
            PuzzleInteraction.Instance.Interact(col.gameObject.GetComponent<IndexSetter>().index);
        }

        if (col.gameObject.CompareTag("Item") || col.gameObject.CompareTag("PuzzlePoint") && !col.gameObject.GetComponent<IndexSetter>().isSolved) {
            interactText.SetActive(true);
        }
    }

    // Generates choose menu
    void OnCollisionEnter2D(Collision2D col) {
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
