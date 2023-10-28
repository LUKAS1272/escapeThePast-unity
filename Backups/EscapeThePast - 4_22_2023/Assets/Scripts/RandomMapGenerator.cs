using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using Random=UnityEngine.Random;

public class RandomMapGenerator : MonoBehaviour
{
    public static RandomMapGenerator Instance;

    public GameObject typeChoose;
    public GameObject generateRandom;
    public GameObject usePregenerated;
    public InputField randomGridSize;

    public TextAsset randomGenerated;
    public Text errorMessage1;
    public Text errorMessage2;
    public Button generateButton;

    public string path;

    int ongoingErrorMessages = 0;

    void Awake() {
        Instance = this;

        path = Path.Combine(Application.streamingAssetsPath, "randomMap.txt");

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write("");
        writer.Close();
    }

    public void GenerateRandom() {
        int size;
        // EventSystem.current.SetSelectedGameObject(null); // Deselects the slot

        if (int.TryParse(randomGridSize.text, out size) && size >= 5 && size <= 20) {
            string map = "";
            for (int y = 0; y < size; y++) {
                string line = "";
                for (int x = 0; x < size; x++) {
                    int num = Random.Range(0, 2);

                    if (num == 0) { line += '+'; }
                    else { line += '-'; }
                }
                
                map += line;
                if (y != size - 1) { map += '\n'; }
            }
            
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(map);
            writer.Close();

            PuzzleGenerator.Instance.SetValues(-1);
        } else {
            StartCoroutine(writeError(randomGridSize.text + ""));
        }
    }

    public void Choose(int chooseIndex) {
        typeChoose.SetActive(false);
        if (chooseIndex == 0) {
            generateRandom.SetActive(true);
        } else {
            usePregenerated.SetActive(true);
        }
    }

    public void Previous() {
        typeChoose.SetActive(true);
        generateRandom.SetActive(false);
        usePregenerated.SetActive(false);
        randomGridSize.text = "";

        // EventSystem.current.SetSelectedGameObject(null); // Deselects the slot
    }

    IEnumerator writeError(string inputValue) {
        errorMessage1.text = "Please enter valid integer number from 5 to 20.";
        errorMessage2.text = $"The input \"{inputValue}\" is not valid.";
        ongoingErrorMessages++;
        yield return new WaitForSeconds(5);
        ongoingErrorMessages--;

        if (ongoingErrorMessages == 0) {
            errorMessage1.text = "";
            errorMessage2.text = "";
        }
    }
}
