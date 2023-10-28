using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSwitcher : MonoBehaviour
{
    public static SlotSwitcher Instance;
    public int index = -1;
    public bool isOn = false;
    public bool wrongGuess = false;
    public bool rightLabel = false;
    public bool wrongLabel = false;

    void Awake() {
        Instance = this;
    }

    public void Clicked() {
        if (SolutionChecker.Instance.Check(index, ref wrongGuess) && !PuzzleGenerator.Instance.isSolved) { PuzzleGenerator.Instance.slotSwitch(index, wrongGuess); }
    }
}