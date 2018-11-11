using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour {

    public GameObject[] squares;
    public GameObject chargingParticlePrefab;
    public GameObject explosionPrefab;

    public float runtime;
    public AnimationCurve speedCurve;
    public AnimationCurve distanceCurve;
    public float chargingParticleRate;

    private float timer;
    private float chargingParticleTimer;
    public GameObject prefabToSpawn;

    private float rotationOffset;
    private SpawnManager managerToNotify;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer >= runtime) {
            SpawnObject();
        }

        UpdateSquarePositions();
        UpdateChargingParticles();

	}

    private void UpdateSquarePositions() {

        float ratio = timer / runtime;

        float speed = speedCurve.Evaluate(ratio);
        rotationOffset += Time.deltaTime * speed;

        float distance = distanceCurve.Evaluate(ratio);

        for (int i = 0; i < squares.Length; i++) {

            GameObject sq = squares[i];

            float angle = (Mathf.PI * 2 * (float)i / (float)squares.Length) + rotationOffset;
            float xPos = distance * Mathf.Cos(angle);
            float yPos = distance * Mathf.Sin(angle);
            Vector2 pos = new Vector2(xPos, yPos);
            Vector3 rot = new Vector3(0, 0, angle * Mathf.Rad2Deg);

            sq.transform.localPosition = pos;
            sq.transform.localRotation = Quaternion.Euler(rot);

        }

    }

    private void UpdateChargingParticles() {

        chargingParticleTimer += Time.deltaTime;
        while (chargingParticleTimer > chargingParticleRate) {
            chargingParticleTimer -= chargingParticleRate;
            CreateChargingParticle();
        }

    }

    private void CreateChargingParticle() {

        float ratio = timer / runtime;
        float squareDistance = distanceCurve.Evaluate(ratio);

        Vector2 position = this.transform.position;
        float distance = squareDistance + Random.Range(0.5f, 1f);
        float dir = Random.Range(0, 2f * Mathf.PI);
        Vector2 dirVec = new Vector2(Mathf.Cos(dir), Mathf.Sin(dir));

        BurstParticle bp = Instantiate(chargingParticlePrefab).GetComponent<BurstParticle>();
        bp.transform.SetParent(this.transform);
        bp.Setup(position - dirVec * distance, dirVec, 2f, distance * Random.Range(0.5f, 1f));

    }

    public void SetPrefabToSpawn(GameObject _prefab, SpawnManager _managerToNotify = null) {
        prefabToSpawn = _prefab;
        managerToNotify = _managerToNotify;
    }

    private void SpawnObject() {

        GameObject o = Instantiate(prefabToSpawn, this.transform.position, Quaternion.identity);

        Rigidbody2D rb = o.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.angularVelocity = Random.Range(800, 1000);
        }

        Explosion e = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity).GetComponent<Explosion>();
        e.IgnoreTarget(o);

        if (managerToNotify != null) {
            managerToNotify.AddObjectToTrack(o);
        }

        Destroy(this.gameObject);

    }

}
