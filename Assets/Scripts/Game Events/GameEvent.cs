using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents {

    public class GameEvent {
        public float time;
        public GameEvent() {
            time = Time.time;
        }
    }

    public class GameEventPlayerDeath : GameEvent {
        public int eliminatorId;
        public int receiverId;
        public GameEventPlayerDeath(int _eliminatorId, int _receiverId) : base() {
            eliminatorId = _eliminatorId;
            receiverId = _receiverId;
        }
    }

    public class GameEventPlayerDamaged : GameEvent {
        public int dealerId;
        public int receiverId;
        public GameEventPlayerDamaged(int _dealerId, int _receiverId) : base() {
            dealerId = _dealerId;
            receiverId = _receiverId;
        }
    }

}

