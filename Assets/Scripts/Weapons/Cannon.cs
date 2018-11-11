using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Weapon {

    [Header("Cannon Settings")]
    public float shipImpulse;
    public float projectileImpulse;
    public GameObject projectilePrefab;
    public float spawnOffset;

    public override void Use() {
        FireCannon();
        base.Use();
    }

    private void FireCannon() {

        Vector2 shipImpulseForce = -owner.Forward2D * shipImpulse;
        owner.Rigidbody.AddForce(shipImpulseForce, ForceMode2D.Impulse);

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = this.transform.position + this.transform.up * spawnOffset;

        Vector2 projectileImpulseForce = this.transform.up * projectileImpulse;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectileImpulseForce, ForceMode2D.Impulse);

    }

}
