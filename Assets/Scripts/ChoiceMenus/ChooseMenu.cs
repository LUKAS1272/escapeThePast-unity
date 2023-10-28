using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMenu : MonoBehaviour
{
    public static ChooseMenu Instance;
    void Awake() { Instance = this; }

    Dictionary<string, Item> stringItemPairs = new Dictionary<string, Item>();
    public Item[] items;

    public GameObject menu, menuButton, menuText, uiParent; // Prefabs + parent UI
    public Text errorText;

    public TextAsset jsonInput; // JSON like input file to be converted
    public List<string[]>[] jsonConverted; // Variable storing converted JSON

    void Start() {
        foreach (Item item in items) {
            stringItemPairs.Add(item.name, item);
        }

        int lineNum = 0; // Variable tracking which line of file is being processed

        string[] separateLines = jsonInput.text.Split(new char[] {';', '\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries); // Splitting file into separate lines
        jsonConverted = new List<string[]>[separateLines.Length]; // Setting the length of array (based on number of lines)

        foreach (string line in separateLines) {
            List<string[]> pairsList = new List<string[]>(); // Stores all pairs of a certain line
            string[] lineItems = line.Split(", "); // Splits line into separate pairs

            foreach (string pair in lineItems) {
                string[] separatedPair = pair.Split(": "); // Splits pairs into separate strings - "a: b" >> {"a", "b"}
                pairsList.Add(separatedPair); // Adds separate pairs into the list of string arrays
            }
            
            jsonConverted[lineNum] = pairsList; // Adds list (line) to the root variable
            lineNum++; // Increases the line index for the next iteration
        }


        // Debug write out
        foreach (string[] pair in jsonConverted[0]) {
            Debug.Log($"{pair[0]}: {pair[1]}");
        }
    }

    public void GenerateChoice(int index, bool canClose) {
        GameObject menuInstance = Instantiate(menu, transform.position, transform.rotation); // Creates choose menu
        if (!canClose) menuInstance.transform.GetChild(1).gameObject.SetActive(false); // Removes close button

        errorText = menuInstance.transform.GetChild(3).GetComponent<Text>();
        Debug.Log(errorText.text);
        menuInstance.transform.SetParent(uiParent.transform);

        foreach (string[] pair in jsonConverted[index]) {
            GameObject button = Instantiate(menuButton, transform.position, transform.rotation);
            button.transform.SetParent(menuInstance.transform.GetChild(2).transform.GetChild(0).transform);
            if (stringItemPairs.ContainsKey(pair[0])) { button.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = stringItemPairs[pair[0]].icon; }
            else { button.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = choiceDatabase.Instance.questionMark; }
            button.GetComponent<ChooseButton>().menuId = index;

            ChooseButton script = button.GetComponent<ChooseButton>();
            Debug.Log(button.transform.GetSiblingIndex());
            GameObject text = Instantiate(menuText, transform.position, transform.rotation);
            text.transform.SetParent(menuInstance.transform.GetChild(2).transform.GetChild(1).transform);
            text.GetComponent<Text>().text = pair[1];

            if (stringItemPairs.ContainsKey(pair[0]) && !InventoryManager.Instance.Items.Contains(stringItemPairs[pair[0]])) {
                button.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 25);
                text.GetComponent<Text>().color = new Color32(255, 255, 255, 25);
                script.isAvailable = false;
            }
        }






        if (index == 4) { // If guards see the player
            foreach (GameObject guard in choiceDatabase.Instance.guards) {
                PuzzleInteraction.Instance.DisplayStatus("Oh no, the guards caught you!\n\nThe only thing you can do is try fighting back with some strength potion.\n\nOtherwise you have to give up...");
                guard.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255); // Changes guard color to red
                guard.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = choiceDatabase.Instance.exclamationMark; // Changes status to angry
                guard.transform.GetChild(1).gameObject.GetComponent<PolygonCollider2D>().isTrigger = true; // Sets view field to trigger
                guard.transform.GetChild(1).transform.localScale = new Vector3((float)0.5, (float)0.35, (float)1); // Makes the view field bigger
                guard.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 100); // Changes the view field color
            }
        }
    }
}
