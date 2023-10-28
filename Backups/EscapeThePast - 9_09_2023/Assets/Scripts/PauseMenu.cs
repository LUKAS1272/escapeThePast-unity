using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool doUpdate = true;
    public GameObject pauseMenuUi;
    public GameObject mainUis;
    int uiNum;

    void Start() {
        uiNum = mainUis.transform.childCount;
    }
    
    void FixedUpdate() {
        if (doUpdate && Input.GetKey(KeyCode.Escape)) {
            int uiActive = -1;
            for (int i = mainUis.transform.childCount - 1; i >= 0; i--) {
                if (mainUis.transform.GetChild(i).gameObject.activeInHierarchy) {
                    uiActive = i;
                    break;
                }
            }

            if (uiActive == -1) {
                pauseMenuUi.SetActive(true);
            } else if (uiActive <= uiNum - 1) {
                mainUis.transform.GetChild(uiActive).gameObject.SetActive(false);
                PuzzleTimer.Instance.StopTimer();
            }

            // else {
            //     mainUis.transform.GetChild(uiActive).gameObject.SetActive(false);
            //     PuzzleTimer.Instance.StopTimer();

            //     if (uiActive > uiNum - 1) // If the active UI is not one of the default
            //         Destroy(mainUis.transform.GetChild(uiActive).gameObject);
            // }

            StartCoroutine(WaitForRelease(KeyCode.Escape));
        }
    }

    IEnumerator WaitForRelease(KeyCode key) {
        doUpdate = false;
        yield return new WaitUntil(() => Input.GetKeyUp(key));
        doUpdate = true;
    }
}
