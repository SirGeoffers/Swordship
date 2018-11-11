using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Explosion : MonoBehaviour {

    [Header("Explosion Settings")]
    public float force;
    public float radius;
    public int segments;

    [Header("Debris Settings")]
    public GameObject debrisPrefab;
    public Color debrisColor;
    public int numPieces;
    public Vector2 debrisSizeRange;
    public float debrisLifetime;

    private ParticleSystem ps;
    private List<GameObject> debrisObjects;
    private List<GameObject> ignoredTargets = new List<GameObject>();

	// Use this for initialization
	void Start () {

        ps = GetComponent<ParticleSystem>();
        SpawnDebris();
        ApplyForce();
        
        StartCoroutine(DespawnDebris(debrisLifetime));

    }

    public void IgnoreTarget(GameObject _go) {
        ignoredTargets.Add(_go);
    }

    private void SpawnDebris() {

        debrisObjects = new List<GameObject>();
        for (int i = 0; i < numPieces; i++) {

            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance = Random.Range(0.1f, 0.5f);
            Vector3 spawnPos = this.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;

            GameObject debris = Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
            debris.GetComponent<SpriteRenderer>().color = debrisColor;
            debris.transform.localScale = new Vector3(Random.Range(debrisSizeRange.x, debrisSizeRange.y), Random.Range(debrisSizeRange.x, debrisSizeRange.y), 1);

            debrisObjects.Add(debris);

        }

    }

    private void ApplyForce() {

        for (int i = 0; i < segments; i++) {

            float angle = ((float)i / (float)segments) * Mathf.PI * 2;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, dir, radius);
            foreach (RaycastHit2D hit in hits) {
                
                if (ignoredTargets.Contains(hit.collider.gameObject)) {
                    continue;
                }

                Rigidbody2D rb = hit.collider.attachedRigidbody;
                if (rb == null) continue;

                float distance = hit.distance;
                Vector2 point = hit.point;
                float wearoff = 1 - (distance / radius);
                float forceStrength = force * wearoff;

                rb.AddForceAtPosition(dir * forceStrength, point, ForceMode2D.Impulse);

            }

        }

    }

    private IEnumerator DespawnDebris(float _delay) {

        yield return new WaitForSeconds(_delay);

        foreach(GameObject g in debrisObjects) {
            Destroy(g);
        }

        while (ps.IsAlive()) {
            yield return null;
        }

        Destroy(this.gameObject);

    }

}
