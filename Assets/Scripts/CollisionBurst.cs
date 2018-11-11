using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swordship.Utility;

public class CollisionBurst : MonoBehaviour {

    [SerializeField]
    private GameObject linePrefab = null;

    public void Burst(Vector2 _origin, Vector2 _collisionVelocity, Transform _parentTransform) {

        int numLines = Random.Range(4, 8);
        
        for (int i = 0; i < numLines; i++) {
            
            float rotationDegrees = Random.Range(-60f, 60f);
            Vector2 direction = _collisionVelocity.Rotate(rotationDegrees).normalized;

            float distanceToTravel = Random.Range(1, 3) * _collisionVelocity.magnitude / 8f;
            if (distanceToTravel < 0.5f) {
                distanceToTravel = Random.Range(0.5f, 1f);
            }

            float speed = distanceToTravel * Random.Range(3f, 4f);

            BurstParticle burstParticle = Instantiate(linePrefab).GetComponent<BurstParticle>();
            burstParticle.Setup(_origin, direction, speed, distanceToTravel);
            burstParticle.transform.SetParent(_parentTransform);

        }

        Destroy(this.gameObject);

    }

}
