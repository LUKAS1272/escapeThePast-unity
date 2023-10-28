using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiceDatabase : MonoBehaviour
{
    public static choiceDatabase Instance;
    void Awake() { Instance = this; }

    public GameObject player;
    public Sprite outlineCircle, circle;

    // Pots choice
    public GameObject potsBarrier;
    public Transform noGoingBackBarriers;
    public Item redPot, greenPot, bluePot; // Potions

    public Item axe, sword;

    public Item armor, spear; // Guards items
    public GameObject[] guards;

    public Sprite questionMark, sleep, skull, exclamationMark; // NPC states

    public GameObject chest;
    public Item keys;

    public GameObject returnPortal, secondPortal; // Portal from halway to secret room
}
