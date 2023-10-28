using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SolutionChecker : MonoBehaviour
{
    public static SolutionChecker Instance;
    void Awake() { Instance = this; }

    public Text heartsText;
    public int hearts;

    public bool Check(int index, ref bool wrongGuess) { // Called by "SlotSwitcher.cs"
        if (hearts == 0) {
            Debug.Log("SolutionChecker.cs");
            return false;
        }

        string mapString = string.Join("", PuzzleGenerator.Instance.mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));

        if (mapString[index] == '-' && !PuzzleGenerator.Instance.labelWrong.isOn && !PuzzleGenerator.Instance.labelRight.isOn && !PuzzleGenerator.Instance.GetIndexInGrid(index).wrongLabel && !PuzzleGenerator.Instance.isSolved) {
            wrongGuess = true;
            hearts--;
        }
        heartsText.text = $"{hearts}";

        PuzzleGenerator.Instance.Continue();
        if (hearts == 0) {
            PuzzleInteraction.Instance.DisplayStatus("Damn man, we messed up...\n\nMaybe next time...");
            PuzzleGenerator.Instance.slotSwitch(index, wrongGuess);
            return false;
        } else {
            return true;
        }
    }
}
