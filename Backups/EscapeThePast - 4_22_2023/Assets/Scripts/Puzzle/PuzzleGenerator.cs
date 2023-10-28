using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

public class PuzzleGenerator : MonoBehaviour
{
    public static PuzzleGenerator Instance;

    public TextAsset pregeneratedMaps; // Variable for storing text file with pregenerated maps
    public string randomMap;
    public string mapString = ""; // String used for storing selected map
    public bool isSolved = false; // Bool which decides whether the puzzle is solved

    public int size; // Map size
    public int chosenApproach = -1; // 0 = generate ; 1 = pregenerated

    public GameObject window, puzzleSlot, blankSlot, labelSlot; // Loads needed GameObjects
    public Toggle labelWrong; // Wrong labeling toggle
    public Toggle labelRight; // Right labeling toggle

    void Awake() {
        Instance = this;
        mapString = pregeneratedMaps.text;
        size = mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries)[0].Length;
    }
    
    int labelsSpace;
    void Start() {
        randomMap = RandomMapGenerator.Instance.path;
        labelsSpace = LabelsGenerator.Instance.findLongest();
        puzzleLoad(size + labelsSpace, labelsSpace); // Loads the puzzle

        GameObject newSlot = Instantiate(puzzleSlot, transform.position, transform.rotation); // Creates a puzzleSlot
        newSlot.transform.SetParent(window.transform); // Sets slot parent
    }


    // ----------------------------------------------
    // Class methods
    // ----------------------------------------------

    public void SetValues(int selectedMap) { // Sets all variables to default when loading a new puzzle
        isSolved = false;

        if (selectedMap == -1) {
            StreamReader reader = new StreamReader(randomMap);
            mapString = reader.ReadToEnd();
            reader.Close();
        } else {
            mapString = pregeneratedMaps.text.Split(';')[selectedMap];
        }

        SolutionChecker.Instance.hearts = 3;
        SolutionChecker.Instance.heartsText.text = $"Hearts: {SolutionChecker.Instance.hearts}";

        size = mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries)[0].Length;

        labelsSpace = LabelsGenerator.Instance.findLongest();
        float outlineSize = 6/((size+labelsSpace)/5);
        Vector2 sizes = new Vector2(outlineSize, outlineSize);

        puzzleSlot.GetComponent<Outline>().effectDistance = sizes; // Changes outline size
        UIAction.Instance.puzzle.SetActive(true); // Shows the puzzle
        puzzleLoad(size + labelsSpace, labelsSpace); // Loads the puzzle
    }

    void clear() { // Clears the grid (deletes all the slots)
        for (int i = window.transform.childCount - 1; i >= 0; i--) {
            Destroy(window.transform.GetChild(i).gameObject);
        }
    }

    public void slotSwitch(int index, bool wrongGuess) { // Switches the slot and reloads puzzle
        int indexInGrid = (labelsSpace + (int)(index / size)) * (size + labelsSpace) + labelsSpace + index % size;
        EventSystem.current.SetSelectedGameObject(null); // Deselects the slot

        GameObject clickedSlot = window.transform.GetChild(indexInGrid).gameObject;
        GameObject clickedSlotCross = clickedSlot.transform.GetChild(0).gameObject;

        if (labelWrong.isOn || labelRight.isOn) {
            if (labelWrong.isOn && !GetIndexInGrid(index).rightLabel) {
                GetIndexInGrid(index).wrongLabel = !GetIndexInGrid(index).wrongLabel;

                clickedSlotCross.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                clickedSlotCross.SetActive(!clickedSlotCross.activeInHierarchy);

                if (clickedSlotCross.activeInHierarchy) {
                    clickedSlot.GetComponent<Image>().color = new Color32(255, 204, 203, 255);
                } else {
                    clickedSlot.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
            } else if (labelRight.isOn && !GetIndexInGrid(index).wrongLabel) {
                GetIndexInGrid(index).rightLabel = !GetIndexInGrid(index).rightLabel;

                clickedSlotCross.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                clickedSlotCross.SetActive(!clickedSlotCross.activeInHierarchy);

                if (clickedSlotCross.activeInHierarchy) {
                    clickedSlot.GetComponent<Image>().color = new Color32(175, 255, 175, 255);
                } else {
                    clickedSlot.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
            }
            
        } else if (wrongGuess) {
            clickedSlot.GetComponent<Button>().enabled = false;
            clickedSlotCross.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            clickedSlotCross.SetActive(true);
        } else if (!GetIndexInGrid(index).wrongLabel && !GetIndexInGrid(index).rightLabel) {
            clickedSlot.GetComponent<Button>().enabled = false; // Disables the slot button
            clickedSlot.GetComponent<Image>().color = new Color32(24, 24, 24, 255); // Sets slot color to black

            // Solution check
            mapString = string.Join("", mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));
            GetIndexInGrid(index).isOn = true;
            isSolved = true;

            for (int i = 0; i < size*size; i++) {
                indexInGrid = (labelsSpace + (int)(i / size)) * (size + labelsSpace) + labelsSpace + i % size;
                if (GetIndexInGrid(i).isOn && mapString[i] != '+' || !GetIndexInGrid(i).isOn && mapString[i] != '-') {
                    
                    isSolved = false;
                    break;
                }
            }
        }
    }

    public void Toggles(int toggleIndex) {
        if (toggleIndex == 0 && labelRight.isOn) {
            labelWrong.isOn = false;
        } else if (toggleIndex == 1 && labelWrong.isOn) {
            labelRight.isOn = false;
        }
    }

    public SlotSwitcher GetIndexInGrid(int index) {
        int indexInGrid = (labelsSpace + (int)(index / size)) * (size + labelsSpace) + labelsSpace + index % size;
        return window.transform.GetChild(indexInGrid).gameObject.GetComponent<SlotSwitcher>();
    }

    void puzzleLoad(int gridSize, int labelsSpace) { // Reloads the puzzle
        window.GetComponent<GridLayoutGroup>().cellSize = new Vector2(800/gridSize, 800/gridSize); // Changes the slot size
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
                    newSlot.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{LabelsGenerator.Instance.horizontal[j - labelsSpace][i - topSlotDeficit[j - labelsSpace]]}"; // Accesses the text componenet
                }
            } else if (i >= labelsSpace && j < labelsSpace) { // Left labels
                GameObject newSlot = Instantiate(labelSlot, transform.position, transform.rotation); // Creates a labelSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent
                
                if (leftSlotDeficit[i - labelsSpace] <= j) { // Sets numbers to the most right point
                    newSlot.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{LabelsGenerator.Instance.vertical[i - labelsSpace][j - leftSlotDeficit[i - labelsSpace]]}"; // Accesses the text componenet
                }
            } else { // Puzzle slots
                GameObject newSlot = Instantiate(puzzleSlot, transform.position, transform.rotation); // Creates a puzzleSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent

                SlotSwitcher.Instance.index = clickSlotIndex; // Sets slot index
                clickSlotIndex++; // Increments the clickSlot index
            }
        }
    }
}
