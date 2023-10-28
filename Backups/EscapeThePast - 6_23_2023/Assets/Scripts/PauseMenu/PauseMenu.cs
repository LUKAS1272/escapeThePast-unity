using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool doUpdate = true;
    public GameObject pauseMenuUi;
    public GameObject mainUis;
    
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
            } else {
                mainUis.transform.GetChild(uiActive).gameObject.SetActive(false);
                PuzzleTimer.Instance.StopTimer();

                if (uiActive > 4) // 4 is the last index of default uis
                    Destroy(mainUis.transform.GetChild(uiActive).gameObject);
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
