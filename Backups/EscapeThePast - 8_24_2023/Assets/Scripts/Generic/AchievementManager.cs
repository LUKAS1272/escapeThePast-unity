using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject uis, achievementMadeUi;
    public Sprite lockIcon, whiteFlag, medievalIcon, clock;

    public static AchievementManager Instance;
    void Awake() {
        LoadAchievements();
        Instance = this;
    }

    Sprite icon = null;
    string achievementName = "? ? ?";
    void SetValues(int index) {
        switch (index) {
            case 0:
                icon = whiteFlag;
                achievementName = "Unsuccessful escapist";
                break;
            case 1:
                icon = medievalIcon;
                achievementName = "Medieval citizen";
                break;
            case 2:
                break;
            case 3:
                icon = clock;
                achievementName = "Focused";
                break;
            case 4:
                icon = clock;
                achievementName = "Dedicated";
                break;
            case 5:
                icon = clock;
                achievementName = "Unstoppable";
                break;
        }
    }

    public void OpenAchievement(int index) {
        
        if (!PlayerPrefs.HasKey(index + "")) {
            SetValues(index);
            achievementMadeUi.SetActive(true);
            achievementMadeUi.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = icon;
            achievementMadeUi.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = achievementName;
        }

        PlayerPrefs.SetInt(index + "", 1);
        LoadAchievements();
    }

    public void LoadAchievements() {
        for (int index = 0; index < uis.transform.GetChild(6).GetChild(1).childCount; index++) {
            if (PlayerPrefs.HasKey(index + "") && PlayerPrefs.GetInt(index + "") == 1) {
                SetValues(index);
                Transform achievement = uis.transform.GetChild(6).GetChild(1).GetChild(index);
                achievement.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = icon;
                achievement.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = achievementName;
            }
        }
    }

    public void ResetAchievements() {
        for (int index = 0; index < uis.transform.GetChild(6).GetChild(1).childCount; index++) {
            if (PlayerPrefs.HasKey(index + "")) {
                PlayerPrefs.DeleteKey(index + "");
                
                Transform achievement = uis.transform.GetChild(6).GetChild(1).GetChild(index);
                achievement.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = lockIcon;
                achievement.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "? ? ?";
            }
        }

        LoadAchievements();
    }
}
