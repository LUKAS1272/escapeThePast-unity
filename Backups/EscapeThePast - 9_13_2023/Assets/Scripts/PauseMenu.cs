using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool doUpdate = true;
    public GameObject mainUis;
    
    int uiNum;
    void Awake() { uiNum = mainUis.transform.childCount; }
    
    void FixedUpdate() {
        if (doUpdate && Input.GetKey(KeyCode.Escape)) {
            if (FirstActiveUI.Instance.Run() == -1) {
                mainUis.transform.GetChild(4).gameObject.SetActive(true); // Turns on the pause menu
            } else if (FirstActiveUI.Instance.Run() <= uiNum - 1) {
                mainUis.transform.GetChild(FirstActiveUI.Instance.Run()).gameObject.SetActive(false); // Turns off the first (in the hierarchy) shown UI
                PuzzleTimer.Instance.StopTimer();
            }

            StartCoroutine(WaitForRelease(KeyCode.Escape));
        }
    }

    IEnumerator WaitForRelease(KeyCode key) {
        doUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        doUpdate = true;
    }
}
