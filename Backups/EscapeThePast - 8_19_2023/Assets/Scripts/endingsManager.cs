using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endingsManager : MonoBehaviour
{
    public GameObject uis;
    public Sprite checkmark, lockIcon, whiteFlag;

    void Awake() {
        LoadEndings();
    }

    public void ShowEndings() {
        uis.transform.GetChild(6).gameObject.SetActive(true);
    }

    public void OpenEnding(int endingNum) {
        uis.transform.GetChild(6).GetChild(1).GetChild(endingNum).gameObject.GetComponent<Image>().sprite = checkmark;
        PlayerPrefs.SetInt(endingNum + "", 1);
    }

    public void LoadEndings() {
        for (int index = 0; index < uis.transform.GetChild(6).GetChild(1).childCount; index++) {
            if (PlayerPrefs.HasKey(index + "") && PlayerPrefs.GetInt(index + "") == 1) {
                Sprite icon = lockIcon;
                string endingName = "";

                switch (index) {
                    case 0:
                        icon = whiteFlag;
                        endingName = "Unsuccessful escapist";
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }

                Transform ending = uis.transform.GetChild(6).GetChild(1).GetChild(index);
                ending.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = icon;
                ending.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = endingName;
            }
        }
    }

    public void ResetEndings() {
        for (int index = 0; index < uis.transform.GetChild(6).GetChild(1).childCount; index++) {
            if (PlayerPrefs.HasKey(index + "")) {
                PlayerPrefs.DeleteKey(index + "");
                
                Transform ending = uis.transform.GetChild(6).GetChild(1).GetChild(index);
                ending.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = lockIcon;
                ending.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "? ? ?";
            }
        }
    }
}
