using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    public GameObject window;
    public GameObject mapSlot;

    void Start() {
        int numOfMaps = PuzzleGenerator.Instance.pregeneratedMaps.text.Split(';').Length;

        for (int i = window.transform.childCount - 1; i >= 0; i--) {
            Destroy(window.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < numOfMaps; i++) {
            GameObject newSlot = Instantiate(mapSlot, transform.position, transform.rotation);
            newSlot.transform.SetParent(window.transform);
            newSlot.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{i + 1}";

            PregeneratedGrid.Instance.index = i;
        }
    }
}
