using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBurstManager : MonoBehaviour {

    [SerializeField]
    private GameObject burstPrefab;

    [SerializeField]
    private float minImpactVelocity = 2f;

    private static CollisionBurstManager collisionBurstManager;
    public static CollisionBurstManager Instance {
        get {
            if (collisionBurstManager == null) {
                collisionBurstManager = GameObject.Find("Collision Bursts").GetComponent<CollisionBurstManager>();
            }
            return collisionBurstManager;
        }
    }

    public void RequestBurst(GameObject _requester, Collision2D _collision) {

        Vector2 collisionVelocity = _collision.relativeVelocity;
        if (collisionVelocity.magnitude < minImpactVelocity) return;

        ContactPoint2D[] contacts = new ContactPoint2D[100];
        _collision.GetContacts(contacts);

        Vector2 origin = contacts[0].point;

        Vector2 contactNormal = contacts[0].normal;
        float contactImpulse = contacts[0].normalImpulse;
        
        CreateBurst(origin, contactNormal * contactImpulse);

    }

    public void CreateBurst(Vector2 _origin, Vector2 _collisionVelocity) {
        CollisionBurst burst = Instantiate(burstPrefab).GetComponent<CollisionBurst>();
        burst.transform.SetParent(this.transform);
        burst.Burst(_origin, _collisionVelocity, this.transform);
    }

}
