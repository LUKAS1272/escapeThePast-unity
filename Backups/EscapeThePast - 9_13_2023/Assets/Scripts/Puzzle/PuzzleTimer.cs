using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTimer : MonoBehaviour
{
    public static PuzzleTimer Instance;
    void Awake() { Instance = this; }

    public float timer, roundedTimer;
    public int hundredths, seconds, minutes, hours;
    public bool runTimer;
    public Text timerText;
    public string timeString = "";

    public void ResetTimer() {
        hundredths = 0;
        seconds = 0;
        minutes = 0;
        hours = 0;
        timer = 0;
        timerText.text = "00:00:00";

        runTimer = true;
    }
    
    public void StopTimer() {
        runTimer = false;
    }

    void FixedUpdate()
    {
        if (runTimer) {
            timer += Time.deltaTime;
            roundedTimer = Mathf.Floor(timer * 100) / 100;

            seconds = (int)(roundedTimer % 60);
            hours = (int)(Mathf.Floor(roundedTimer / 3600));
            timeString = "";

            // Time string constructor
            if (hours == 0) {
                minutes = (int)(Mathf.Floor(roundedTimer / 60));
                hundredths = (int)(roundedTimer % 1 * 100);

                if (minutes < 10) {
                    timeString += "0";
                }
                timeString += minutes + ":";

                if (seconds < 10) {
                    timeString += "0";
                }
                timeString += seconds + ":" + hundredths;

                if (timeString.Length == 7) {
                    timeString += "0";
                }
            } else {
                minutes = (int)(Mathf.Floor((roundedTimer / 60) % 60));

                if (hours < 10) {
                    timeString += "0";
                }
                timeString += hours + ":";

                if (minutes < 10) {
                    timeString += "0";
                }
                timeString += minutes + ":";

                if (seconds < 10) {
                    timeString += "0";
                }
                timeString += seconds;
            }

            timerText.text = timeString;


            if (minutes == 10 && seconds == 0) {
                AchievementManager.Instance.OpenAchievement(3);
            } else if (minutes == 30 && seconds == 0) {
                AchievementManager.Instance.OpenAchievement(4);
            } else if (minutes == 60 && seconds == 0) {
                AchievementManager.Instance.OpenAchievement(5);
            }
        }
    }
}
