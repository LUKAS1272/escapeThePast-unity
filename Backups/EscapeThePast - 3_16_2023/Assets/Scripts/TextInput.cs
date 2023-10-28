using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TextInput : MonoBehaviour
{
    public TextAsset randomGenerated;
    public InputField randomGridSize;
    public Text errorMessage;
    public Button generateButton;

    // Random rnd = new Random();

    public void GenerateRandom() {
        int size;

        string[] text = randomGenerated.text.Split(new char[] {'\n', '\r'}, System.StringSplitOptions.RemoveEmptyEntries);

        Debug.Log(text.Length);
        foreach (string line in text) {
            Debug.Log(line + " " + line.Length);
        }
        // Debug.Log(text[9][0]);
        // Debug.Log(text[9][10]);
        // Debug.Log(Convert.ToInt32(text[9][0]));
        // Debug.Log(Convert.ToInt32(text[9][10]));

        if (int.TryParse(randomGridSize.text, out size) && size >= 5 && size <= 20) {
            // Debug.Log(size);
            // StreamWriter writer = new StreamWriter(randomGenerated);

            // for (int y = 0; y < 5; y++) {
            //     string line = "";
            //     for (int x = 0; x < 5; x++) {
            //         int num = Random.Range(0, 2);

            //         switch (num) {
            //             case 0:
            //                 line += '-';
            //                 break;
            //             case 1:
            //                 line += '+';
            //                 break;
            //         }
            //     }
            //     Debug.Log(line);
            //     writer.WriteLine(line);
            // }

            // Debug.Log(UnityEditor.AssetDatabase.GetAssetPath(randomGenerated));
            // errorMessage.text = UnityEditor.AssetDatabase.GetAssetPath(randomGenerated);
        } else {
            Debug.Log("Invalid number");
        }
    }
}
