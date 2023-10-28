using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Random=UnityEngine.Random;

public class RandomMapGenerator : MonoBehaviour
{
    public static RandomMapGenerator Instance;

    public GameObject typeChoose;
    public GameObject generateRandom;
    public InputField randomGridSize;

    public TextAsset randomGenerated;
    public Text errorMessage;
    public Button generateButton;

    public string path;

    void Awake() {
        Instance = this;

        path = Path.Combine(Application.streamingAssetsPath, "randomMap.txt");

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write("");
        writer.Close();
    }

    public void GenerateRandom() {
        int size;

        if (int.TryParse(randomGridSize.text, out size) && size >= 5 && size <= 20) {
            Debug.Log(size);

            string map = "";
            for (int y = 0; y < size; y++) {
                string line = "";
                for (int x = 0; x < size; x++) {
                    int num = Random.Range(0, 2);

                    if (num == 0) {
                        line += '+';
                    } else {
                        line += '-';
                    }
                }
                
                map += line;
                if (y != size - 1) { map += '\n'; }
            }
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(map);
            writer.Close();


            PuzzleGenerator.Instance.SetValues(0);
            UIAction.Instance.puzzle.SetActive(true);


        } else {
            Debug.Log("Invalid number");
        }
    }

    // 0 = generate ; 1 = pregenerated
    public void Choose(int chooseIndex) {
        typeChoose.SetActive(false);

        if (chooseIndex == 0) { generateRandom.SetActive(true); }
    }

    public void Previous() {
        typeChoose.SetActive(true);
        generateRandom.SetActive(false);
        randomGridSize.text = "";
    }
}
