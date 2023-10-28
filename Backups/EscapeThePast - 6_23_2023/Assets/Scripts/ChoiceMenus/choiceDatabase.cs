using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiceDatabase : MonoBehaviour
{
    public static choiceDatabase Instance;
    void Awake() { Instance = this; }

    public GameObject player;
    public Sprite outlineCircle;

    // Pots choice
    public GameObject potsBarrier;
    public Item redPot, greenPot, bluePot; // Potions

    public Item armor, spear; // Guards items
    public GameObject[] guards;

    public Sprite questionMark, sleep, skull; // NPC states

    public GameObject chest;
}
