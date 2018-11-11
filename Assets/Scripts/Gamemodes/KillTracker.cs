using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTracker {

    private Dictionary<int, GameEventPlayerDamaged> lastDamagedBy;

    public KillTracker() {
        lastDamagedBy = new Dictionary<int, GameEventPlayerDamaged>();
    }

    public void ProcessPlayerDamaged(GameEventPlayerDamaged _e) {

        int receiverId = _e.receiverId;
        int dealerId = _e.dealerId;

        if (dealerId == -1) return;

        if (lastDamagedBy.ContainsKey(receiverId)) {
            lastDamagedBy[receiverId] = _e;
        } else {
            lastDamagedBy.Add(receiverId, _e);
        }

    }

    public int ProcessPlayerDeath(GameEventPlayerDeath _e) {

        int receiverId = _e.receiverId;
        int eliminatorId = _e.eliminatorId;

        if (eliminatorId == -1 && lastDamagedBy.ContainsKey(receiverId)) {
            eliminatorId = lastDamagedBy[receiverId].dealerId;
        }

        lastDamagedBy.Remove(receiverId);

        return eliminatorId;

    }

}
