using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour {

    public Text timerText;

	public void SetTime(float _time) {

        if (_time <= 0) _time = 0;

        int minutes = Mathf.FloorToInt(_time / 60f);
        int seconds = Mathf.FloorToInt(_time) - (minutes * 60);

        string secondString = "";
        if (seconds < 10) secondString = "0";
        secondString += seconds;

        timerText.text = minutes + ":" + secondString;

    }

}
