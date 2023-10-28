using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMethods : MonoBehaviour
{
    public void Delete(GameObject parent) {
        Destroy(parent);
    }

    public void QuitGame() {
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void SaveGame() {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        print("Game saved!");
    }
    
    public void LoadGame() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        print("Game loaded!");
    }

    public void generateMap(int width) {
        string map = "";
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < width; j++) {
                int num = Random.Range(0, 101);

                if (num <= 34) {
                    map += "-";
                } else {
                    map += "+";
                }
            }
            map += "\n";
        }

        Debug.Log(map);
    }
}
