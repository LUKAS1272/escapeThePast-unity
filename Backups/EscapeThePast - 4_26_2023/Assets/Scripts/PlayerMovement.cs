using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public Item branch;

    Rigidbody2D rb;
    public GameObject uis;
    float speed = 150f;
    public Dictionary<int, GameObject> colliderObejcts = new Dictionary<int, GameObject>();

    void Awake() {
        Instance = this;
        uis.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() { // Player movement
        if (!uis.transform.GetChild(0).gameObject.activeInHierarchy && !uis.transform.GetChild(1).gameObject.activeInHierarchy && !uis.transform.GetChild(2).gameObject.activeInHierarchy) {
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
            if (!colliderObejcts.ContainsKey(col.gameObject.GetComponent<IndexSetter>().index)) {
                colliderObejcts.Add(col.gameObject.GetComponent<IndexSetter>().index, col.gameObject);
            }

            currentColider = col.gameObject.GetComponent<IndexSetter>();
            hasInteracted = true;
            
            Debug.Log("Interacted");
            if (PuzzleGenerator.Instance.mapLoaded != col.gameObject.GetComponent<IndexSetter>().index) {
                PuzzleGenerator.Instance.SetValues(col.gameObject.GetComponent<IndexSetter>().index);
            } else {
                UIAction.Instance.puzzle.SetActive(true);
            }
            
        }
    }

    public void GiveReward(int rewardIndex) {
        switch (rewardIndex) {
            case 0:
                Debug.Log("Door puzzle was solved");
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 1:
                Debug.Log("Wall puzzle was solved");
                colliderObejcts[rewardIndex].SetActive(false);
                break;
            case 2:
                Debug.Log("Branch collected");
                InventoryManager.Instance.Add(branch);
                break;
        }
    }
}
