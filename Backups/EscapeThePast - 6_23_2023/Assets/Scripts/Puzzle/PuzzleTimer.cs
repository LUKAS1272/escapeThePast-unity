using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTimer : MonoBehaviour
{
    public static PuzzleTimer Instance;
    void Awake() { Instance = this; }

    public float timer, roundedTimer;
    public int hundredths, seconds, minutes;
    public bool runTimer;
    public Text timerText;
    public string timeString = "";

    public void ResetTimer() {
        hundredths = 0;
        seconds = 0;
        minutes = 0;
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

            hundredths = (int)(roundedTimer % 1 * 100);
            seconds = (int)(roundedTimer % 60);
            minutes = (int)(Mathf.Floor(roundedTimer / 60));


            // Time string constructor
            timeString = "";
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

            timerText.text = timeString;
            
            if (timeString.Length != 8) {
                Debug.Log(timeString);
            }
        }
    }
}
