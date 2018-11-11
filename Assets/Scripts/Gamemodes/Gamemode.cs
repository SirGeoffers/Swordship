using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode : MonoBehaviour {

    [Header("Gamemode Settings")]
    public GameObject playerScoreDisplayPrefab;
    public GameObject timerDisplayPrefab;
    public Transform playerScoreCanvasGroup;
    public float time = 120;
    public GameObject initialMapPrefab;

    [Header("Countdown")]
    public GameObject countdownNumberPrefab;

    [Header("End Game")]
    public GameObject endGamePanelPrefab;

    private GameManager gameManager;
    private bool isPlaying;
    private bool isGameOver = false;

    protected KillTracker killTracker;
    protected ScoreTracker scoreTracker;

    private Dictionary<int, PlayerScoreDisplay> playerScoreDisplays;

    private TimerDisplay timerDisplay;

	// Use this for initialization
	void Start () {

    }

    public virtual void SetupGame() {

        gameManager = GameManager.Instance;
        gameManager.LoadMap(initialMapPrefab);
        gameManager.CreatePlayers();
        SetupPlayers();

        killTracker = new KillTracker();
        scoreTracker = new ScoreTracker();

        GameObject canvas = GameObject.Find("Canvas");
        timerDisplay = Instantiate(timerDisplayPrefab).GetComponent<TimerDisplay>();
        RectTransform timerTransform = timerDisplay.GetComponent<RectTransform>();
        timerDisplay.transform.SetParent(canvas.transform);
        timerDisplay.transform.localScale = Vector2.one;
        timerTransform.anchoredPosition = new Vector2(0, -40);

    }
	
	// Update is called once per frame
	void Update () {

        if (isPlaying) {

            time -= Time.deltaTime;
            timerDisplay.SetTime(time);

            if (time <= 0) {
                EndGame();
            }

        }

        if (isGameOver) {

            if (Input.GetKeyDown(KeyCode.Escape)) {
                gameManager.LoadMainMenu();
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                gameManager.PlayAgain();
            }

        }

	}

    public virtual void SetupPlayers() {

        Dictionary<int, ShipController> _players = gameManager.Players;
        playerScoreDisplays = new Dictionary<int, PlayerScoreDisplay>();

        for (int i = 0; i < _players.Count; i++) {

            ShipController player = _players[i];

            PlayerScoreDisplay psd = Instantiate(playerScoreDisplayPrefab).GetComponent<PlayerScoreDisplay>();
            psd.transform.SetParent(playerScoreCanvasGroup);
            psd.transform.localScale = Vector2.one;

            psd.SetColor(player.GetColor(ShipColor.Type.Highlight));
            psd.SetPlayerText("P" + (i + 1));
            psd.SetScoreText(0);

            playerScoreDisplays.Add(i, psd);

        }

    }

    public void StartCountdown() {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown() {

        yield return new WaitForSeconds(1);

        GameObject canvas = GameObject.Find("Canvas");

        int count = 4;
        for (int i = 0; i < count; i++) {

            GameObject number = Instantiate(countdownNumberPrefab);
            number.transform.SetParent(canvas.transform, false);
            number.GetComponent<CountdownNumber>().SetNumber(i);

            if (i == count - 1) {
                gameManager.UnlockPlayers();
                isPlaying = true;
            }

            yield return new WaitForSeconds(1);

            Destroy(number);

        }

        yield return null;

    }

    public virtual void NotifyPlayerDamaged(GameEventPlayerDamaged _e) {
        killTracker.ProcessPlayerDamaged(_e);
    }

    public virtual void NotifyPlayerDeath(GameEventPlayerDeath _e) {

        int eliminatorId = killTracker.ProcessPlayerDeath(_e);
        if (eliminatorId != -1) {
            scoreTracker.IncrementScore(eliminatorId, 1);
        } else {
            scoreTracker.IncrementScore(_e.receiverId, -1);
        }
        UpdateDisplay();

        GameManager.Instance.GetPlayer(_e.receiverId).Respawn();

    }

    private void UpdateDisplay() {
        for (int i = 0; i < playerScoreDisplays.Count; i++) {
            PlayerScoreDisplay psd = playerScoreDisplays[i];
            psd.SetScoreText(scoreTracker.GetScore(i));
        }
    }

    private void EndGame() {

        isPlaying = false;
        isGameOver = true;

        gameManager.LockPlayers();

        GameObject canvas = GameObject.Find("Canvas");
        ShipController winner = GetWinner();

        EndGamePanel egp = Instantiate(endGamePanelPrefab).GetComponent<EndGamePanel>();
        egp.transform.SetParent(canvas.transform, false);
        egp.SetWinner(winner.playerId + 1, winner.GetColor(ShipColor.Type.Main));

    }

    private ShipController GetWinner() {
        int winnerScore = int.MinValue;
        ShipController winner = null;
        foreach (ShipController player in gameManager.Players.Values) {
            int id = player.playerId;
            int score = scoreTracker.GetScore(id);
            if (score > winnerScore) {
                winnerScore = score;
                winner = player;
            }
        }
        return winner;
    }

}
