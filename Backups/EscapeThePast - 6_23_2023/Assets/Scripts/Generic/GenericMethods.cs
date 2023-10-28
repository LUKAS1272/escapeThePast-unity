using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMethods : MonoBehaviour
{
    public void Delete(GameObject parent) {
        Destroy(parent);
    }

    public void QuitGame() {
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false;
    }
}
