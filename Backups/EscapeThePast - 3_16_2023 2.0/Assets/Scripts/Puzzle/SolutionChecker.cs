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

        string mapString = string.Join("", PuzzleGenerator.Instance.pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));
        Debug.Log(mapString);
        Debug.Log($"{mapString[index]} : {PuzzleGenerator.Instance.wrongLabels[index]}");

        if (mapString[index] == '-' && !PuzzleGenerator.Instance.labelWrong.isOn && !PuzzleGenerator.Instance.wrongLabels[index]) {
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
