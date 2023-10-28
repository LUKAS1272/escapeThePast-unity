using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexSetter : MonoBehaviour
{
    public static IndexSetter Instance;
    void Awake() { Instance = this; }
    public int index;
    public bool isSolved;
}
