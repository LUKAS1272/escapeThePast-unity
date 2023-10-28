using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMethods : MonoBehaviour
{
    public static GenericMethods Instance;
    void Awake() { Instance = this; }

    public void Delete(GameObject parent) {
        Destroy(parent);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerToMazeStart() {
        choiceDatabase.Instance.player.transform.position = new Vector3(-29, 20.5f, 0);
    }

    // ----------------------------
    // DEV FUNCTION
    // ----------------------------

    // public void generateMap(int width) {
    //     string map = "";
    //     for (int i = 0; i < width; i++) {
    //         for (int j = 0; j < width; j++) {
    //             int num = Random.Range(0, 101);

    //             if (num <= 34) {
    //                 map += "-";
    //             } else {
    //                 map += "+";
    //             }
    //         }
    //         map += "\n";
    //     }

    //     Debug.Log(map);
    // }
}
