using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ongoingErrorMessages : MonoBehaviour
{
    public int num = 0;
    public static ongoingErrorMessages Instance;

    void Awake() { Instance = this; }

    public IEnumerator writeError(string inputValue) {
        ChooseMenu.Instance.errorText.text = $"\"{inputValue}\" option is not available at this moment...";

        num++;
        yield return new WaitForSeconds(5);
        num--;

        if (num == 0) {
            ChooseMenu.Instance.errorText.text = "";
        }
    }
    
    void Update() {
        if (FirstActiveUI.Instance.Run() == -1) {
            num = 0;
        }
    }
}
