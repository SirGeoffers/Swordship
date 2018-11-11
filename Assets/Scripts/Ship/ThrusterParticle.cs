using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterParticle : MonoBehaviour {

    private Vector3 velocity;
    private float lifetime;

    private float timeAlive = 0;
    private float initialScale = 1;

    public void Initialize(Vector3 _velocity, float _lifetime, float _size) {
        velocity = _velocity;
        lifetime = _lifetime;
        initialScale = _size;
        this.transform.localScale = Vector3.one * initialScale;
    }

	void Update () {

        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime) {
            Destroy(this.gameObject);
        }

        this.transform.position += velocity * Time.deltaTime;

        float scaleVal = initialScale * (lifetime - timeAlive) / lifetime;
        if (scaleVal < 0) scaleVal = 0;
        Vector3 scale = Vector3.one * scaleVal;
        this.transform.localScale = scale;

	}

}
