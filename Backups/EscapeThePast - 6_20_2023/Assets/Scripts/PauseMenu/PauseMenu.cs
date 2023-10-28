using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool doUpdate = true;
    public GameObject pauseMenuUi;
    
    void FixedUpdate() {
        if (doUpdate) {
            if (Input.GetKey(KeyCode.Escape)) {
                pauseMenuUi.SetActive(!pauseMenuUi.activeInHierarchy);
                StartCoroutine(WaitForRelease(KeyCode.Escape));
            }
        }
    }

    IEnumerator WaitForRelease(KeyCode key) {
        doUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        doUpdate = true;
    }
}
