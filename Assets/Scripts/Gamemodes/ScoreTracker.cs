using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker {

    private Dictionary<int, int> playerScores;

    public ScoreTracker() {
        playerScores = new Dictionary<int, int>();
    }

    public void SetScore(int _playerId, int _score) {
        if (playerScores.ContainsKey(_playerId)) {
            playerScores[_playerId] = _score;
        } else {
            playerScores.Add(_playerId, _score);
        }
    }

    public void IncrementScore(int _playerId, int _scoreIncrement) {
        SetScore(_playerId, GetScore(_playerId) + _scoreIncrement);
    }

    public int GetScore(int _playerId) {
        if (playerScores.ContainsKey(_playerId)) {
            return playerScores[_playerId];
        }
        return 0;
    }

}
