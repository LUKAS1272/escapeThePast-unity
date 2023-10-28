using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSwitcher : MonoBehaviour
{
    public static SlotSwitcher Instance;
    void Awake() { Instance = this; }
    
    public int index = -1;
    public bool isOn = false;
    public bool wrongGuess = false;
    public bool rightLabel = false;
    public bool wrongLabel = false;

    public void Clicked() {
        if (isOn || wrongGuess || SolutionChecker.Instance.hearts == 0 || PuzzleGenerator.Instance.isSolved) return;

        if (Input.GetMouseButtonDown(0) && !wrongLabel) {
            SolutionChecker.Instance.Check(index, ref wrongGuess);

            if (!wrongGuess) {
                isOn = true;
                rightLabel = false;
            }
        }

        if (Input.GetMouseButtonDown(1) && !rightLabel) { // Label wrong
            wrongLabel = !wrongLabel;
        }

        if (Input.GetMouseButtonDown(2) && !wrongLabel) { // Label right
            rightLabel = !rightLabel;
        }

        SlotSwitch();
    }

    void SlotSwitch() {
        GameObject cross = transform.GetChild(1).gameObject;

        if (wrongGuess) {
            GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            cross.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            cross.SetActive(true);
        } else if (isOn) {
            GetComponent<Image>().color = new Color32(24, 24, 24, 255);
            cross.SetActive(false);
        } else if (rightLabel) {
            GetComponent<Image>().color = new Color32(175, 255, 175, 255);
            cross.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            cross.SetActive(true);
        } else if (wrongLabel) {
            GetComponent<Image>().color = new Color32(255, 204, 203, 255);
            cross.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            cross.SetActive(true);
        } else { // Default state (white square)
            GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            cross.SetActive(false);
        }


        PuzzleGenerator.Instance.CheckSolution();
    }
}