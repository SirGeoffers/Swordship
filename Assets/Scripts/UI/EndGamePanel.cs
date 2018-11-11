using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour {

    public Text winnerText;

    public void SetWinner(int _playerNum, Color _color) {
        winnerText.text = "Player " + _playerNum + "\nwins";
        winnerText.color = _color;
    }

}
