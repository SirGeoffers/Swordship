using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    private List<Transform> playerSpawns;
    private List<Transform> itemSpawns;

	public void Setup() {
        GatherSpawns();
    }

    public Transform GetPlayerSpawn(int _playerIndex) {
        return playerSpawns[_playerIndex];
    }

    public Transform GetRandomItemSpawn() {
        int spawnIndex = Random.Range(0, itemSpawns.Count);
        return itemSpawns[spawnIndex];
    }

    private void GatherSpawns() {

        playerSpawns = new List<Transform>();
        itemSpawns = new List<Transform>();

        PlayerSpawn[] playerSpawnList = this.gameObject.GetComponentsInChildren<PlayerSpawn>();
        foreach (PlayerSpawn pSpawn in playerSpawnList) {
            playerSpawns.Add(pSpawn.transform);
        }

        ItemSpawn[] itemSpawnList = this.gameObject.GetComponentsInChildren<ItemSpawn>();
        foreach (ItemSpawn iSpawn in itemSpawnList) {
            itemSpawns.Add(iSpawn.transform);
        }

        Debug.Log(playerSpawns.Count);
        Debug.Log(itemSpawns.Count);

    }

}
