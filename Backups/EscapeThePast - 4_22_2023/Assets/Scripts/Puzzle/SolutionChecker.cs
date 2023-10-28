using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SolutionChecker : MonoBehaviour
{
    public static SolutionChecker Instance;

    public Text heartsText;
    public int hearts;

    void Awake() {
        Instance = this;
    }

    void Start() {
        heartsText.text = $"Hearts: {hearts}";
    }

    public bool Check(int index, ref bool wrongGuess) { // Called by "SlotSwitcher.cs"
        if (hearts <= 0) { return false; }

        string mapString = string.Join("", PuzzleGenerator.Instance.mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));

        if (mapString[index] == '-' && !PuzzleGenerator.Instance.labelWrong.isOn && !PuzzleGenerator.Instance.labelRight.isOn && !PuzzleGenerator.Instance.GetIndexInGrid(index).wrongLabel && !PuzzleGenerator.Instance.GetIndexInGrid(index).rightLabel) {
            wrongGuess = true;
            hearts--;
        }
        heartsText.text = $"Hearts: {hearts}";

        if (hearts <= 0) {
            if (hearts == 0) { PuzzleGenerator.Instance.slotSwitch(index, wrongGuess); }
            return false;
        } else {
            return true;
        }
    }
}
