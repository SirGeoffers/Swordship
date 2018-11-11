using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstParticle : MonoBehaviour {

    private LineRenderer lineRenderer;
    private Vector2 origin;
    private Vector2 velocity;
    private float lifetime;
    private float timeAlive = 0;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        timeAlive += Time.deltaTime;
        if (timeAlive > lifetime) {
            Destroy(this.gameObject);
        }

        lineRenderer.SetPosition(1, origin + velocity * timeAlive);

        float halflife = lifetime / 2f;
        float ratio = (timeAlive - halflife) / halflife;
        if (ratio > 0) {
            Vector2 currPos = origin + velocity * Mathf.Lerp(0, lifetime, ratio);
            lineRenderer.SetPosition(0, currPos);
        }

    }

    public void Setup(Vector2 _origin, Vector2 _direction, float _speed, float _distanceToTravel) {

        lineRenderer = GetComponent<LineRenderer>();

        origin = _origin;
        velocity = _direction * _speed;
        lifetime = _distanceToTravel / _speed;

        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.numCapVertices = Random.Range(0, 2);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, _origin);
        lineRenderer.SetPosition(1, _origin);

    }

}
