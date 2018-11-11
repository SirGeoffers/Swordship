using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class AroundTheWorld : Gamemode {

    [Header("ATW Settings")]
    public GameObject mapPrefab;

    public override void NotifyPlayerDeath(GameEventPlayerDeath _e) {

        int deadPlayerCount = 0;
        ShipController lastLivePlayer = null;
        foreach (ShipController s in GameManager.Instance.Players.Values) {
            if (s.IsBroken) {
                deadPlayerCount++;
            } else {
                lastLivePlayer = s;
            }
        }

        if (deadPlayerCount == GameManager.Instance.Players.Count - 1) {
            scoreTracker.IncrementScore(lastLivePlayer.playerId, 1);
            MoveToNextMap();
        }

    }

    private void MoveToNextMap() {

    }

}
