using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class PuzzleGenerator : MonoBehaviour
{
    public static PuzzleGenerator Instance;
    public bool isSolved = false;

    public GameObject window, lightSlot, darkSlot, wrongSlot, wrongLabelSlot, blankSlot, labelSlot;
    public List<bool> areOn = new List<bool>();
    public List<bool> mapSolution = new List<bool>();

    void Awake() { Instance = this; }

    public static string mapPath = "Assets/Maps"; // Path to the puzzle map
    public static string mapName = "pregeneratedMaps.txt"; // Puzzle map file

    public static int size = File.ReadAllLines($"{mapPath}/{mapName}")[0].Length;
    bool[] wrongGuesses = new bool[File.ReadAllLines($"{mapPath}/{mapName}")[0].Length*File.ReadAllLines($"{mapPath}/{mapName}")[0].Length]; // Keeps track of player wrong guesses
    public bool[] wrongLabels = new bool[File.ReadAllLines($"{mapPath}/{mapName}")[0].Length*File.ReadAllLines($"{mapPath}/{mapName}")[0].Length]; // Keeps track of player wrong labels
    public Toggle labelWrong; // Wrong labeling toggle
    
    int labelsSpace;
    void Start() {
        labelsSpace = LabelsGenerator.Instance.findLongest();

        for (int i = 0; i < size*size; i++) { areOn.Add(false); } // Adds the list for keeping track of the puzzle field
        advancedReload(size + labelsSpace, labelsSpace); // Loads the puzzle
    }




    // ----------------------------------------------
    // Class methods
    // ----------------------------------------------

    void clear() { // Clears the grid (deletes all the slots)
        for (int i = window.transform.childCount - 1; i >= 0; i--) {
            Destroy(window.transform.GetChild(i).gameObject);
        }
    }

    public void slotSwitch(int index, bool wrongGuess) { // Switches the slot and reloads puzzle
        if (labelWrong.isOn) {
            wrongLabels[index] = !wrongLabels[index];
        } else if (wrongGuess) {
            wrongGuesses[index] = true;
        } else if (!wrongLabels[index]) {
            areOn[index] = !areOn[index];

            // Solution check
            string mapString = string.Join("", File.ReadAllLines($"{mapPath}/{mapName}"));
            // string mapString = string.Join("", pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));
            isSolved = true;
            for (int i = 0; i < size*size; i++) {
                if (areOn[i] && mapString[i] != '+' || !areOn[i] && mapString[i] != '-') {
                    isSolved = false;
                    break;
                }
            }
        }
        advancedReload(size + labelsSpace, labelsSpace);
    }

    void advancedReload(int gridSize, int labelsSpace) { // Reloads the puzzle
        window.GetComponent<GridLayoutGroup>().cellSize = new Vector2(800/gridSize, 800/gridSize); // Changes the slot size
        clear(); // Clears the grid

        int[] topSlotDeficit = new int[gridSize - labelsSpace]; // Slot numbers deficit on the x axis
        int[] leftSlotDeficit = new int[gridSize - labelsSpace]; // Slot numbers deficit on the y axis

        for (int i = 0; i < gridSize - labelsSpace; i++) {
            Debug.Log($"{i} {gridSize} {labelsSpace}");
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
            } else {
                if (wrongLabels[clickSlotIndex]) {
                    GameObject newSlot = Instantiate(wrongLabelSlot, transform.position, transform.rotation); // Creates a wrongLabelSlot
                    newSlot.transform.SetParent(window.transform); // Sets slot parent
                } else if (wrongGuesses[clickSlotIndex]) {
                    GameObject newSlot = Instantiate(wrongSlot, transform.position, transform.rotation); // Creates a wrongSlot
                    newSlot.transform.SetParent(window.transform); // Sets slot parent
                } else if (areOn[clickSlotIndex]) {
                    GameObject newSlot = Instantiate(darkSlot, transform.position, transform.rotation); // Creates a darkSlot
                    newSlot.transform.SetParent(window.transform); // Sets slot parent
                } else {
                    GameObject newSlot = Instantiate(lightSlot, transform.position, transform.rotation); // Creates a lightSlot
                    newSlot.transform.SetParent(window.transform); // Sets slot parent
                }

                SlotSwitcher.Instance.index = clickSlotIndex; // Sets slot index
                clickSlotIndex++; // Increments the clickSlot index
            }
        }
    }










    // ----------------------------------------------
    // Currently unused class methods
    // ----------------------------------------------

    void generateSolution() {
        string[] linesArray = File.ReadAllLines($"{mapPath}/{mapName}");
        // string[] linesArray = pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in linesArray) {
            foreach (char character in line) {
                if (character == '+') {
                    mapSolution.Add(true);
                } else if (character == '-') {
                    mapSolution.Add(false);
                }
            }
        }
    }

    void clearLoad(int gridSize) { // Sets up blank grid
        window.GetComponent<GridLayoutGroup>().cellSize = new Vector2(800/gridSize, 800/gridSize); // Changes the slot size
        clear();
        areOn.Clear();

        for (int i = 0; i < size*size; i++) {
            GameObject newSlot = Instantiate(lightSlot, transform.position, transform.rotation); // Creates a slot
            newSlot.transform.SetParent(window.transform); // Sets slot parent

            SlotSwitcher.Instance.index = i; // Sets slot index
            areOn.Add(false);
        }
    }

    void reload() {
        clear();

        for (int i = 0; i < size*size; i++) {
            if (areOn[i]) {
                GameObject newSlot = Instantiate(darkSlot, transform.position, transform.rotation); // Creates a darkSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent
            } else {
                GameObject newSlot = Instantiate(lightSlot, transform.position, transform.rotation); // Creates a lightSlot
                newSlot.transform.SetParent(window.transform); // Sets slot parent
            }

            SlotSwitcher.Instance.index = i; // Sets slot index
        }
    }
}
