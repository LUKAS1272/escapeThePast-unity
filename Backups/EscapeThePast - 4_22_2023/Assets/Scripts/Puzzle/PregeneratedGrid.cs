using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregeneratedGrid : MonoBehaviour
{
    public int index = -1;

    public static PregeneratedGrid Instance;
    void Awake() { Instance = this; }

    public void Clicked() {
        PuzzleGenerator.Instance.SetValues(index);
    }
}