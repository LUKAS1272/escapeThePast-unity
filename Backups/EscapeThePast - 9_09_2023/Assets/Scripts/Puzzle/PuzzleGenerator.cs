using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;
using Random=UnityEngine.Random;

public class PuzzleGenerator : MonoBehaviour
{
    public static PuzzleGenerator Instance;
    void Awake() { Instance = this; }

    public TextAsset pregeneratedMaps; // Variable for storing text file with pregenerated maps
    public string randomMap;
    public string mapString = ""; // String used for storing selected map
    public bool isSolved = false; // Bool which decides whether the puzzle is solved
    public int mapLoaded = -1;

    public int size; // Map size
    int labelsSpace; // The size of labels next to puzzle map
    public int chosenApproach = -1; // -1 = generate ; non-negative = pregenerated map index

    public GameObject window, puzzleSlot, blankSlot, labelSlot, continueButton; // Loads needed GameObjects
    public Toggle disableLines; // Hover lines toggle

    float windowSize;

    void Start() {
        randomMap = RandomMapGenerator.Instance.path;
        windowSize = window.GetComponent<RectTransform>().sizeDelta.x;
        continueButton.SetActive(false);
    }


    // ----------------------------------------------
    // Class methods
    // ----------------------------------------------

    public void SetValues(int selectedMap) { // Sets all variables to default when loading a new puzzle
        isSolved = false;
        mapLoaded = selectedMap;

        if (selectedMap == -1) {
            StreamReader reader = new StreamReader(randomMap);
            mapString = reader.ReadToEnd();
            reader.Close();
        } else {
            mapString = MapsDivider.Instance.final.ElementAt(selectedMap).ElementAt(Random.Range(0, MapsDivider.Instance.final.ElementAt(selectedMap).Count));
        }

        SolutionChecker.Instance.hearts = 3;
        SolutionChecker.Instance.heartsText.text = $"{SolutionChecker.Instance.hearts}";

        size = mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries)[0].Length;

        labelsSpace = LabelsGenerator.Instance.findLongest();
        float outlineSize = 6;
        if (size + labelsSpace >= 5)
            outlineSize = 6/((size+labelsSpace)/5);
        Vector2 sizes = new Vector2(outlineSize, outlineSize);

        puzzleSlot.GetComponent<Outline>().effectDistance = sizes; // Changes outline size
        UIAction.Instance.puzzle.SetActive(true); // Shows the puzzle
        puzzleLoad(size + labelsSpace, labelsSpace); // Loads the puzzle
        PuzzleTimer.Instance.ResetTimer(); // Resets and turns on the timer

        Continue();
    }

    void clear() { // Clears the grid (deletes all the slots)
        for (int i = window.transform.childCount - 1; i >= 0; i--) {
            Destroy(window.transform.GetChild(i).gameObject);
        }
    }

    public void CheckSolution() {
        int indexInGrid;
        mapString = string.Join("", mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));
        isSolved = true;

        for (int i = 0; i < size*size; i++) {
            indexInGrid = (labelsSpace + (int)(i / size)) * (size + labelsSpace) + labelsSpace + i % size;
            if (GetIndexInGrid(i).isOn && mapString[i] != '+' || !GetIndexInGrid(i).isOn && mapString[i] != '-') {
                isSolved = false;
                break;
            }
        }
            
        if (isSolved) { // Showing the solution
            PuzzleTimer.Instance.StopTimer();
            if (PlayerMovement.Instance.currentColider != null) { PlayerMovement.Instance.currentColider.isSolved = true; }
            PuzzleInteraction.Instance.GiveReward(mapLoaded);

            for (int i = 0; i < size*size; i++) {
                indexInGrid = (labelsSpace + (int)(i / size)) * (size + labelsSpace) + labelsSpace + i % size;

                if (!GetIndexInGrid(i).wrongGuess && !GetIndexInGrid(i).isOn) {
                    window.transform.GetChild(indexInGrid).gameObject.GetComponent<Button>().enabled = false;
                    window.transform.GetChild(indexInGrid).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    window.transform.GetChild(indexInGrid).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    window.transform.GetChild(indexInGrid).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                } else if (GetIndexInGrid(i).isOn) {
                    window.transform.GetChild(indexInGrid).gameObject.GetComponent<Image>().color = new Color32(0, 255, 87, 255);
                }
            }
        }

        Continue();
    }

    public SlotSwitcher GetIndexInGrid(int index) {
        int indexInGrid = (labelsSpace + (int)(index / size)) * (size + labelsSpace) + labelsSpace + index % size;
        return window.transform.GetChild(indexInGrid).gameObject.GetComponent<SlotSwitcher>();
    }

    void puzzleLoad(int gridSize, int labelsSpace) { // Reloads the puzzle
        window.GetComponent<GridLayoutGroup>().cellSize = new Vector2((float)(windowSize/gridSize), (float)(windowSize/gridSize)); // Changes the slot size
        clear(); // Clears the grid

        int[] topSlotDeficit = new int[gridSize - labelsSpace]; // Slot numbers deficit on the x axis
        int[] leftSlotDeficit = new int[gridSize - labelsSpace]; // Slot numbers deficit on the y axis

        for (int i = 0; i < gridSize - labelsSpace; i++) {
            topSlotDeficit[i] = labelsSpace - LabelsGenerator.Instance.horizontal[i].Count;
            leftSlotDeficit[i] = labelsSpace - LabelsGenerator.Instance.vertical[i].Count;
        }

        int clickSlotIndex = 0;
        for (int index = 0; index < gridSize*gridSize; index++) {
            int i = index / gridSize;
            int j = index - i * gridSize;

            if (i < labelsSpace && j < labelsSpace) {
                GameObject newSlot = Instantiate(blankSlot, transform.position, transform.rotation); // Creates a blankSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent
            } else if (i < labelsSpace && j >= labelsSpace) { // Top labels
                GameObject newSlot = Instantiate(labelSlot, transform.position, transform.rotation); // Creates a labelSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent

                if (topSlotDeficit[j - labelsSpace] <= i) { // Sets numbers to the most down point
                    newSlot.transform.GetChild(1).gameObject.GetComponent<Text>().text = $"{LabelsGenerator.Instance.horizontal[j - labelsSpace][i - topSlotDeficit[j - labelsSpace]]}"; // Accesses the text componenet
                }
            } else if (i >= labelsSpace && j < labelsSpace) { // Left labels
                GameObject newSlot = Instantiate(labelSlot, transform.position, transform.rotation); // Creates a labelSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent
                
                if (leftSlotDeficit[i - labelsSpace] <= j) { // Sets numbers to the most right point
                    newSlot.transform.GetChild(1).gameObject.GetComponent<Text>().text = $"{LabelsGenerator.Instance.vertical[i - labelsSpace][j - leftSlotDeficit[i - labelsSpace]]}"; // Accesses the text componenet
                }
            } else { // Puzzle slots
                GameObject newSlot = Instantiate(puzzleSlot, transform.position, transform.rotation); // Creates a puzzleSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent

                SlotSwitcher.Instance.index = clickSlotIndex; // Sets slot index
                clickSlotIndex++; // Increments the clickSlot index
            }
        }
    }

    public void Continue() {
        if (!isSolved && SolutionChecker.Instance.hearts != 0) {
            continueButton.SetActive(true);
        } else {
            continueButton.SetActive(false);
            mapLoaded = -1;
        }
    }
}