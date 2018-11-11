using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {

    public class VersusSetup : MonoBehaviour {

        public MenuScreen versusScreen;

        public int minPlayers = 1;
        public int maxPlayers = 4;
        public string[] inputBases;
        public JoinPanel[] joinPanels;

        public static List<PlayerSetupData> playerSetupData;

        // Use this for initialization
        void Start() {
            VersusSetup.playerSetupData = new List<PlayerSetupData>();
        }

        // Update is called once per frame
        void Update() {

            foreach (string iBase in inputBases) {
                if (IsInUse(iBase)) continue;
                if (Input.GetButtonDown(iBase + "_Action1")) {
                    AddPlayer(iBase);
                }
                if (Input.GetButtonDown(iBase + "_Action2") && !IsInUse(iBase)) {
                    Reset();
                    MenuManager.Instance.LoadPreviousScreen();
                }
            }

            foreach (JoinPanel jp in joinPanels) {
                jp.Process();
            }

            if (IsReadyToPlay()) {
                LoadGame();
            }

        }

        private void Reset() {
            foreach (JoinPanel jp in joinPanels) {
                jp.Reset();
            }
        }

        private void AddPlayer(string _inputBase) {

            JoinPanel openSlot = GetOpenSlot();
            if (openSlot != null) {
                openSlot.Join(_inputBase);
            }

        }

        private JoinPanel GetOpenSlot() {
            
            for (int i = 0; i < joinPanels.Length; i++) {
                JoinPanel jp = joinPanels[i];
                if (!jp.IsJoined) {
                    return jp;
                }
            }

            // No slots open
            return null;

        }

        private bool IsInUse(string _inputBase) {
            foreach(JoinPanel jp in joinPanels) {
                if (jp.IsJoined && jp.InputBase.Equals(_inputBase)) {
                    return true;
                }
            }
            return false;
        }

        private bool IsReadyToPlay() {

            int count = 0;

            foreach(JoinPanel jp in joinPanels) {
                if (jp.IsJoined) {
                    if (jp.IsReady) {
                        count++;
                    } else {
                        return false;
                    }
                }
            }

            return count >= minPlayers;

        }

        private void LoadGame() {

            foreach(JoinPanel jp in joinPanels) {
                if (jp.IsJoined) {
                    PlayerSetupData psd = new PlayerSetupData();
                    psd.inputType = jp.InputBase;
                    playerSetupData.Add(psd);
                }
            }

            MenuManager.Instance.LoadGame();

        }

    }

}