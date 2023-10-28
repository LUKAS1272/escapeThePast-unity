using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LabelsGenerator : MonoBehaviour
{
    public static LabelsGenerator Instance;

    string[] linesArray;
    int size;

    public List<List<int>> vertical = new List<List<int>>();
    public List<List<int>> horizontal = new List<List<int>>();

    void Awake() {
        Instance = this;

        linesArray = PuzzleGenerator.Instance.pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);
        size = PuzzleGenerator.Instance.size;
    }


    bool canExist() {
        foreach (string line in linesArray) {
            if (line.Length != size) {
                return false;
            }
        }

        return true;
    }

    void verticalScan() {
        for (int i = 0; i < size; i++) {
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
        verticalScan();
        horizontalScan();

        if (longestIn(horizontal) > longestIn(vertical)) {
            return longestIn(horizontal);
        } else {
            return longestIn(vertical);
        }
    }






    // Unused methods

    // void writeScan(List<List<int>> lists) {
    //     string itemsString = "";
    //     foreach (List<int> list in lists) {
    //         foreach (int item in list) {
    //             itemsString += item + " ";
    //         }
    //         itemsString += "\n";
    //     }
    //     // Debug.Log(itemsString);
    // }
}
