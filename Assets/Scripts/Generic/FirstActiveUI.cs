using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstActiveUI : MonoBehaviour
{
    public GameObject mainUis;

    public static FirstActiveUI Instance;
    void Awake() { Instance = this; }

    public int Run() { // Checks, whether there is some toggled UI
        for (int i = 0; i < mainUis.transform.childCount; i++) {
            if (mainUis.transform.GetChild(i).gameObject.activeInHierarchy) {
                return i;
            }
        }

        return -1;
    }
}