using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapsDivider : MonoBehaviour
{
    public static MapsDivider Instance;
    void Awake() { Instance = this; }

    public TextAsset pregeneratedMap;
    public string[] difficulties;
    public List<string[]> maps = new List<string[]>();
    public List<List<string>> final = new List<List<string>>();

    void Start() {
        difficulties = pregeneratedMap.text.Split(";;");

        for (int i = 0; i < difficulties.Length; i++) {
            maps.Add(difficulties[i].Split(";"));
        }

        for (int i = 0; i < difficulties.Length; i++) {
            final.Add(new List<string>());
            for (int j = 0; j < maps.ElementAt(i).Length; j++) {
                final.ElementAt(i).Add(maps.ElementAt(i)[j]);
            }
        }
    }
}
