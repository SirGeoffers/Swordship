using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Menu {

    public class MenuManager : MonoBehaviour {

        public MenuScreen firstScreen;
        public EventSystem eventSystem;

        public string gameSceneName;

        public AudioClip selectSound;

        private GameObject lastSelectedGameObject = null;

        private MenuScreen currentScreen;
        private Queue<MenuScreen> previousScreens;

        private static MenuManager instance;
        public static MenuManager Instance {
            get {
                if (instance == null) instance = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
                return instance;
            }
        }

        // Use this for initialization
        void Start() {
            previousScreens = new Queue<MenuScreen>();
            SetCurrentScreen(firstScreen);
        }

        // Update is called once per frame
        void Update() {

            if (eventSystem.currentSelectedGameObject == null) {
                eventSystem.SetSelectedGameObject(lastSelectedGameObject);
            } else {
                lastSelectedGameObject = eventSystem.currentSelectedGameObject;
            }

        }

        private void SetCurrentScreen(MenuScreen _screen) {
            
            if (currentScreen != null) currentScreen.gameObject.SetActive(false);
            _screen.gameObject.SetActive(true);

            eventSystem.SetSelectedGameObject(null);
            if (_screen.firstSelected != null) {
                eventSystem.SetSelectedGameObject(_screen.firstSelected);
            }

            currentScreen = _screen;

        }

        public void LoadScreen(MenuScreen _screen) {
            previousScreens.Enqueue(currentScreen);
            SetCurrentScreen(_screen);
        }

        public void LoadPreviousScreen() {
            if (previousScreens.Count > 0) {
                MenuScreen screen = previousScreens.Dequeue();
                SetCurrentScreen(screen);
            }
        }

        public void LoadGame() {
            SceneManager.LoadScene(gameSceneName);
        }

        //[ContextMenu("Select It")]
        //private void SelectButton() {
        //    eventSystem.SetSelectedGameObject(selectMe);
        //}

    }


}