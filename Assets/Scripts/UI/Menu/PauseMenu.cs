using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private static PauseMenu instance;
    public static PauseMenu Instance {
        get {
            if (instance == null) instance = GameObject.Find("Pause Menu").GetComponent<PauseMenu>();
            return instance;
        }
    }

    private bool isPaused = false;

	public void Resume() {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void Pause() {
        Time.timeScale = 0;
        this.transform.SetAsLastSibling();
        this.gameObject.SetActive(true);
    }

    public void Toggle() {
        if (isPaused) Resume();
        else Pause();
    }

    public void QuitToMenu() {
        GameManager.Instance.LoadMainMenu();
    }

}
