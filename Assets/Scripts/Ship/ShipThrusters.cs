using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusters : MonoBehaviour {

    [SerializeField]
    private float spawnOffset;

    [SerializeField]
    private GameObject particlePrefab;

    [SerializeField]
    private float particleRate;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float lifetime;

    [SerializeField]
    private float initialSize = 1;
    
    private Transform particleHolder;

    private float lastGenerationTime = 0;

    private void Start() {
        particleHolder = GameManager.Instance.thrusterParticleHolder;
    }

    public void GenerateThrusterParticles() {

        if (Time.time - particleRate > lastGenerationTime) {
            lastGenerationTime = Time.time;
            Vector3 spawnPosition = this.transform.position + this.transform.up * spawnOffset + new Vector3(0, 0, 1);
            Quaternion spawnRotation = Quaternion.Euler(this.transform.up);
            ThrusterParticle p = Instantiate(particlePrefab, spawnPosition, spawnRotation).GetComponent<ThrusterParticle>();
            p.transform.SetParent(particleHolder);
            p.Initialize(-this.transform.up * speed, lifetime, initialSize);
        }

    }

    public void SetHolder(Transform _t) {
        particleHolder = _t;
    }

}
