using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {

    public class JoinPanel : MonoBehaviour {

        public GameObject noJoinGameObject;
        public GameObject joinedGameObject;
        public Text inputText;
        public GameObject readyGameObject;

        [SerializeField]
        private ControlData[] controlData;
        private Dictionary<string, ControlData> controlDataDict;
        private ControlData currControlData;

        public GameObject leavePrompt;
        public GameObject unreadyPrompt;
        public GameObject readyPrompt;

        public AudioSource positiveAudioSource;
        public AudioSource negativeAudioSource;

        private string inputBase;
        public string InputBase {
            get {
                return inputBase;
            }
        }

        private bool ready = false;
        public bool IsReady {
            get {
                return ready;
            }
        }

        private bool joined = false;
        public bool IsJoined {
            get {
                return joined;
            }
        }

        // Used to stop the Join Panel from joining and readying on the same frame
        private bool justJoined;

        private void Start() {
            controlDataDict = new Dictionary<string, ControlData>();
            foreach(ControlData cd in controlData) {
                controlDataDict.Add(cd.inputBase, cd);
            }
        }

        public void Process() {

            if (joined) {
                
                if (justJoined) {
                    justJoined = false;
                    return;
                }

                if (Input.GetButtonDown(inputBase + "_Action1")) {
                    Ready();
                } else if (Input.GetButtonDown(inputBase + "_Action2")) {
                    Unready();
                }

            }
            

        }

        public void Reset() {
            Unready();
            Unjoin();
        }

        public void Join(string _inputBase) {

            positiveAudioSource.PlayOneShot(MenuManager.Instance.selectSound);

            inputBase = _inputBase;
            inputText.text = _inputBase;
            joined = true;
            noJoinGameObject.SetActive(false);
            joinedGameObject.SetActive(true);
            
            if (_inputBase.Equals("WASD")) {
                currControlData = controlDataDict["WASD"];
            } else if (_inputBase.Equals("Arrows")) {
                currControlData = controlDataDict["Arrows"];
            } else {
                currControlData = controlDataDict["Joy"];
            }
            currControlData.controlDisplay.SetActive(true);

            justJoined = true;

        }

        private void Unjoin() {

            negativeAudioSource.PlayOneShot(MenuManager.Instance.selectSound);

            inputBase = null;
            joined = false;
            noJoinGameObject.SetActive(true);
            joinedGameObject.SetActive(false);
            
            if (currControlData != null) currControlData.controlDisplay.SetActive(false);

        }

        private void Ready() {

            if (ready) return;
            positiveAudioSource.PlayOneShot(MenuManager.Instance.selectSound);

            ready = true;
            readyGameObject.SetActive(true);

            unreadyPrompt.SetActive(true);
            leavePrompt.SetActive(false);
            readyPrompt.SetActive(false);
            currControlData.readyKey.SetActive(false);

        }

        private void Unready() {

            if (ready) {

                negativeAudioSource.PlayOneShot(MenuManager.Instance.selectSound);

                ready = false;
                readyGameObject.SetActive(false);

                unreadyPrompt.SetActive(false);
                leavePrompt.SetActive(true);
                readyPrompt.SetActive(true);
                currControlData.readyKey.SetActive(true);

            } else {
                Unjoin();
            }

        }

        [Serializable]
        private class ControlData {
            public string inputBase = "";
            public GameObject controlDisplay = null;
            public GameObject readyKey = null;
        }

    }

}