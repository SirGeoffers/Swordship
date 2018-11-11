using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreDisplay : MonoBehaviour {

    public Image backgroundImage;
    public Text playerText;
    public Text scoreText;

    public void SetPlayerText(string _string) {
        playerText.text = _string;
    }

    public void SetScoreText(int _value) {
        scoreText.text = "" + _value;
    }

    public void SetColor(Color _color) {
        backgroundImage.color = _color;
    }

}
