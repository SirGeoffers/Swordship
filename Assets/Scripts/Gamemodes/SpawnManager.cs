using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject spawnerPrefab;
    public GameObject[] spawnedPrefabs;
    public GameObject spawnLocationsHolder;

    private Transform[] spawnLocations;
    private List<GameObject> activeWeaponGameobjects = new List<GameObject>();
    private int targetNumWeapons;
    private bool spawningWeapon;

	// Use this for initialization
	void Start () {

        spawnLocations = spawnLocationsHolder.GetComponentsInChildren<Transform>();

        GameObject[] initialActiveWeaponGameobjects = GameObject.FindGameObjectsWithTag("Weapon");
        foreach(GameObject g in initialActiveWeaponGameobjects) {
            activeWeaponGameobjects.Add(g);
        }
        targetNumWeapons = activeWeaponGameobjects.Count;

        StartCoroutine(UpdateWeaponRefs());

	}

    public void AddObjectToTrack(GameObject _o) {
        activeWeaponGameobjects.Add(_o);
    }

    private void SpawnWeapon() {

        int weaponIndex = Random.Range(0, spawnedPrefabs.Length);
        GameObject weaponPrefab = spawnedPrefabs[weaponIndex];

        Vector3 pos = GetSpawnPosition();
        GameObject spawnerObject = Instantiate(spawnerPrefab, pos, Quaternion.identity);
        ObjectSpawn spawner = spawnerObject.GetComponent<ObjectSpawn>();
        spawner.SetPrefabToSpawn(weaponPrefab, this);

    }

    private Vector3 GetSpawnPosition() {
        int spawnIndex = Random.Range(0, spawnLocations.Length);
        return spawnLocations[spawnIndex].position;
    }

    private IEnumerator UpdateWeaponRefs() {

        while (true) {

            for (int i = 0; i < activeWeaponGameobjects.Count; i++) {
                if (activeWeaponGameobjects[i] == null) {
                    activeWeaponGameobjects.RemoveAt(i);
                    i--;
                }
            }

            if (activeWeaponGameobjects.Count < targetNumWeapons) {
                SpawnWeapon();
            }

            yield return new WaitForSeconds(10);

        }

    }

}
