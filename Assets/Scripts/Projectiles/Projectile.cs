using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Projectile Settings")]
    public float lifespan;
    public ShipThrusters shipThrusters;

    private float age = 0;
    private Rigidbody2D rBody;

    private void Start() {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        age += Time.deltaTime;
        shipThrusters.GenerateThrusterParticles();
        this.transform.up = rBody.velocity;
        if (age >= lifespan) {
            Destroy(this.gameObject);
        }
    }

}
