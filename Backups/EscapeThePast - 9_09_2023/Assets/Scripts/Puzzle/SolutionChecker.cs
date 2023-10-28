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

    public void Check(int index, ref bool wrongGuess) { // Called by "SlotSwitcher.cs"
        string mapString = string.Join("", PuzzleGenerator.Instance.mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));

        if (mapString[index] == '-' && !PuzzleGenerator.Instance.GetIndexInGrid(index).wrongLabel) {
            wrongGuess = true;
            hearts--;
        }
        heartsText.text = $"{hearts}";

        PuzzleGenerator.Instance.Continue();
        if (hearts == 0) {
            PuzzleTimer.Instance.StopTimer();
            PuzzleInteraction.Instance.DisplayStatus("Damn man, we messed up...\n\nMaybe next time...");
        }
    }
}
