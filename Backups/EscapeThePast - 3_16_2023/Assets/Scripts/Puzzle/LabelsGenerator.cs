using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LabelsGenerator : MonoBehaviour
{
    public static LabelsGenerator Instance;

    static string[] linesArray = File.ReadAllLines($"{PuzzleGenerator.mapPath}/{PuzzleGenerator.mapName}");
    // string[] linesArray;
    int size = linesArray[0].Length;

    public List<List<int>> vertical = new List<List<int>>();
    public List<List<int>> horizontal = new List<List<int>>();

    void Awake() { Instance = this; }

    // void Start() {
    //     // linesArray = PuzzleGenerator.Instance.pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);
    //     // linesArray = PuzzleGenerator.Instance.pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);
    //     size = linesArray[0].Length;
    //     Debug.Log("::: " + linesArray[0]);
    //     Debug.Log($"Size: {size} {linesArray.Length}");
    // }



    bool canExist() {
        foreach (string line in linesArray) {
            if (line.Length != size) {
                return false;
            }
        }

        return true;
    }

    void verticalScan() {
        Debug.Log("42: " + size);

        for (int i = 0; i < size; i++) {
            Debug.Log($"Sizee: {size}");
            vertical.Add(new List<int>());
            int streak = 0;

            for (int j = 0; j < size; j++) {
                if (linesArray[i][j] == '+') {
                   streak++;

                    if (j == size - 1) {
                        vertical[i].Add(streak);
                    }
                } else if (linesArray[i][j] == '-') {
                    if (streak > 0) {
                        vertical[i].Add(streak);
                        streak = 0;
                    }
                }
            }
        }
    }

    void horizontalScan() {
        for (int i = 0; i < size; i++) {
            horizontal.Add(new List<int>());
            int streak = 0;

            for (int j = 0; j < size; j++) {
                if (linesArray[j][i] == '+') {
                    streak++;
                    if (j == size - 1) {
                        horizontal[i].Add(streak);
                    }
                } else if (linesArray[j][i] == '-') {
                if (streak > 0) {
                        horizontal[i].Add(streak);
                        streak = 0;
                    }
                }
            }
        }
    }

    void writeScan(List<List<int>> lists) {
        string itemsString = "";
        foreach (List<int> list in lists) {
            foreach (int item in list) {
                itemsString += item + " ";
            }
            itemsString += "\n";
        }
        Debug.Log(itemsString);
    }

    int longestIn(List<List<int>> lists) {
        int longest = 0;
        foreach (List<int> list in lists) {
            if (list.Count > longest) {
                longest = list.Count;
            }
        }
        return longest;
    }

    public int findLongest() {
        Debug.Log("Run");
        verticalScan();
        horizontalScan();

        Debug.Log("4444444444        " + longestIn(horizontal));
        if (longestIn(horizontal) > longestIn(vertical)) {
            return longestIn(horizontal);
        } else {
            return longestIn(vertical);
        }
    }
}
