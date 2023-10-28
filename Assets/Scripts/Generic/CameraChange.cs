using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public CircleCollider2D player;
    public GameObject oppositePortal;
    public Vector3 oppositeCameraPosition = new Vector3(0, 0, -10);
    public int oppositeCameraSize = 5;
    public Vector3 currentCameraPosition = new Vector3(0, 0, -10);
    bool firstSwitch = true;

    void Start() {
        Physics2D.IgnoreCollision(player, GetComponent<BoxCollider2D>());
    }

    bool isColliding = false;
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            isColliding = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && oppositePortal.GetComponent<CameraChange>().isColliding) {
            isColliding = false;
            Camera.main.GetComponent<Transform>().position = oppositeCameraPosition;
            Camera.main.orthographicSize = oppositeCameraSize;
            // Camera switch

            if (oppositeCameraPosition == new Vector3(-18, 15, -10) && oppositePortal.GetComponent<CameraChange>().oppositeCameraPosition == new Vector3(-18, 5, -10) && firstSwitch) {
                choiceDatabase.Instance.returnPortal.GetComponent<BoxCollider2D>().isTrigger = false;
                PuzzleInteraction.Instance.DisplayStatus("If you would come back to the secret room, someone could see you.\n\nSo, I would not realy recommend that...");
                choiceDatabase.Instance.player.GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.circle; // Makes player visible
                choiceDatabase.Instance.player.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); // Makes player visible
                choiceDatabase.Instance.noGoingBackBarriers.GetChild(0).gameObject.GetComponent<BoxCollider2D>().isTrigger = true; // Removes noGoingBackBarrier
                
                if (choiceDatabase.Instance.guards[0].transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite != choiceDatabase.Instance.skull) {
                    Transform guards = choiceDatabase.Instance.guards[0].transform;
                    for (int i = 0; i < 2; i++) {
                        guards.GetChild(i).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null; // Removes status
                        guards.GetChild(i).GetChild(1).gameObject.SetActive(true); // Adds vision field
                        guards.GetChild(i).GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 132, 255, 100); // Changes vision field color
                        guards.GetChild(i).GetChild(1).gameObject.GetComponent<PolygonCollider2D>().isTrigger = false; // Turns vision field into collider (so player can't walk through it)
                        guards.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 132, 255, 255); // Changes color
                    }
                }
            } else if (oppositeCameraPosition == new Vector3(-29, 30, -10) && oppositePortal.GetComponent<CameraChange>().oppositeCameraPosition == new Vector3(-36, 15, -10)) {
                if (firstSwitch) {
                    PuzzleInteraction.Instance.DisplayStatus("Well, when i said these castles are like a labyrinth, I meant it as a metaphor.\n\n\nNOT LITERALY!\n\nLike how did this even get here?");
                }

                choiceDatabase.Instance.mazeUi.SetActive(true);
            } else if (oppositeCameraPosition == new Vector3(-51, 45, -10) && oppositePortal.GetComponent<CameraChange>().oppositeCameraPosition == new Vector3(-29, 30, -10) && firstSwitch) {
                choiceDatabase.Instance.mazeUi.transform.GetChild(0).gameObject.SetActive(false);
                choiceDatabase.Instance.mazeUi.transform.GetChild(1).gameObject.SetActive(false);
                
                if (choiceDatabase.Instance.mazeSolution.activeInHierarchy) {
                    int insNum = Random.Range(0, 6);
                    string insString = "";
                    switch (insNum) {
                        case 0:
                            insString = "You're like a human GPS with a 'Lost in 404 Error' mode when it comes to labyrinths!";
                            break;
                        case 1:
                            insString = "If labyrinths had a 'Hall of Shame,' your portrait would be the centerpiece!";
                            break;
                        case 2:
                            insString = "You must have a PhD in Labyrinthology, specializing in getting stuck!";
                            break;
                        case 3:
                            insString = "You must be the reason they put 'Exit' signs in labyrinths – just for folks like you!";
                            break;
                        case 4:
                            insString = "It's a-maze-ing how you can make even the simplest labyrinth seem like a cosmic puzzle!";
                            break;
                        case 5:
                            insString = "You couldn't find your way out of a paper bag, let alone a labyrinth!";
                            break;
                    }

                    PuzzleInteraction.Instance.DisplayStatus("Finally, what took you so long to escape?\n\n" + insString);
                } else {
                    PuzzleInteraction.Instance.DisplayStatus("Oh, you managed to find the exit?\n\nGood job out there!");
                    choiceDatabase.Instance.mazeSolution.SetActive(true);
                }
            }

            if (oppositeCameraPosition != new Vector3(-29, 30, -10)) {
                choiceDatabase.Instance.mazeUi.SetActive(false);
            }

            firstSwitch = false;
        }
    }
}