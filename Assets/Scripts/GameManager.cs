using GameEvents;
using Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) instance = GameObject.Find("Game Manager").GetComponent<GameManager>();
            return instance;
        }
    }

    [SerializeField]
    private CameraController cameraController;

    [Header("Player Initialization")]
    public GameObject playerPrefab;
    public ShipColor[] playerColors;
    public Transform[] spawns;
    public Transform thrusterParticleHolder;

    [Header("Gamemode Settings")]
    public Gamemode gamemode;
    public Map currentMap;

    [Header("References")]
    public PauseMenu pauseMenu;

    [Header("Test Settings")]
    public bool useCountdown;

    private List<PlayerSetupData> playerData;
    private Dictionary<int, ShipController> players;
    public Dictionary<int, ShipController> Players {
        get { return players; }
    }

    private void Start() {

        // Reset timescale in case it was changed elsewhere
        Time.timeScale = 1;

        // Get player data
        playerData = VersusSetup.playerSetupData;

        // Create default player for testing
        if (playerData == null) {
            playerData = new List<PlayerSetupData>();
            playerData.Add(new PlayerSetupData() { inputType = "WASD" });
            playerData.Add(new PlayerSetupData() { inputType = "Arrows" });
            //playerData.Add(new PlayerSetupData() { inputType = "Joy1" });
            //playerData.Add(new PlayerSetupData() { inputType = "Joy2" });
        }

        players = new Dictionary<int, ShipController>();
        gamemode.SetupGame();
        
        if (useCountdown) {
            LockPlayers();
            gamemode.StartCountdown();
        }

    }

    private void Update() {

    }

    public void CreatePlayers() {
        for (int i = 0; i < playerData.Count; i++) {
            SpawnPlayer(i);
        }
    }

    public void LoadMap(GameObject _mapPrefab) {
        GameObject mapObject = Instantiate(_mapPrefab);
        currentMap = mapObject.GetComponent<Map>();
        currentMap.Setup();
    }

    public ShipController GetPlayer(int _playerId) {
        if (players.ContainsKey(_playerId)) {
            return players[_playerId];
        }
        return null;
    }

    public void SpawnPlayer(int _playerId) {

        GameObject player = Instantiate(playerPrefab);
        player.name = "Player_" + playerData[_playerId].inputType;

        PlayerController playerController = player.GetComponent<PlayerController>();
        ShipController shipController = player.GetComponent<ShipController>();

        shipController.Init(_playerId, thrusterParticleHolder);

        playerController.SetControls(playerData[_playerId].inputType);
        shipController.SetColor(playerColors[_playerId]);

        shipController.Spawn(currentMap.GetPlayerSpawn(_playerId));

        if (players.ContainsKey(_playerId)) {
            ShipController currentPlayer = players[_playerId];
            if (currentPlayer != null) {
                Destroy(currentPlayer.gameObject);
            }
            players[shipController.playerId] = shipController;
        } else {
            players.Add(shipController.playerId, shipController);
        }

    }

    public void LockPlayers() {
        foreach (ShipController player in players.Values) {
            player.controllable = false;
        }
    }

    public void UnlockPlayers() {
        foreach (ShipController player in players.Values) {
            player.controllable = true;
        }
    }

    public void AddCameraTarget(string _name, Transform _target) {
        cameraController.AddTarget(_name, _target);
    }

    public void RemoveCameraTarget(string _name) {
        cameraController.RemoveTarget(_name);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void PlayAgain() {
        SceneManager.LoadScene(1);
    }

    public void NotifyEvent(GameEvent _e) {

        if (_e is GameEventPlayerDeath) {
            GameEventPlayerDeath e = (GameEventPlayerDeath)_e;
            gamemode.NotifyPlayerDeath(e);
        } else if (_e is GameEventPlayerDamaged) {
            GameEventPlayerDamaged e = (GameEventPlayerDamaged)_e;
            gamemode.NotifyPlayerDamaged(e);
        }

    }

}
