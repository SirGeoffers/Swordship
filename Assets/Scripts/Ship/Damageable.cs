using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    [Header("Damageable Settings")]

    [SerializeField]
    protected float startDurability;

    [SerializeField]
    protected float durability;

    protected bool broken = false;
    public bool IsBroken {
        get {
            return broken;
        }
    }

    protected virtual void Start() {
        ResetDurability();
    }

    public virtual void Damage(float _amount, GameObject _source) {
        durability -= _amount;
        if (durability <= 0) {
            durability = 0;
            if (!broken) Break(_source);
        }
    }

    protected virtual void Break(GameObject _source) {
        broken = true;
    }

    protected virtual void ResetDurability() {
        durability = startDurability;
        broken = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D _collision) {
        
        ContactPoint2D[] contacts = new ContactPoint2D[_collision.contacts.Length];
        _collision.GetContacts(contacts);

        float impactForce = 0;
        foreach (ContactPoint2D cp in contacts) {
            //impactForce += cp.normalImpulse;
            if (cp.normalImpulse > impactForce) impactForce = cp.normalImpulse;
        }
        //impactForce /= (float)contacts.Length;

        // Check if damage should be applied
        DamageScalar ds = _collision.collider.GetComponent<DamageScalar>();
        float impulseScalar = 1;
        if (ds != null) {
            impulseScalar = ds.impulseScalar;
        }

        if (impactForce * impulseScalar < 3) {
            return;
        }

        // Calc damage scale
        float damageScale = 1;
        if (ds != null) {
            damageScale = ds.damageScalar;
        }

        // Apply damage
        float damageToDeal = impactForce * damageScale;
        if (float.IsNaN(damageToDeal) || damageToDeal > startDurability) {
            damageToDeal = startDurability;
        }
        this.Damage(damageToDeal, _collision.gameObject);

        // Particles
        CollisionBurstManager.Instance.RequestBurst(this.gameObject, _collision);

    }

}
