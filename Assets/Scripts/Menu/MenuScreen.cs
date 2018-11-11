using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {

    public class MenuScreen : MonoBehaviour {

        public GameObject firstSelected;
        public bool controlledCancel = false;

        private MenuManager menuManager;

        // Use this for initialization
        void Start() {
            menuManager = MenuManager.Instance;
        }

        // Update is called once per frame
        void Update() {

            if (!controlledCancel && Input.GetButton("Cancel")) {
                menuManager.LoadPreviousScreen();
            }

        }

    }
    
}