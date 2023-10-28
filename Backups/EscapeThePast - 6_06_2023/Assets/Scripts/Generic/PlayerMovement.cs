using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    Rigidbody2D rb;
    public GameObject uis;
    float speed = 150f;

    void Awake() {
        Instance = this;
        uis.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
    }

    // Player movement
    void FixedUpdate() {
        bool doUpdate = true;

        for (int i = 0; i < uis.transform.childCount; i++) {
            if (uis.transform.GetChild(i).gameObject.activeInHierarchy) {
                doUpdate = false;
                break;
            }
        }

        if (doUpdate) {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime * speed;
            hasInteracted = false;
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    public IndexSetter currentColider;
    bool hasInteracted = false;
    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag("Item") && Input.GetKey("i") && !hasInteracted) {
            Debug.Log("Item pickUp");
            hasInteracted = true;

            col.gameObject.GetComponent<ItemPickUp>().PickUp();
        } else if (col.gameObject.CompareTag("PuzzlePoint") && Input.GetKey("i") && !hasInteracted && !col.gameObject.GetComponent<IndexSetter>().isSolved) {
            if (!PuzzleInteraction.Instance.colliderObejcts.ContainsKey(col.gameObject.GetComponent<IndexSetter>().index)) {
                PuzzleInteraction.Instance.colliderObejcts.Add(col.gameObject.GetComponent<IndexSetter>().index, col.gameObject);
            }

            currentColider = col.gameObject.GetComponent<IndexSetter>();
            hasInteracted = true;
            
            Debug.Log("Interacted");
            PuzzleInteraction.Instance.Interact(col.gameObject.GetComponent<IndexSetter>().index);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("ChoosePoint")) {
            ChooseMenu.Instance.GenerateChoice(col.gameObject.GetComponent<ChooseMenuID>().id);
        }
    }
}
