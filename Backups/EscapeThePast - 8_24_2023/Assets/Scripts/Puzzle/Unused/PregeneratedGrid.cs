using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregeneratedGrid : MonoBehaviour
{
    public static PregeneratedGrid Instance;
    void Awake() { Instance = this; }

    public int index = -1;

    public void Clicked() {
        PuzzleGenerator.Instance.SetValues(index);
    }
}