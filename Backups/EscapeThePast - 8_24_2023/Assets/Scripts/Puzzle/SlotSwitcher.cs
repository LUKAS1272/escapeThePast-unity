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
        SolutionChecker.Instance.Check(index, ref wrongGuess);

        if (!PuzzleGenerator.Instance.isSolved) {
            PuzzleGenerator.Instance.slotSwitch(index, wrongGuess);
        }
    }
}