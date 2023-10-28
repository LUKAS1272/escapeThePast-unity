using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

public class PuzzleGenerator : MonoBehaviour
{
    public TextAsset pregeneratedMaps; // Variable for storing text file with pregenerated maps
    public string randomMap;
    public int size; // Map size
    public int chosenApproach = -1;

    public string mapString = "";

    public static PuzzleGenerator Instance;
    public bool isSolved = false;

    public GameObject window, lightSlot, darkSlot, wrongSlot, wrongLabelSlot, blankSlot, labelSlot;
    public bool[] areOn;
    // public List<bool> mapSolution = new List<bool>(); // Currentely not used

    bool[] wrongGuesses; // Keeps track of player wrong guesses
    public bool[] wrongLabels; // Keeps track of player wrong labels
    public Toggle labelWrong; // Wrong labeling toggle

    void Awake() {
        Instance = this;
        mapString = pregeneratedMaps.text;
        size = mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries)[0].Length;

        areOn = new bool[size*size];
        wrongGuesses = new bool[size*size];
        wrongLabels = new bool[size*size];
    }
    
    int labelsSpace;
    void Start() {
        randomMap = RandomMapGenerator.Instance.path;
        labelsSpace = LabelsGenerator.Instance.findLongest();
        advancedReload(size + labelsSpace, labelsSpace); // Loads the puzzle
    }

    public void SetValues(int selectedApproach) {
        chosenApproach = selectedApproach;

        if (chosenApproach == 0) {
            StreamReader reader = new StreamReader(randomMap);
            mapString = reader.ReadToEnd();
            reader.Close();
        } else {
            mapString = pregeneratedMaps.text;
        }

        size = mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries)[0].Length;


        // Clears all tracking arrays
        Array.Clear(areOn, 0, areOn.Length);
        Array.Clear(wrongGuesses, 0, wrongGuesses.Length);
        Array.Clear(wrongLabels, 0, wrongLabels.Length);

        Array.Resize(ref areOn, size*size);
        Array.Resize(ref wrongGuesses, size*size);
        Array.Resize(ref wrongLabels, size*size);

        labelsSpace = LabelsGenerator.Instance.findLongest();
        float outlineSize = 6/((size+labelsSpace)/5);
        Vector2 sizes = new Vector2(outlineSize, outlineSize);

        lightSlot.GetComponent<Outline>().effectDistance = sizes;
        darkSlot.GetComponent<Outline>().effectDistance = sizes;
        wrongSlot.GetComponent<Outline>().effectDistance = sizes;
        wrongLabelSlot.GetComponent<Outline>().effectDistance = sizes;

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
            // string mapString = string.Join("", File.ReadAllLines($"{mapPath}/{mapName}"));
            mapString = string.Join("", mapString.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries));
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

    // void generateSolution() {
    //     string[] linesArray = File.ReadAllLines($"{mapPath}/{mapName}");
    //     // string[] linesArray = pregeneratedMaps.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);

    //     foreach (string line in linesArray) {
    //         foreach (char character in line) {
    //             if (character == '+') {
    //                 mapSolution.Add(true);
    //             } else if (character == '-') {
    //                 mapSolution.Add(false);
    //             }
    //         }
    //     }
    // }

    // void clearLoad(int gridSize) { // Sets up blank grid
    //     window.GetComponent<GridLayoutGroup>().cellSize = new Vector2(800/gridSize, 800/gridSize); // Changes the slot size
    //     clear();
    //     areOn.Clear();

    //     for (int i = 0; i < size*size; i++) {
    //         GameObject newSlot = Instantiate(lightSlot, transform.position, transform.rotation); // Creates a slot
    //         newSlot.transform.SetParent(window.transform); // Sets slot parent

    //         SlotSwitcher.Instance.index = i; // Sets slot index
    //         areOn.Add(false);
    //     }
    // }

    // void reload() {
    //     clear();

    //     for (int i = 0; i < size*size; i++) {
    //         if (areOn[i]) {
    //             GameObject newSlot = Instantiate(darkSlot, transform.position, transform.rotation); // Creates a darkSlot
    //             newSlot.transform.SetParent(window.transform); // Sets slot parent
    //         } else {
    //             GameObject newSlot = Instantiate(lightSlot, transform.position, transform.rotation); // Creates a lightSlot
    //             newSlot.transform.SetParent(window.transform); // Sets slot parent
    //         }

    //         SlotSwitcher.Instance.index = i; // Sets slot index
    //     }
    // }
}
